using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject;
using DataAccess.Repositories;
using System.Collections.Generic;
using DataAccess;

namespace eStore.Pages.Admin
{
    public class EditProductModel : PageModel
    {
        private readonly ProductRepository _productRepository;
        private readonly CategoryRepository _categoryRepository;

        public EditProductModel(ProductRepository productRepository, CategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public List<Category> Categories { get; set; }
        public Products Product { get; set; }

        public void OnGet(int id)
        {
            Product = _productRepository.FindProductById(id);
            Categories = _categoryRepository.GetAllCategory();
        }

        public IActionResult OnPost(int ProductId, string ProductName, decimal UnitPrice, int UnitsInStock, int CategoryId)
        {
            if (string.IsNullOrEmpty(ProductName) || UnitPrice <= 0 || UnitsInStock < 0 || CategoryId <= 0)
            {
                ModelState.AddModelError(string.Empty, "Vui lòng nhập thông tin hợp lệ.");
                Categories = _categoryRepository.GetAllCategory();
                return Page();
            }

            var updatedProduct = new Products
            {
                ProductId = ProductId,
                ProductName = ProductName,
                UnitPrice = UnitPrice,
                UnitsInStock = UnitsInStock,
                CategoryId = CategoryId
            };

            _productRepository.UpdateProduct(updatedProduct);
            return RedirectToPage("/Admin/ManageProducts");
        }
    }
}
