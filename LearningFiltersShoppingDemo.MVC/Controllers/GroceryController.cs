using LearningFiltersShoppingDemo.MVC.Models;
using LearningFiltersShoppingDemo.MVC.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace LearningFiltersShoppingDemo.MVC.Controllers;

public class GroceryController : Controller
{
    private readonly GenericRepo<BigBasketGroceries> _groceryRepository;

    public GroceryController(GenericRepo<BigBasketGroceries> groceryRepository)
    {
        _groceryRepository = groceryRepository;
    }

    public IActionResult Index(int page = 1, int pageSize = 10)
    {
        var groceries = _groceryRepository.GetPaged(page, pageSize);
        return View("ShowGroceryView", groceries);
    }

    [HttpPost]
    public IActionResult Filter(int page = 1, int pageSize = 10,
                               string category = "", string brand = "",
                               decimal minPrice = 0, decimal maxPrice = decimal.MaxValue)
    {
        #region try 1 fail: is null or empty
        //Expression<Func<BigBasketGroceries, bool>> filter = g =>
        //    (string.IsNullOrEmpty(category) || g.Category == category) &&
        //    (string.IsNullOrEmpty(brand) || g.Brand == brand) &&
        //    (g.SalePrice >= minPrice && g.SalePrice <= maxPrice);
        #endregion

        #region try 2 fail:  And
        //Expression<Func<BigBasketGroceries, bool>> filter = g => true;

        //if (!string.IsNullOrEmpty(category))
        //{
        //    filter = filter.And(g => g.Category == category);
        //}

        //if (!string.IsNullOrEmpty(brand))
        //{
        //    filter = filter.And(g => g.Brand == brand);
        //}

        //filter = filter.And(g => g.SalePrice >= minPrice && g.SalePrice <= maxPrice);
        #endregion

        #region try 3 fail: AndAlso
        //Expression<Func<BigBasketGroceries, bool>> filter = g =>
        //(string.IsNullOrEmpty(category) || g.Category == category);

        //if (minPrice != null)
        //{
        //    Expression<Func<BigBasketGroceries, bool>> minPriceFilter = g => g.SalePrice >= minPrice;
        //    filter = filter.AndAlso(minPriceFilter);
        //}

        //if (maxPrice != null)
        //{
        //    Expression<Func<BigBasketGroceries, bool>> maxPriceFilter = g => g.SalePrice <= maxPrice;
        //    filter = filter.AndAlso(maxPriceFilter);
        //}

        //if (minRating != null)
        //{
        //    Expression<Func<BigBasketGroceries, bool>> minRatingFilter = g => g.Rating >= minRating;
        //    filter = filter.AndAlso(minRatingFilter);
        //}

        //if (maxRating != null)
        //{
        //    Expression<Func<BigBasketGroceries, bool>> maxRatingFilter = g => g.Rating <= maxRating;
        //    filter = filter.AndAlso(maxRatingFilter);
        //}
        #endregion

        #region try 4 fail: hasValue

        //Expression<Func<BigBasketGroceries, bool>> filter = g =>
        //    (string.IsNullOrEmpty(category) || g.Category == category) &&
        //    (!minPrice.HasValue || g.SalePrice >= minPrice.Value) &&
        //    (!maxPrice.HasValue || g.SalePrice <= maxPrice.Value) &&
        //    (!minRating.HasValue || g.Rating >= minRating.Value) &&
        //    (!maxRating.HasValue || g.Rating <= maxRating.Value);
        #endregion

        #region try 5 fail: Ternary Operator
        //Expression<Func<BigBasketGroceries, bool>> filter = g =>
        //    (string.IsNullOrEmpty(category) ? true : g.Category == category) &&  // Check category only if it has a value
        //    (!string.IsNullOrEmpty(brand) ? g.Brand == brand : true) &&  // Check brand only if it has a value
        //    (g.SalePrice >= minPrice && g.SalePrice <= maxPrice);

        #endregion

        #region try 6 fail: AND
        //Expression<Func<BigBasketGroceries, bool>> filter = null;

        //// Construct the filter based on provided parameters
        //if (!string.IsNullOrEmpty(category))
        //{
        //    filter = g => g.Category == category;
        //}

        //if (!string.IsNullOrEmpty(brand))
        //{
        //    if (filter == null) // If category filter wasn't applied yet
        //    {
        //        filter = g => g.Brand == brand;
        //    }
        //    else // Combine category and brand filters with AND
        //    {
        //        Expression<Func<BigBasketGroceries, bool>> brandFilter = g => g.Brand == brand;
        //        filter = Expression.And(filter, brandFilter); // Pass both filters to And
        //        //filter = filter && brandFilter;
        //    }
        //}

        //if (minPrice != 0 || maxPrice != decimal.MaxValue) // Check for non-default price range
        //{
        //    if (filter == null) // If no previous filters applied
        //    {
        //        filter = g => g.SalePrice >= minPrice && g.SalePrice <= maxPrice;
        //    }
        //    else // Combine existing filter with price range using AND
        //    {
        //        filter = filter.And(g => g.SalePrice >= minPrice && g.SalePrice <= maxPrice);
        //    }
        //}
        #endregion

        #region try 7: Just arrange it in if else and else if statements because and declare separate expressions for each of  different cases
        //Expression<Func<BigBasketGroceries, bool>> filter = null;

        //// Category Filter
        //if (!string.IsNullOrEmpty(category))
        //{
        //    filter = g => g.Category == category; // Create filter for category
        //}

        //// Brand Filter (Combine with Category filter if it exists)
        //if (!string.IsNullOrEmpty(brand))
        //{
        //    if (filter != null)
        //    {
        //        filter = g => filter(g) && g.Brand == brand; // Combine filters with AND
        //    }
        //    else
        //    {
        //        filter = g => g.Brand == brand; // Create filter for brand
        //    }
        //}

        //// Price Range Filter (Combine with existing filter if it exists)
        //if (minPrice != 0 || maxPrice != decimal.MaxValue)
        //{
        //    if (filter != null)
        //    {
        //        filter = g => filter(g) && g.SalePrice >= minPrice && g.SalePrice <= maxPrice; // Combine filters with AND
        //    }
        //    else
        //    {
        //        filter = g => g.SalePrice >= minPrice && g.SalePrice <= maxPrice; // Create filter for price range
        //    }
        //}

        #endregion

        #region Manual way
        Expression<Func<BigBasketGroceries, bool>> filter = null;

        // Category Filter
        if (!string.IsNullOrEmpty(category))
        {
            if (!string.IsNullOrEmpty(brand))
            {
                if (minPrice != 0 || maxPrice != decimal.MaxValue)
                {
                    filter = g => (g.Category == category) && (g.Brand == brand) && (g.Sale_Price >= minPrice && g.Sale_Price <= maxPrice); // All three filters applied
                }
                else
                {
                    filter = g => (g.Category == category) && (g.Brand == brand); // Category and Brand filter
                }
            }
            else
            {
                if (minPrice != 0 || maxPrice != decimal.MaxValue)
                {
                    filter = g => (g.Category == category) &&(g.Sale_Price >= minPrice && g.Sale_Price <= maxPrice); // All three filters applied
                }
                else
                {
                    filter = g => (g.Category == category); // Category and Brand filter
                }
            }
        }
        else
        {
            // Handle case where category is empty
            if (!string.IsNullOrEmpty(brand))
            {
                if (minPrice != 0 || maxPrice != decimal.MaxValue)
                {
                    filter = g => (g.Brand == brand) && (g.Sale_Price >= minPrice && g.Sale_Price <= maxPrice); // All three filters applied
                }
                else
                {
                    filter = g => (g.Brand == brand); // Category and Brand filter
                }
            }
            else
            {
                if (minPrice != 0 || maxPrice != decimal.MaxValue)
                {
                    filter = g => (g.Sale_Price >= minPrice && g.Sale_Price <= maxPrice); // All three filters applied
                }
                else
                {
                    filter = null; // Category and Brand filter
                }
            }
        }
        #endregion

        var filteredGroceries = _groceryRepository.GetFiltered(filter)
                                                  .Skip((page - 1) * pageSize)
                                                  .Take(pageSize);

        return View("ShowGroceryView", filteredGroceries);
    }

