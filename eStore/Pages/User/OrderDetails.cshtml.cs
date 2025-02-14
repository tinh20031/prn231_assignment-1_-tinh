using BusinessObject;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace eStore.Pages.User;
public class OrderDetailsModel : PageModel
{
    private readonly eStoreDbContext _context;

    public Order Order { get; set; }

    public OrderDetailsModel(eStoreDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Order = await _context.orders
            .Include(o => o.OrderDetails)
            .ThenInclude(d => d.Products)
            .FirstOrDefaultAsync(o => o.OrderId == id);

        if (Order == null)
        {
            return NotFound();
        }

        return Page();
    }
}

