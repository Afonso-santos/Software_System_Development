DROP DATABASE SchedulePlanner;
CREATE DATABASE IF NOT EXISTS SchedulePlanner DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;

USE SchedulePlanner;

CREATE TABLE IF NOT EXISTS User (
    Username VARCHAR(100) NOT NULL,
    Password VARCHAR(100) NOT NULL,
    Admin    BIT(1)       NOT NULL,
    PRIMARY KEY (Username)
);

CREATE TABLE IF NOT EXISTS Course (
    Name VARCHAR(100) NOT NULL,
    PRIMARY KEY (Name)
);

CREATE TABLE IF NOT EXISTS UC (
    Code VARCHAR(50) NOT NULL,
    Name VARCHAR(100) NOT NULL,
    Course VARCHAR(100) NOT NULL,
    Preference VARCHAR(200),
    CourseYear INT NOT NULL,
    Semester INT NOT NULL,
    PRIMARY KEY (Code),
    FOREIGN KEY (Course) REFERENCES Course(Name)
);

CREATE TABLE IF NOT EXISTS Classroom (
    Number VARCHAR(50) NOT NULL,
    Capacity INT NOT NULL,
    PRIMARY KEY (Number)
);

CREATE TABLE IF NOT EXISTS Shift (
    Num INT NOT NULL,
    Type ENUM('T', 'TP', 'PL') NOT NULL,
    UC VARCHAR(50) NOT NULL,
    Day ENUM('Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday') NOT NULL,
    StartingHour TIME NOT NULL,
    EndingHour TIME NOT NULL,
    `Limit` INT,
    Classroom VARCHAR(50) NOT NULL,

    PRIMARY KEY (Type, Num, UC),
    UNIQUE KEY (Num, Type, UC),

    FOREIGN KEY (UC) REFERENCES UC(Code),
    FOREIGN KEY (Classroom) REFERENCES Classroom(Number)
);

CREATE TABLE IF NOT EXISTS Student (
    Num VARCHAR(20) NOT NULL,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Statute BIT(2) NOT NULL,
    Year INT NOT NULL,
    Course VARCHAR(50) NOT NULL,
    PartialMean FLOAT NOT NULL,

    PRIMARY KEY (Num),
    FOREIGN KEY (Num) REFERENCES User(Username),
    FOREIGN KEY (Course) REFERENCES Course(Name)
);

CREATE TABLE IF NOT EXISTS Enrollment (
    Student VARCHAR(20) NOT NULL,
    
    ShiftNum INT NOT NULL,
    ShiftType ENUM('T', 'TP', 'P') NOT NULL,
    ShiftUC VARCHAR(50) NOT NULL,

    PRIMARY KEY (Student, ShiftNum, ShiftType, ShiftUC),
    
    FOREIGN KEY (Student) REFERENCES Student(Num),
    FOREIGN KEY (ShiftNum, ShiftType, ShiftUC) REFERENCES Shift(Num, Type, UC)
);

-- Create the admin user and assign privileges
CREATE USER IF NOT EXISTS 'admin'@'localhost' IDENTIFIED BY 'admin123';
GRANT ALL PRIVILEGES ON *.* TO 'admin'@'localhost' WITH GRANT OPTION;
