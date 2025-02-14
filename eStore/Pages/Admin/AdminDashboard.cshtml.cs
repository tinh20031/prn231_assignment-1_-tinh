using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using DataAccess.DAO;

namespace eStore.Pages.Admin
{
    public class AdminDashboardModel : PageModel
    {
        public int TotalUsers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }

        public void OnGet()
        {
            TotalUsers = MemberDAO.GetMembers().Count;
            TotalProducts = ProductDAO.GetProducts().Count;
            TotalOrders = OrderDAO.GetOrders().Count;

            // ✅ Tính tổng doanh thu
            TotalRevenue = OrderDAO.GetOrders().Sum(o => o.OrderDetails.Sum(d => d.Quantity * d.UnitPrice));

        }
    }
}
