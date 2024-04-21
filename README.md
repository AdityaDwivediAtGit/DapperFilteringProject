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
