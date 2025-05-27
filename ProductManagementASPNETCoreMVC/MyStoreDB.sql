USE master
GO

CREATE DATABASE MyStoreDB
GO

USE MyStoreDB
GO

CREATE TABLE AccountMember (
  MemberID nvarchar(20) primary key,
  MemberPassword nvarchar(80) not null,
  EmailAddress nvarchar(100) unique not null, 
  FullName nvarchar(80) not null,
  MemberRole int not null
)
GO

INSERT INTO AccountMember VALUES(N'A01' ,N'@1','admin@gmail.com', N'System Admin', 1);
INSERT INTO AccountMember VALUES(N'A02' ,N'@1','manager@gmail.com', N'Manager', 2);
INSERT INTO AccountMember VALUES(N'A03' ,N'@1','staff@gmail.com', N'Staff', 3);
INSERT INTO AccountMember VALUES(N'A04' ,N'@1','member1@gmail.com', N'Member 1', 4);
GO


CREATE TABLE Categories (
  CategoryID int primary key,
  CategoryName nvarchar(15) not null
)
GO

INSERT INTO Categories VALUES(1, N'Makeup')
GO
INSERT INTO Categories VALUES(2, N'Skincare')
GO
INSERT INTO Categories VALUES(3, N'Body Care')
GO
INSERT INTO Categories VALUES(4, N'Hair Care')
GO
INSERT INTO Categories VALUES(5, N'Fragrance')
GO

CREATE TABLE Products (
  ProductID int PRIMARY KEY,
  ProductName nvarchar(40) not null,
  UnitsInStock smallint,
  UnitPrice money, 
  CategoryID int FOREIGN KEY references Categories(CategoryID) on delete cascade on update cascade,
)
GO


INSERT INTO Products VALUES(1, N'Maybelline + Poreless Foundation', 30, 10000, 1)
GO
INSERT INTO Products VALUES(2, N'NARS Blush', 12, 15000, 1)
GO
INSERT INTO Products VALUES(3, N'CeraVe Hydrating Facial Cleanser', 13, 27000, 2)
GO
INSERT INTO Products VALUES(4, N'The Ordinary Niacinamide 10% + Zinc 1%', 22, 30000, 2)
GO
INSERT INTO Products VALUES(5, N'Vaseline Intensive Care Lotion', 28, 3000, 3)
GO
INSERT INTO Products VALUES(6, N'Neutrogena Hydro Boost Water Gel', 44, 15000, 3)
GO
INSERT INTO Products VALUES(7, N'Lush Sugar Plum Fairy Shower Gel', 6, 10500, 4)
GO
INSERT INTO Products VALUES(8, N'St. Ives Fresh Skin Apricot Scrub', 18, 20000, 4)
GO
INSERT INTO Products VALUES(9, N'Eucerin Advanced Repair Cream', 51, 35000, 4)
GO



