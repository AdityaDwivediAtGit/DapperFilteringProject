Create database LearningFiltersShoppingDemoDB;

USE LearningFiltersShoppingDemoDB;

CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL
);

INSERT INTO Categories (CategoryId, Name) VALUES
(1, 'Electronics'),
(2, 'Clothing'),
(3, 'Books'),
(4, 'Appliances'),
(5, 'Accessories');

CREATE TABLE Products (
    ProductId INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    CategoryId INT NOT NULL,
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId)
);
--Drop table Products;

INSERT INTO Products (Name, Price, CategoryId) VALUES
('Laptop', 999.99, 1),
('T-Shirt', 19.99, 2),
('Java Programming', 39.99, 3),
('Smartphone', 699.99, 1),
('Jeans', 29.99, 2),
('C# Programming', 49.99, 3),
('Tablet', 299.99, 1),
('Dress', 49.99, 2),
('Python Programming', 59.99, 3),
('Headphones', 49.99, 1),
('Mouse', 19.99, 1),
('Keyboard', 29.99, 1),
('Monitor', 199.99, 1),
('Speaker', 99.99, 1),
('Hoodie', 39.99, 2),
('Socks', 9.99, 2),
('Shoes', 59.99, 2),
('Hat', 14.99, 2),
('Gloves', 9.99, 2),
('Database Design', 29.99, 3),
('Web Development', 39.99, 3),
('Algorithms', 49.99, 3),
('Machine Learning', 59.99, 3),
('Operating Systems', 39.99, 3),
('TV', 499.99, 1),
('Camera', 399.99, 1),
('Smartwatch', 199.99, 1),
('Fitness Tracker', 99.99, 1),
('Printer', 149.99, 1),
('Jacket', 79.99, 2),
('Pants', 39.99, 2),
('Skirt', 29.99, 2),
('Scarf', 19.99, 2),
('Sweater', 49.99, 2),
('Computer Science Basics', 19.99, 3),
('Data Structures', 29.99, 3),
('Software Engineering', 39.99, 3),
('Artificial Intelligence', 49.99, 3),
('Cybersecurity', 39.99, 3),
('Microwave', 79.99, 1),
('Blender', 29.99, 1),
('Toaster', 19.99, 1),
('Coffee Maker', 49.99, 1),
('Vacuum Cleaner', 99.99, 1),
('Backpack', 29.99, 2),
('Wallet', 19.99, 2),
('Belt', 14.99, 2),
('Tie', 9.99, 2),
('Dumbbell', 19.99, 1),
('Yoga Mat', 24.99, 1);

---- Stored PROCEDURES ----
-- Create
CREATE PROCEDURE SP_InsertProducts
	@ProductId INT,
    @Name NVARCHAR(100),
    @Price DECIMAL(10, 2),
    @CategoryId INT
AS
BEGIN
    INSERT INTO Products (Name, Price, CategoryId) VALUES (@Name, @Price, @CategoryId);
END
GO
--Drop procedure SP_InsertProduct
--Drop procedure SP_InsertProducts

-- Read
CREATE PROCEDURE SP_GetProducts
    @id INT
AS
BEGIN
    SELECT * FROM Products WHERE ProductId = @id;
END
GO
--drop PROCEDURE SP_GetProduct;

-- Update
CREATE PROCEDURE SP_UpdateProducts
    @ProductId INT,
    @Name NVARCHAR(100),
    @Price DECIMAL(10, 2),
    @CategoryId INT
AS
BEGIN
    UPDATE Products SET Name = @Name, Price = @Price, CategoryId = @CategoryId WHERE ProductId = @ProductId;
END
GO
--drop PROCEDURE SP_UpdateProduct

-- Delete
CREATE PROCEDURE SP_DeleteProducts
    @ProductId INT
AS
BEGIN
    DELETE FROM Products WHERE ProductId = @ProductId;
END
GO
--drop PROCEDURE SP_DeleteProduct

-----
select * from Products;

---
--Filter
CREATE PROCEDURE FilterProducts
    @CategoryId INT = NULL,
    @Limit INT = NULL,
    @Skip INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQL NVARCHAR(MAX);
    SET @SQL = 'SELECT * FROM Products';

    IF @CategoryId IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' WHERE CategoryId = @CategoryId';
    END

    IF @Limit IS NOT NULL AND @Skip IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' ORDER BY ProductId OFFSET @Skip ROWS FETCH NEXT @Limit ROWS ONLY';
    END

    EXEC sp_executesql @SQL, N'@CategoryId INT, @Limit INT, @Skip INT', @CategoryId, @Limit, @Skip;
END

---- Test Filters
EXEC FilterProducts @CategoryId = 2, @Limit = 100, @Skip = 20;