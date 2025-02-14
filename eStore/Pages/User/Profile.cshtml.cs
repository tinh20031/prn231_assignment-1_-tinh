using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject;
using DataAccess.DAO;
using System.Threading.Tasks;

namespace eStore.Pages.User
{
    public class ProfileModel : PageModel
    {
        private readonly MemberDAO _memberDAO;

        public ProfileModel(MemberDAO memberDAO)
        {
            _memberDAO = memberDAO;
        }

        [BindProperty]
        public ProfileViewModel Profile { get; set; }

        public string Email { get; set; } // Sử dụng để hiển thị email (không bind)

        public async Task<IActionResult> OnGetAsync()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToPage("/Account/Login");
            }

            var member = await _memberDAO.GetMemberByEmailAsync(email);
            if (member == null)
            {
                return NotFound();
            }

            // Gán dữ liệu từ Member sang ProfileViewModel
            Profile = new ProfileViewModel
            {
                CompanyName = member.CompanyName,
                City = member.City,
                Country = member.Country
            };

            Email = member.Email; // Để hiển thị trong view (read-only)

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Nếu ModelState không hợp lệ, hiển thị lại trang
                return Page();
            }

            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToPage("/Account/Login");
            }

            var member = await _memberDAO.GetMemberByEmailAsync(email);
            if (member == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin từ ProfileViewModel vào đối tượng Member
            member.CompanyName = Profile.CompanyName;
            member.City = Profile.City;
            member.Country = Profile.Country;

            await _memberDAO.UpdateMemberAsync(member);

            TempData["SuccessMessage"] = "Cập nhật thành công!";
            return RedirectToPage("/User/Profile");
        }
    }
}
