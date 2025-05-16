-- Create the database
CREATE DATABASE FirstWebAPIProject;
GO

-- Use the database
USE FirstWebAPIProject;
GO

-- Create the Product table
CREATE TABLE Product (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    Price DECIMAL(10,2) NOT NULL,
    Quantity INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

INSERT INTO Product (Name, Description, Price, Quantity)
VALUES 
('Laptop', '15-inch display', 999.99, 10),
('Mouse', 'Wireless optical mouse', 25.50, 50),
('Keyboard', 'Mechanical keyboard', 79.99, 30);