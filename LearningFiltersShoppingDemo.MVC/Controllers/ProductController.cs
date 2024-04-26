using LearningFiltersShoppingDemo.MVC.Models;
using LearningFiltersShoppingDemo.MVC.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq.Expressions;

namespace LearningFiltersShoppingDemo.MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IDbConnection _connection;
        private readonly GenericRepo<Products> _productRepository;
        private readonly GenericRepo<Categories> _categoryRepository;

        public ProductController(IDbConnection connection, GenericRepo<Products> productRepository, GenericRepo<Categories> categoryRepository)
        {
            _connection = connection;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var products = _productRepository.GetPaged(page, pageSize);
            return View(products);
        }

        [HttpPost]
        //public IActionResult Filter(int page = 1, int pageSize = 10, int categoryId = -1, decimal minPrice = 0, decimal maxPrice = decimal.MaxValue)
        public IActionResult Filter(int page = 1, int pageSize = 10, string categoryId = "", decimal minPrice = 0, decimal maxPrice = decimal.MaxValue)
        {
            Expression<Func<Products, bool>> filter = p =>
                (string.IsNullOrEmpty(categoryId) || p.CategoryId == int.Parse(categoryId)) &&
                (p.Price >= minPrice && p.Price <= maxPrice);

            #region Working Expression (without method)
            //Expression<Func<Products, bool>> filter = null;

            //if (categoryId != -1)
            //{
            //    filter = p => (p.CategoryId == categoryId) && (p.Price >= minPrice && p.Price <= maxPrice);
            //}
            //else
            //{
            //    filter = p => (p.Price >= minPrice && p.Price <= maxPrice);
            //}
            #endregion

            //var categoryIdConstant = Expression.Constant(categoryId);
            //Expression<Func<Products, bool>> filter = p => ((p.CategoryId == (int)categoryIdConstant.Value) && (p.Price >= 2 && p.Price <= 200));
            //Expression<Func<Products, bool>> filter = p => ((p.CategoryId == 2) && (p.Price >= minPrice && p.Price <= 200));

            var filteredProducts = _productRepository.GetFiltered(filter)
                                                     .Skip((page - 1) * pageSize)
                                                     .Take(pageSize);

            //string connectionString = "Data Source=DESKTOP-S0ERLT1;Initial Catalog=LearningFiltersShoppingDemoDB;Integrated Security=True;Trusted_Connection=True;";
            //var repository = new EntityRepository<Products>(connectionString);

            //Expression<Func<Products, int>> orderBy = p => p.ProductId;

            //List<Products> filteredProducts = repository.GetEntities(filter, orderBy, descending: false);
            //                                            //.Skip((page - 1) * pageSize)
            //                                            //.Take(pageSize);

            return View("Filter", filteredProducts);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _categoryRepository.GetAll().ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Products product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.Insert(product.Name, product.Price, product.CategoryId);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _categoryRepository.GetAll().ToList();
            return View(product);
        }

        public IActionResult Edit(int id)
        {
            var product = _productRepository.Get(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = _categoryRepository.GetAll().ToList();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Products product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _productRepository.Update(product.ProductId, product.Name, product.Price, product.CategoryId);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _categoryRepository.GetAll().ToList();
            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = _productRepository.Get(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _productRepository.Get(id);
            if (product == null)
            {
                return NotFound();
            }

            _productRepository.Delete(product.ProductId);
            return RedirectToAction(nameof(Index));
        }
    }
}
