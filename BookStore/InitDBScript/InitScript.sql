-- 创建数据库
CREATE DATABASE IF NOT EXISTS onlinebookstore;
USE onlinebookstore;

-- 创建用户表
CREATE TABLE IF NOT EXISTS Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(100) NOT NULL,
    Password VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 创建图书表
CREATE TABLE IF NOT EXISTS Books (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Author VARCHAR(255) NOT NULL,
    ISBN VARCHAR(13) NOT NULL,
    Stock INT NOT NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 创建订单表
CREATE TABLE IF NOT EXISTS Orders (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT NOT NULL,
    BookId INT NOT NULL,
    OrderDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (BookId) REFERENCES Books(Id)
);

-- 插入用户示例数据
INSERT INTO Users (Username, Password, Email) VALUES
('john_doe', 'password123', 'john@example.com'),
('jane_doe', 'password123', 'jane@example.com');

-- 插入图书示例数据
INSERT INTO Books (Title, Author, ISBN, Stock) VALUES
('The Great Gatsby', 'F. Scott Fitzgerald', '9780743273565', 10),
('1984', 'George Orwell', '9780451524935', 15),
('To Kill a Mockingbird', 'Harper Lee', '9780061120084', 8);

-- 插入订单示例数据
INSERT INTO Orders (UserId, BookId) VALUES
(1, 1),
(1, 2),
(2, 3);
