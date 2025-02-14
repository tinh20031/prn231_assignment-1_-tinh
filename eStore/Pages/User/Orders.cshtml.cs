using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject;

namespace eStore.Pages.User
{
    public class OrdersModel : PageModel
    {
        private readonly eStoreDbContext _context;

        public OrdersModel(eStoreDbContext context)
        {
            _context = context;
        }

        public List<OrderViewModel> Orders { get; set; } = new List<OrderViewModel>();

        public async Task<IActionResult> OnGetAsync()
        {
            int? memberId = HttpContext.Session.GetInt32("MemberId");
            if (memberId == null || memberId == 0)
            {
                return RedirectToPage("/Account/Login");
            }

            Orders = await _context.orders
                .Where(o => o.MemberId == memberId)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new OrderViewModel
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    ShippedDate = o.ShippedDate,
                    TotalPrice = o.OrderDetails.Sum(d => d.UnitPrice * d.Quantity)
                })
                .ToListAsync();

            return Page();
        }
    }

    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
