USE master
GO

IF DB_ID('College1en') IS NOT NULL
BEGIN
    ALTER DATABASE College1en SET SINGLE_USER WITH ROLLBACK IMMEDIATE
    DROP DATABASE College1en
END
GO

CREATE DATABASE College1en
GO

USE College1en
GO

-- Create Programs table
CREATE TABLE Programs (
    ProgId VARCHAR(5) NOT NULL PRIMARY KEY,
    ProgName VARCHAR(50) NOT NULL
)
GO

-- Create Courses table
CREATE TABLE Courses (
    CId VARCHAR(7) NOT NULL PRIMARY KEY,
    CName VARCHAR(50) NOT NULL,
    ProgId VARCHAR(5) NOT NULL,
    CONSTRAINT FK_Courses_Programs FOREIGN KEY (ProgId)
    REFERENCES Programs(ProgId)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
)
GO

-- Create Students table
CREATE TABLE Students (
    StId VARCHAR(10) NOT NULL PRIMARY KEY,
    StName VARCHAR(50) NOT NULL,
    ProgId VARCHAR(5) NOT NULL,
    CONSTRAINT FK_Students_Programs FOREIGN KEY (ProgId)
    REFERENCES Programs(ProgId)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
)
GO

-- Create Enrollments table
CREATE TABLE Enrollments (
    StId VARCHAR(10) NOT NULL,
    CId VARCHAR(7) NOT NULL,
    FinalGrade INT,
    PRIMARY KEY (StId, CId),
    CONSTRAINT FK_Enrollments_Students FOREIGN KEY (StId)
    REFERENCES Students(StId)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
    CONSTRAINT FK_Enrollments_Courses FOREIGN KEY (CId)
    REFERENCES Courses(CId)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
)
GO


INSERT INTO Programs (ProgId, ProgName)
VALUES ('P0001', 'Computer Science'),
       ('P0002', 'Mathematics')

-- Insert data into Courses table
INSERT INTO Courses (CId, CName, ProgId)
VALUES ('C000001', 'Introduction to Computer Science', 'P0001'),
       ('C000002', 'Data Structures and Algorithms', 'P0001'),
       ('C000003', 'Calculus I', 'P0002'),
       ('C000004', 'Calculus II', 'P0002')

-- Insert data into Students table
INSERT INTO Students (StId, StName, ProgId)
VALUES ('S000000001', 'John Doe', 'P0001'),
       ('S000000002', 'Jane Smith', 'P0002')

-- Insert data into Enrollments table
INSERT INTO Enrollments (StId, CId, FinalGrade)
VALUES ('S000000001', 'C000001', 90),
       ('S000000001', 'C000002', 85),
       ('S000000002', 'C000003', 95),
       ('S000000002', 'C000004', 80)
GO