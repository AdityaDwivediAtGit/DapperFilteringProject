USE [master]
GO
/****** Object:  Database [LearningFiltersShoppingDemoDB]    Script Date: 20-04-2024 08:26:36 PM ******/
CREATE DATABASE [LearningFiltersShoppingDemoDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LearningFiltersShoppingDemoDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\LearningFiltersShoppingDemoDB.mdf' , SIZE = 139264KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'LearningFiltersShoppingDemoDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\LearningFiltersShoppingDemoDB_log.ldf' , SIZE = 204800KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LearningFiltersShoppingDemoDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET RECOVERY FULL 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET  MULTI_USER 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'LearningFiltersShoppingDemoDB', N'ON'
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [LearningFiltersShoppingDemoDB]
GO
/****** Object:  Table [dbo].[BigBasketGroceries]    Script Date: 20-04-2024 08:26:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BigBasketGroceries](
	[index] [int] IDENTITY(1,1) NOT NULL,
	[product] [nvarchar](max) NULL,
	[category] [nvarchar](max) NULL,
	[sub_category] [nvarchar](max) NULL,
	[brand] [nvarchar](max) NULL,
	[sale_price] [decimal](10, 2) NULL,
	[market_price] [decimal](10, 2) NULL,
	[type] [nvarchar](max) NULL,
	[rating] [decimal](3, 1) NULL,
	[description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[index] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 20-04-2024 08:26:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChromaElectronics]    Script Date: 20-04-2024 08:26:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChromaElectronics](
	[index] [int] IDENTITY(1,1) NOT NULL,
	[link] [nvarchar](max) NULL,
	[price] [nvarchar](max) NULL,
	[name] [nvarchar](max) NULL,
	[category] [nvarchar](max) NULL,
	[features] [nvarchar](max) NULL,
	[overview] [nvarchar](max) NULL,
	[images] [nvarchar](max) NULL,
	[specification] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[index] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 20-04-2024 08:26:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Price] [decimal](10, 2) NOT NULL,
	[CategoryId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([CategoryId])
GO
/****** Object:  StoredProcedure [dbo].[FilterBigBasketGroceries]    Script Date: 20-04-2024 08:26:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FilterBigBasketGroceries]
    @Index INT = NULL,
    @Product NVARCHAR(MAX) = NULL,
    @Category NVARCHAR(MAX) = NULL,
    @SubCategory NVARCHAR(MAX) = NULL,
    @Brand NVARCHAR(MAX) = NULL,
    @SalePrice DECIMAL(10, 2) = NULL,
    @MarketPrice DECIMAL(10, 2) = NULL,
    @Type NVARCHAR(MAX) = NULL,
    @MinRating DECIMAL(3, 1) = NULL,
    @MaxRating DECIMAL(3, 1) = NULL,
    @Description NVARCHAR(MAX) = NULL,
    @Limit INT = NULL,
    @Skip INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQL NVARCHAR(MAX);
    SET @SQL = 'SELECT * FROM BigBasketGroceries';

    IF @Index IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' WHERE [index] = @Index';
    END
    ELSE
    BEGIN
        SET @SQL = @SQL + ' WHERE 1=1';
    END

    IF @Product IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' AND product = @Product';
    END

    IF @Category IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' AND category = @Category';
    END

    IF @SubCategory IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' AND sub_category = @SubCategory';
    END

    IF @Brand IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' AND brand = @Brand';
    END

    IF @SalePrice IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' AND sale_price = @SalePrice';
    END

    IF @MarketPrice IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' AND market_price = @MarketPrice';
    END

    IF @Type IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' AND [type] = @Type';
    END

    IF @MinRating IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' AND rating >= @MinRating';
    END

    IF @MaxRating IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' AND rating <= @MaxRating';
    END

    IF @Description IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' AND description = @Description';
    END

    IF @Limit IS NOT NULL AND @Skip IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' ORDER BY [index] OFFSET @Skip ROWS FETCH NEXT @Limit ROWS ONLY';
    END

    EXEC sp_executesql @SQL, N'@Index INT, @Product NVARCHAR(MAX), @Category NVARCHAR(MAX), @SubCategory NVARCHAR(MAX), @Brand NVARCHAR(MAX), @SalePrice DECIMAL(10, 2), @MarketPrice DECIMAL(10, 2), @Type NVARCHAR(MAX), @MinRating DECIMAL(3, 1), @MaxRating DECIMAL(3, 1), @Description NVARCHAR(MAX), @Limit INT, @Skip INT', @Index, @Product, @Category, @SubCategory, @Brand, @SalePrice, @MarketPrice, @Type, @MinRating, @MaxRating, @Description, @Limit, @Skip;
END
GO
/****** Object:  StoredProcedure [dbo].[FilterChromaElectronics]    Script Date: 20-04-2024 08:26:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FilterChromaElectronics]
    @Index INT = NULL,
    @Limit INT = NULL,
    @Skip INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQL NVARCHAR(MAX);
    SET @SQL = 'SELECT * FROM ChromaElectronics';

    IF @Index IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' WHERE [index] = @Index';
    END

    IF @Limit IS NOT NULL AND @Skip IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' ORDER BY [index] OFFSET @Skip ROWS FETCH NEXT @Limit ROWS ONLY';
    END

    EXEC sp_executesql @SQL, N'@Index INT, @Limit INT, @Skip INT', @Index, @Limit, @Skip;
END
GO
/****** Object:  StoredProcedure [dbo].[FilterProducts]    Script Date: 20-04-2024 08:26:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FilterProducts]
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
GO
/****** Object:  StoredProcedure [dbo].[SP_DeleteProduct]    Script Date: 20-04-2024 08:26:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Delete
CREATE PROCEDURE [dbo].[SP_DeleteProduct]
    @ProductId INT
AS
BEGIN
    DELETE FROM Products WHERE ProductId = @ProductId;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetProduct]    Script Date: 20-04-2024 08:26:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Read
CREATE PROCEDURE [dbo].[SP_GetProduct]
    @ProductId INT
AS
BEGIN
    SELECT * FROM Products WHERE ProductId = @ProductId;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertProduct]    Script Date: 20-04-2024 08:26:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Create
CREATE PROCEDURE [dbo].[SP_InsertProduct]
    @Name NVARCHAR(100),
    @Price DECIMAL(10, 2),
    @CategoryId INT
AS
BEGIN
    INSERT INTO Products (Name, Price, CategoryId) VALUES (@Name, @Price, @CategoryId);
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateProduct]    Script Date: 20-04-2024 08:26:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Update
CREATE PROCEDURE [dbo].[SP_UpdateProduct]
    @ProductId INT,
    @Name NVARCHAR(100),
    @Price DECIMAL(10, 2),
    @CategoryId INT
AS
BEGIN
    UPDATE Products SET Name = @Name, Price = @Price, CategoryId = @CategoryId WHERE ProductId = @ProductId;
END
GO
USE [master]
GO
ALTER DATABASE [LearningFiltersShoppingDemoDB] SET  READ_WRITE 
GO
