using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject;
using DataAccess.Repositories;
using System.Collections.Generic;
using DataAccess;

namespace eStore.Pages.Admin
{
    public class AddProductModel : PageModel
    {
        private readonly ProductRepository _productRepository;
        private readonly CategoryRepository _categoryRepository;

        public AddProductModel(ProductRepository productRepository, CategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public List<Category> Categories { get; set; }

        public void OnGet()
        {
            Categories = _categoryRepository.GetAllCategory();
        }

        public IActionResult OnPost(string ProductName, decimal Weight, decimal UnitPrice, int UnitsInStock, int CategoryId)
        {
            if (string.IsNullOrEmpty(ProductName) || Weight <= 0 || UnitPrice <= 0 || UnitsInStock < 0 || CategoryId <= 0)
            {
                ModelState.AddModelError(string.Empty, "Vui lòng nhập thông tin hợp lệ.");
                Categories = _categoryRepository.GetAllCategory();
                return Page();
            }

            var newProduct = new Products
            {
                ProductName = ProductName,
                Weight = Weight,
                UnitPrice = UnitPrice,
                UnitsInStock = UnitsInStock,
                CategoryId = CategoryId
            };

            _productRepository.SaveProduct(newProduct);
            return RedirectToPage("/Admin/ManageProducts");
        }
    }
}
