CREATE DATABASE RaceDayManagement;

DROP TABLE IF EXISTS[Result];
DROP TABLE IF EXISTS[Enrolment];
DROP TABLE IF EXISTS[RouteInfo];
DROP TABLE IF EXISTS[EventCategory];
DROP TABLE IF EXISTS[Event];
DROP TABLE IF EXISTS[User];


-- Table Statements for every entity in the ERD.
-- Table 1: User
CREATE TABLE [User] (
UserID INT IDENTITY(1,1) PRIMARY KEY,
FirstName VARCHAR(50) NOT NULL,
LastName VARCHAR(50) NOT NULL,
Email VARCHAR(100) NOT NULL UNIQUE,
PasswordHash VARCHAR(255) NOT NULL,
Role VARCHAR(20) NOT NULL Check (Role IN ('Organiser', 'Participant')),
RegistrationDate DATETIME NOT NULL DEFAULT GETDATE()
);

-- Table 2: Event
CREATE TABLE Event (
EventID INT IDENTITY(1,1) PRIMARY KEY,
OrganiserID INT NOT NULL,
Title VARCHAR(100) NOT NULL,
Description VARCHAR(MAX) NULL,
EventDate DATETIME NOT NULL,
Location VARCHAR(150) NOT NULL,
CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
CONSTRAINT FK_Event_User FOREIGN KEY (OrganiserID)
REFERENCES [User](UserID) ON DELETE CASCADE
);

-- Table 3: EventCategory
CREATE TABLE EventCategory (
CategoryID INT IDENTITY(1,1) PRIMARY KEY,
EventID INT NOT NULL,
CategoryName VARCHAR(50) NOT NULL,
DistanceKm DECIMAL(5,2) NOT NULL CHECK (DistanceKm > 0),
EntryFee DECIMAL(10,2) NOT NULL CHECK (EntryFee >= 0),
MaxParticipants INT NOT NULL CHECK (MaxParticipants > 0),
CONSTRAINT FK_EventCategory_Event FOREIGN KEY (EventID)
REFERENCES [Event](EventID) ON DELETE CASCADE
);

-- Table 4: RouteInfo
CREATE TABLE RouteInfo (
RouteID INT IDENTITY(1,1) PRIMARY KEY,
EventID INT NOT NULL UNIQUE,
GPXDataUrl VARCHAR(2083) NULL,
ElevationGainMeters INT NOT NULL CHECK (ElevationGainMeters >=0),
ForecastSummary VARCHAR(255) NULL,
CONSTRAINT FK_RouteInfo_Event FOREIGN KEY (EventID)
REFERENCES [Event](EventID) ON DELETE CASCADE
);

-- Table 5: Enrolment
CREATE TABLE Enrolment (
EnrolmentID INT IDENTITY(1,1) PRIMARY KEY,
CategoryID INT NOT NULL,
ParticipantID INT NOT NULL,
EnrolmentDate DATETIME NOT NULL DEFAULT GETDATE(),
EnrolmentStatus VARCHAR(20) NOT NULL DEFAULT 'Confirmed'
CHECK (EnrolmentStatus IN ('Confirmed','Cancelled','Pending')),
CONSTRAINT FK_Enrolment_EventCategory FOREIGN KEY (CategoryID)
REFERENCES EventCategory(CategoryID) ON DELETE CASCADE,
CONSTRAINT FK_Enrolment_USER FOREIGN KEY (ParticipantID)
REFERENCES [User](UserID) ON DELETE NO ACTION,
CONSTRAINT UQ_Participant_Category UNIQUE (CategoryID, ParticipantID)
);

-- Table 6: Result
CREATE TABLE Result (
ResultID INT IDENTITY(1,1) PRIMARY KEY,
EnrolmentID INT NOT NULL UNIQUE,
CategoryPosition INT NOT NULL CHECK (CategoryPosition > 0),
OverallPosition INT NOT NULL CHECK (OverallPosition > 0),
CompletionTime TIME(7) NOT NULL,
Status VARCHAR(20) NOT NULL DEFAULT 'Finished'
CHECK (Status IN ('Finished', 'DNF','DNS', 'Disqualified')),
CONSTRAINT FK_Result_Enrolment FOREIGN KEY (EnrolmentID)
REFERENCES Enrolment(EnrolmentID) ON DELETE CASCADE
);

-- INSERT statments to populate the users of the RaceDay Management DB
-- ( Passwords are stored as hashed values in the application).
INSERT INTO [User] (FirstName,LastName,Email,PasswordHash,Role)
VALUES
('Kirsten', 'Smith', 'organiser@events.com', 'AQAAAAEAACcQAAAAEHashedPassword123!', 'Organiser'),
('Diesel', 'Manger', 'Diesel.runner@gmail.com', 'AQAAAAEAACcQAAAAEHashedPassword771!', 'Participant'),
('Summer', 'Lynn', 'summer.marathon@gmail.com', 'AQAAAAEAACcQAAAAEHashedPassword367!', 'Participant');

-- INSERT statments to populate the events of the RaceDay Management DB
INSERT INTO Event (OrganiserID, Title, Description, EventDate, Location)
VALUES
(1, 'Cape Peninsula Marathon 2026', 'Annual coastal road running race across Cape Town.', '2026-9-19 08:00:00','Cape Town, South Africa'),
(1, 'Table Mountain Trail Challenge', 'Scenic off-road trail race up Table Mountain.', '2026-11-20 08:00:00','Cape Town, South Africa');

-- INSERT statments to populate the event category of the RaceDay Management DB
INSERT INTO EventCategory (EventID, CategoryName, DistanceKm, EntryFee, MaxParticipants)
VALUES
(1, 'Full Marathon', 39.20, 250.00, 600),
(1, 'Half Marathon', 25.10, 150.00, 750),
(2, '10km Trail Run', 10.00, 200.00, 800);

-- INSERT statments to populate the route info of the RaceDay Management DB
INSERT INTO RouteInfo (EventID, GPXDataUrl, ElevationGainMeters, ForecastSummary)
VALUES
(1, 'https://storage.events.com/gpx/peninsula2026.gpx', 150, 'Partly Cloudy, 18 Degrees, Moderate breeze'),
(2, 'https://storage.events.com/gpx/tablemtn2026.gpx', 820, 'Clear Sky, 22 Degrees, Light Wind');

-- INSERT statments to populate the enrolments of the RaceDay Management DB
INSERT INTO Enrolment (CategoryID, ParticipantID, EnrolmentStatus)
VALUES
(1, 2, 'Confirmed'),
(2, 3, 'Confirmed');

-- INSERT statments to populate the results of the RaceDay Management DB
INSERT INTO Result (EnrolmentID, CategoryPosition, OverallPosition, CompletionTime, Status)
VALUES
(1, 12, 45, '03:19:23', 'Finished'),
(2, 5, 18, '02:46:03', 'Finished');

SELECT * FROM [User];
SELECT * FROM [Event];
SELECT * FROM [EventCategory];
SELECT * FROM [RouteInfo];
SELECT * FROM [Enrolment];
SELECT * FROM [Result];
