using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject;
using DataAccess.Repositories;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DataAccess;

namespace eStore.Pages.Admin
{
    public class ManageProductsModel : PageModel
    {
        private readonly ProductRepository _productRepository;

        public ManageProductsModel(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<Products> Products { get; set; }

        public void OnGet()
        {
            Products = _productRepository.GetAllProducts();
        }

        [BindProperty]
        public int ProductId { get; set; }

        public IActionResult OnPostDeleteProduct()
        {
            var product = _productRepository.FindProductById(ProductId);
            if (product != null)
            {
                _productRepository.DeleteProduct(product);
            }

            return RedirectToPage("/Admin/ManageProducts");
        }
        public IActionResult OnPost()
        {
            var product = _productRepository.FindProductById(ProductId);
            if (product != null)
            {
                _productRepository.DeleteProduct(product);
            }

            return RedirectToPage("/Admin/ManageProducts");
        }
    }
}
