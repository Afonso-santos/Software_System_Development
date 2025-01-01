CREATE DATABASE IF NOT EXISTS SchedulePlanner DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;

USE SchedulePlanner;

CREATE TABLE IF NOT EXISTS User (
    Username VARCHAR(100) NOT NULL,
    Password VARCHAR(100) NOT NULL,
    PRIMARY KEY (Username)
);

CREATE TABLE IF NOT EXISTS Student (
    Num VARCHAR(20) NOT NULL,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Statute BIT(2) NOT NULL,
    Year INT NOT NULL,
    Semester INT NOT NULL,
    Course VARCHAR(100) NOT NULL,
    PartialMean FLOAT NOT NULL,
    Username VARCHAR(100) NOT NULL,
    PRIMARY KEY (Num),
    FOREIGN KEY (Username) REFERENCES User(Username)
);

CREATE TABLE IF NOT EXISTS Classroom (
    Num VARCHAR(50) NOT NULL,
    Capacity INT NOT NULL,
    PRIMARY KEY (Num)
);

CREATE TABLE IF NOT EXISTS Course (
    Code VARCHAR(50) NOT NULL,
    Name VARCHAR(100) NOT NULL,
    PRIMARY KEY (Code)
);

CREATE TABLE IF NOT EXISTS Shift (
    Num INT NOT NULL,
    Type ENUM('T', 'TP', 'P') NOT NULL,
    Day ENUM('Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday') NOT NULL,
    Hour TIME NOT NULL,
    `Limit` INT,
    Course VARCHAR(50) NOT NULL,
    Classroom VARCHAR(50) NOT NULL,
    PRIMARY KEY (Num),
    FOREIGN KEY (Course) REFERENCES Course(Code),
    FOREIGN KEY (Classroom) REFERENCES Classroom(Num)
);

CREATE TABLE IF NOT EXISTS UC (
    Code VARCHAR(50) NOT NULL,
    Name VARCHAR(100) NOT NULL,
    Course VARCHAR(50) NOT NULL,
    Preference VARCHAR(200),
    PRIMARY KEY (Code),
    FOREIGN KEY (Course) REFERENCES Course(Code)
);

CREATE TABLE IF NOT EXISTS Enrollment (
    Student VARCHAR(20) NOT NULL,
    UC VARCHAR(50) NOT NULL,
    Shift INT NOT NULL,
    PRIMARY KEY (Student, UC, Shift),
    FOREIGN KEY (Student) REFERENCES Student(Num),
    FOREIGN KEY (UC) REFERENCES UC(Code),
    FOREIGN KEY (Shift) REFERENCES Shift(Num)
);

CREATE TABLE IF NOT EXISTS Schedule (
    Student VARCHAR(20) NOT NULL,
    Shift INT NOT NULL,
    PRIMARY KEY (Student, Shift),
    FOREIGN KEY (Student) REFERENCES Student(Num),
    FOREIGN KEY (Shift) REFERENCES Shift(Num)
);

-- Create the admin user and assign privileges
CREATE USER 'admin'@'localhost' IDENTIFIED BY 'admin123';
GRANT ALL PRIVILEGES ON *.* TO 'admin'@'localhost' WITH GRANT OPTION;