    public IActionResult Create()
    {
        // Consider adding logic to populate dropdown lists for categories, brands, etc.
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(BigBasketGroceries grocery)
    {
        if (ModelState.IsValid)
        {
            _groceryRepository.Insert(grocery);
            return RedirectToAction(nameof(Index));
        }

        // Consider repopulating dropdown lists here if needed
        return View(grocery);
    }

    public IActionResult Edit(int id)
    {
        var grocery = _groceryRepository.Get(id);
        if (grocery == null)
        {
            return NotFound();
        }

        // Consider pre-populating dropdown lists for categories, brands, etc. based on existing data
        return View("EditGroceryView", grocery);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, BigBasketGroceries grocery)
    {
        if (id != grocery.Index) // Assuming Index is the unique identifier
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            _groceryRepository.Update(grocery);
            return RedirectToAction(nameof(Index));
        }

        // Consider repopulating dropdown lists here if needed
        return View("ShowGroceryView", grocery);
    }

    public IActionResult Delete(int id)
    {
        var grocery = _groceryRepository.Get(id);
        if (grocery == null)
        {
            return NotFound();
        }

        return View("ShowGroceryView", grocery);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var grocery = _groceryRepository.Get(id);
        if (grocery == null)
        {
            return NotFound();
        }

        _groceryRepository.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}

