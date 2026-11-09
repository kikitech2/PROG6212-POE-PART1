using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RaceDayManagementAPI.Services
{
    public class BlobService : IBlobService
    {
        // 1. Make the field nullable to resolve CS8618
        private readonly BlobContainerClient? _containerClient;

        public BlobService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            var containerName = configuration["AzureBlobStorage:ContainerName"];

            try
            {
                if (!string.IsNullOrEmpty(connectionString) && !string.IsNullOrEmpty(containerName))
                {
                    var blobServiceClient = new BlobServiceClient(connectionString);
                    _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                    _containerClient.CreateIfNotExists(PublicAccessType.Blob);
                }
            }
            catch (Exception ex)
            {
                // Safely bypasses local Azure connection failure so offline testing works
                System.Diagnostics.Debug.WriteLine($"Blob storage offline: {ex.Message}");
            }
        }

        // 2. Change return type to Task<string?> to resolve CS8603
        public async Task<string?> UploadFileAsync(IFormFile file)
        {
            // 3. Guard against null file or uninitialized container (offline mode)
            if (file == null || file.Length == 0 || _containerClient == null)
                return null;

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var blobClient = _containerClient.GetBlobClient(fileName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });
            }

            return blobClient.Uri.ToString();
        }

        public async Task<bool> DeleteFileAsync(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl) || _containerClient == null)
                return false;

            var uri = new Uri(fileUrl);
            var fileName = Path.GetFileName(uri.LocalPath);
            var blobClient = _containerClient.GetBlobClient(fileName);

            return await blobClient.DeleteIfExistsAsync();
        }
    }
}
