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
        return View(groceries);
    }

    [HttpPost]
    public IActionResult Filter(int page = 1, int pageSize = 10,
                               string category = "", string brand = "",
                               decimal minPrice = 0, decimal maxPrice = decimal.MaxValue)
    {
        Expression<Func<BigBasketGroceries, bool>> filter = g =>
            (string.IsNullOrEmpty(category) || g.Category == category) &&
            (string.IsNullOrEmpty(brand) || g.Brand == brand) &&
            (g.SalePrice >= minPrice && g.SalePrice <= maxPrice);

        var filteredGroceries = _groceryRepository.GetFiltered(filter)
                                                  .Skip((page - 1) * pageSize)
                                                  .Take(pageSize);

        return View("Filter", filteredGroceries);
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
        return View(grocery);
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
        return View(grocery);
    }

    public IActionResult Delete(int id)
    {
        var grocery = _groceryRepository.Get(id);
        if (grocery == null)
        {
            return NotFound();
        }

        return View(grocery);
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

