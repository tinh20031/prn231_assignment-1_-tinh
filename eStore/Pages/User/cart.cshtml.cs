using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using BusinessObject;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace eStore.Pages.User
{
    public class CartModel : PageModel
    {
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public decimal TotalPrice { get; set; }

        private readonly eStoreDbContext _context;

        public CartModel(eStoreDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void OnGet()
        {
            LoadCart();
        }

        public IActionResult OnPost()
        {
            if (Request.Form.ContainsKey("updateCart"))
            {
                return OnPostUpdate();
            }
            else if (Request.Form.ContainsKey("checkout"))
            {
                return OnPostCheckout();
            }
            else if (Request.Form.ContainsKey("removeProductId"))
            {
                int productId = int.Parse(Request.Form["removeProductId"]);
                return OnPostRemove(productId);
            }

            return Page();
        }

        private IActionResult OnPostUpdate()
        {
            string cartCookie = Request.Cookies["Cart"];
            if (!string.IsNullOrEmpty(cartCookie))
            {
                var cart = JsonConvert.DeserializeObject<List<CartItem>>(cartCookie);
                var productIds = Request.Form["productIds"].Select(int.Parse).ToList();
                var quantities = Request.Form["quantities"].Select(int.Parse).ToList();

                for (int i = 0; i < productIds.Count; i++)
                {
                    var item = cart.FirstOrDefault(c => c.ProductId == productIds[i]);
                    if (item != null)
                    {
                        item.Quantity = quantities[i];
                    }
                }

                SaveCart(cart);
            }

            return RedirectToPage();
        }

        private IActionResult OnPostCheckout()
        {
            int? memberId = HttpContext.Session.GetInt32("MemberId");
            if (memberId == null || memberId == 0)
            {
                return RedirectToPage("/Account/Login");
            }

            string cartCookie = Request.Cookies["Cart"];
            if (string.IsNullOrEmpty(cartCookie))
            {
                return RedirectToPage();
            }

            var cart = JsonConvert.DeserializeObject<List<CartItem>>(cartCookie);
            if (cart.Count == 0)
            {
                return RedirectToPage();
            }

            var order = new Order
            {
                OrderDate = DateTime.Now,
                RequiredDate = DateTime.Now.AddDays(3),
                ShippedDate = DateTime.Now.AddDays(5),
                Freight = 10,
                MemberId = memberId.Value,
                OrderDetails = cart.Select(item => new OrderDetail
                {
                    ProductId = item.ProductId,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    Discount = 0
                }).ToList()
            };

            _context.orders.Add(order);
            _context.SaveChanges();

            Response.Cookies.Delete("Cart");
            return RedirectToPage("/User/Orders");
        }

        private IActionResult OnPostRemove(int productId)
        {
            string cartCookie = Request.Cookies["Cart"];
            if (!string.IsNullOrEmpty(cartCookie))
            {
                var cart = JsonConvert.DeserializeObject<List<CartItem>>(cartCookie);
                var itemToRemove = cart.FirstOrDefault(c => c.ProductId == productId);

                if (itemToRemove != null)
                {
                    cart.Remove(itemToRemove);
                    SaveCart(cart);
                }
            }

            return RedirectToPage();
        }

        private void LoadCart()
        {
            string cartCookie = Request.Cookies["Cart"];
            if (!string.IsNullOrEmpty(cartCookie))
            {
                CartItems = JsonConvert.DeserializeObject<List<CartItem>>(cartCookie);
            }
            TotalPrice = CartItems.Sum(c => c.UnitPrice * c.Quantity);
        }

        private void SaveCart(List<CartItem> cart)
        {
            string updatedCart = JsonConvert.SerializeObject(cart);
            CookieOptions options = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddHours(2),
                HttpOnly = true
            };
            Response.Cookies.Append("Cart", updatedCart, options);
        }
    }
}
