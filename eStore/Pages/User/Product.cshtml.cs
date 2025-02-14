using BusinessObject;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace eStore.Pages.User
{
    public class ProductModel : PageModel
    {
        private readonly ProductRepository productRepository = new ProductRepository();
        public List<Products> Products { get; set; } = new List<Products>();
        [BindProperty(SupportsGet = true)]
        public string SearchKeyword { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public decimal? MaxPrice { get; set; }

        public void OnGet()
        {
            Products = productRepository.GetAllProducts();
            if (!string.IsNullOrEmpty(SearchKeyword))
            {
                Products = Products.Where(p => p.ProductName.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Lọc theo giá tối đa
            if (MaxPrice.HasValue)
            {
                Products = Products.Where(p => p.UnitPrice <= MaxPrice.Value).ToList();
            }
        }
        //public void OnPost()
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Products = productRepository.GetAllProducts();

        //        //ok 
        //    }
        //    else
        //    {
        //        //loi
        //    }
        //}

        public IActionResult OnPost(int productId)
        {
            var product = productRepository.GetAllProducts().FirstOrDefault(p => p.ProductId == productId);
            if (product == null) return NotFound();

            // Ví dụ sử dụng Cookie để lưu giỏ hàng (không dùng Session)
            List<CartItem> cart = new List<CartItem>();
            string cartCookie = Request.Cookies["Cart"];
            if (!string.IsNullOrEmpty(cartCookie))
            {
                cart = JsonConvert.DeserializeObject<List<CartItem>>(cartCookie);
            }

            var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity++; // Tăng số lượng
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    UnitPrice = product.UnitPrice,
                    Quantity = 1
                });
            }

            string updatedCart = JsonConvert.SerializeObject(cart);
            CookieOptions options = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddHours(2),
                HttpOnly = true
            };
            Response.Cookies.Append("Cart", updatedCart, options);

            return RedirectToPage("/User/Cart");
        }
    }


}
