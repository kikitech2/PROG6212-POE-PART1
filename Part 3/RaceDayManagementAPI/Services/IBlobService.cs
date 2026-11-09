using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RaceDayManagementAPI.Services
{
    public interface IBlobService
    {
        Task<string?> UploadFileAsync(IFormFile file);
        Task<bool> DeleteFileAsync(string fileUrl);
    }
}
