# Dapper Filtering Project
![image](https://github.com/AdityaDwivediAtGit/DapperFilteringProject/assets/162092463/8435b8f2-4f4d-4be9-921c-52ee1c1f72ce)


## Overview

This project is a demonstration of implementing `filtering` and `pagination` features in a shopping website using Dapper and ASP.NET Core MVC. It includes a generic repository class for database operations, CRUD functionality, and filtering products based on different criteria.

---

## Table of Contents

- [Features](#features)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [Data](#data)
- [Examples](#examples)
- [Issues](#issues)
- [Contributing](#contributing)

---

## Features

- **Filtering:** Allows users to filter products based on category, price range, brand, and other attributes.
- **Pagination:** Implements pagination for large datasets to improve performance and user experience.
- **Generic Repository:** Utilizes a generic repository class for database operations, providing a scalable and maintainable solution.
- **Stored Procedures:** Includes stored procedures for efficient data retrieval and manipulation.

---

## Getting Started

To get started with this project, follow these steps:

1. Clone the repository: `git clone https://github.com/AdityaDwivediAtGit/DapperFilteringProject`
2. Open the solution file `LearningFiltersShoppingDemo.sln` in Visual Studio.
3. Configure the database connection string in `appsettings.json`.
4. Run the project and visit for eg. https://localhost:7163/Product to see the Product filters in action.

---

## Usage

### Lambda Function in Controller
```csharp
public IActionResult FilterProducts(int categoryId, decimal minPrice, decimal maxPrice)
{
    Expression<Func<Products, bool>> filter = p => ((p.CategoryId == categoryId) && (p.Price >= minPrice && p.Price <= maxPrice));
    var filteredProducts = _productRepository.GetFilteredProducts(filter);
    return View(filteredProducts);
}
```

### Stored Procedure
#### Simple Filter
```sql
CREATE PROCEDURE [dbo].[FilterProducts]
    @CategoryId INT,
    @MinPrice DECIMAL,
    @MaxPrice DECIMAL
AS
BEGIN
    SELECT * FROM Products
    WHERE CategoryId = @CategoryId
    AND Price BETWEEN @MinPrice AND @MaxPrice
END
```
#### Better Filter
```sql
CREATE PROCEDURE FilterBigBasketGroceries
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
```

---

## Data

The project includes the following data files:

- `BigBasket Products.csv`
- `croma_products_final.csv`
- `LearningFiltersShoppingDemoDB script 20 April 2024, 8_24 PM.sql`

---

## Examples

- **Filtering Products by Category and Price Range:**
  ```csharp
  Expression<Func<Products, bool>> filter = p => ((p.CategoryId == 2) && (p.Price >= 2 && p.Price <= 200));
  ```

---

## Issues

If you encounter any issues or have suggestions for improvement, please [open an issue](https://github.com/AdityaDwivediAtGit/DapperFilteringProject/issues).

---

## Contributing

Contributions are welcome! Please fork the repository and create a pull request with your improvements.


---

**Author: Aditya Dwivedi**

*Feel free to provide feedback or suggestions! Contributions are welcome.*
