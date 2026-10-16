# Repository: PROG6212-POE-PART2
# Race Day Management API
- Student ID: ST10464927
- Name: Kirsten Smith
- Module: PROG6212-Programming 2B
  
# Planning
Part 1 of the POE consists of 3 Parts:

- Section A - ERD
- Section B - API Endpoint Plan
- Section C - SQL Database Script

Part 2 of the POE consists of 6 Requirements:
1. Project Setup
2. Authentication and Role-Based Access
3. Functional Requirements
4. Swagger Integration
5. Unit Testing
6. GitHub and CI/CD


# Part 1 SET UP
- ReadMe file - Work Breakdown and Outputs
- Part 1 Folder:
1. ERD - can be accessed in the ReadMe and docs folder - ERD. png
2. API - can be accessed in the ReadMe and docs folder - API_Endpoint_Plan.pdf
3. SQL Script - can be accessed in the ReadMe and docs folder - RaceDayManagemnetDB_ Script.sql
  
# Section A
In this POE PART 1 - I have created my ERD in Microsoft Visio.
The ERD occupies 6 entities with their functional requirements.

1. User:
Users goes through an authentication, registration and role management process that identifies users as organizers or participants of a specific race day event.

2. Event:
Users can create one or more events as well as searching or finding event names, description, date, location and creation of a specific event ( When it was created).

3. Event Category
The Race Day events offers one or more event categories that users can choose from like the categories name and the entry fee.

4. Enrolment
Each race enrolment yields the results for each event like the participants results for each event category.

5. Route Information
Each event has their own route information that displays their forecast and GPX.

6. Result
Results records all the finish times, overall positions and status of each event.

<img width="1610" height="737" alt="image" src="https://github.com/user-attachments/assets/14ddf4cf-f79c-4892-9728-2fc3947b58b3" />

# Section B

Section B is in a table formatted in word docs that covers all the functionalities across 6 entities.

1. Authentication Endpoints for account registration and login (/api/auth/)
  <img width="885" height="302" alt="image" src="https://github.com/user-attachments/assets/add3750d-050c-4fb1-bd67-abf8e2e5c0c2" />

2. User Profile (/api/users)
<img width="866" height="222" alt="image" src="https://github.com/user-attachments/assets/a607c40a-8199-4d85-b24f-3bc60a2cc3a3" />
<img width="871" height="137" alt="image" src="https://github.com/user-attachments/assets/3d3c2414-30a7-4944-9b48-995943bc5811" />

3. Event (/api/events)
<img width="861" height="447" alt="image" src="https://github.com/user-attachments/assets/4a252c3d-0dfb-40a8-b483-bcae66467e33" />
<img width="847" height="272" alt="image" src="https://github.com/user-attachments/assets/33e6e3b9-935a-4e92-bc2c-faf0f662e4cb" />

4. Category (/api/categories)
<img width="866" height="292" alt="image" src="https://github.com/user-attachments/assets/b89a29fc-2d40-4cb0-88c9-3b1c04ca409c" />
<img width="855" height="327" alt="image" src="https://github.com/user-attachments/assets/fa4d90cb-feff-4691-bc9d-a72a330fae2b" />

5. Enrolment (/api/enrolments)
<img width="847" height="237" alt="image" src="https://github.com/user-attachments/assets/b12e8f70-ca00-4382-963b-40de9fa1b977" />
<img width="862" height="427" alt="image" src="https://github.com/user-attachments/assets/fe8ae020-d595-41b8-ae35-ec5e77db6f27" />

6. Result (/api/results)
<img width="882" height="185" alt="image" src="https://github.com/user-attachments/assets/8c08464e-6d67-4280-942f-83b3af95da15" />
<img width="847" height="342" alt="image" src="https://github.com/user-attachments/assets/6f8ac3fd-aec4-487a-9a88-ee159effd003" />

7. Route Information ( /api/routes/)
<img width="865" height="217" alt="image" src="https://github.com/user-attachments/assets/74e19ce2-f9a3-458c-b87d-88a51bdee95f" />
<img width="850" height="120" alt="image" src="https://github.com/user-attachments/assets/34d34176-d131-493a-af37-6ebf94b51856" />

# Section C
Output of the Race Day Management Database system.
<img width="846" height="446" alt="image" src="https://github.com/user-attachments/assets/adc0bafa-0639-4243-8a20-3581c42ee4fa" />
<img width="1682" height="681" alt="image" src="https://github.com/user-attachments/assets/611739cb-79e1-44bf-a294-95b1ca5663b6" />

# Part 2 
# Project Overview
The Race Day Management API is a Restful web API built using C# and .NET 8.0. It provides a centralized service for managing race day events, participants, and results. The project adheres to clean architecture principles, featuring full unit test coverage and automated Continuous Integration (CI/CD) through GitHub Actions.

# SET UP & Tools
- ReadMe file - Work Breakdown and Outputs
- Part 2 Folder
1. RaceDayMangementAPI.sln
2. RaceDayManagementAPI
3. RaceDayManagementAPI.Tests
- Framework: .NET 8.0 (ASP.NET Core Web API)
- IDE: Microsoft Visual Studio 2022
- Unit Testing: xUnit/NUnit & Moq
- CI/CD: Github Actions
- Versions Control: Git & GitHub

# Running via Visual Studio
- Navigate to Part 2 folder and open RaceDayManagementAPI.sln in Visual Studio 2022.
- Press F5 or click Start Debugging to launch API.
- Access the Interactive Swagger UI in your browser.

# Running Unit Tests Locally
- In Visual Studio, open Test > Test Explorer
- Click Run All Test.

# Screenshot of all unit tests passed
<img width="822" height="401" alt="image" src="https://github.com/user-attachments/assets/8e64d5b5-45f3-448d-8643-e4e1fdf66387" />

# Swagger UI
<img width="1888" height="767" alt="image" src="https://github.com/user-attachments/assets/aa45a028-9efe-4e9c-bc23-daf4ceabac3b" />
<img width="1867" height="823" alt="image" src="https://github.com/user-attachments/assets/49c53488-3e8f-4f02-986c-f96a4a667cfa" />
<img width="1886" height="863" alt="image" src="https://github.com/user-attachments/assets/c4eacdb2-0bde-43e5-a492-b80c81c58fd9" />
<img width="1862" height="896" alt="image" src="https://github.com/user-attachments/assets/298e0092-2fca-4b06-ae6f-c7e6d7968f6f" />


# API Endpoint Documentation
<img width="606" height="374" alt="image" src="https://github.com/user-attachments/assets/05e24206-2e67-47da-b4b7-8664a29b0262" />



# GitHub Actions CI/CD Workflow & Continuous Integration Setup
# PART 1
<img width="1392" height="286" alt="image" src="https://github.com/user-attachments/assets/d6b5db8c-ac52-484c-97d9-8f89660f508f" />
![CI/CD Build Success](./cicd-build.png)

# PART 2
<img width="1178" height="138" alt="image" src="https://github.com/user-attachments/assets/45348f23-9caf-4f0c-9028-e7ca824970da" />


# YouTube Link
https://youtu.be/cNDCuXv7iDU
https://youtu.be/-AbNhpH1okY

# References
Troelsen, A. and Japikse, P. (2021). Pro C# 9 with .NET 5: Foundational Principles and Practices in Programming. 10th edn. New York: Apress.
.

