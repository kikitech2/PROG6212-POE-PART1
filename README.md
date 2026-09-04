# PROG6212-POE-PART1
# Planning
Part 1 of the POE consists of 3 Parts:

- Section A - ERD
- Section B - API Endpoint Plan
- Section C - SQL Database Script

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

Race Day Management SQL Script is in the RaceDayManagementDB file. 

# References

