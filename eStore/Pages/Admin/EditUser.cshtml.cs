using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject;
using DataAccess.Repositories;

namespace eStore.Pages.Admin
{
    public class EditUserModel : PageModel
    {
        private readonly MemberRepository _memberRepository = new MemberRepository();

        public int MemberId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        public IActionResult OnGet(int id)
        {
            var member = _memberRepository.GetMemberById(id);
            if (member == null)
            {
                return NotFound();
            }

            // Gán dữ liệu để hiển thị trong form
            MemberId = member.MemberId;
            Email = member.Email;
            CompanyName = member.CompanyName;
            City = member.City;
            Country = member.Country;

            // Lưu mật khẩu hiện tại vào TempData để sử dụng sau khi submit
            TempData["Password"] = member.Password;

            return Page();
        }

        public IActionResult OnPost()
        {
            int memberId = int.Parse(Request.Form["memberId"]);
            string email = Request.Form["email"];
            string companyName = Request.Form["companyName"];
            string city = Request.Form["city"];
            string country = Request.Form["country"];
            string password = Request.Form["password"]; // Lấy mật khẩu mới từ form

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(companyName) ||
                string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(country))
            {
                ModelState.AddModelError(string.Empty, "Vui lòng điền đầy đủ thông tin.");
                return Page();
            }

            // Nếu mật khẩu mới không được nhập, giữ nguyên mật khẩu cũ
            if (string.IsNullOrWhiteSpace(password))
            {
                password = TempData["Password"] as string ?? throw new Exception("Không tìm thấy mật khẩu");
            }

            // Cập nhật thông tin người dùng
            var updatedUser = new Member
            {
                MemberId = memberId,
                Email = email,
                CompanyName = companyName,
                City = city,
                Country = country,
                Password = password  // Giữ nguyên hoặc cập nhật mật khẩu
            };

            _memberRepository.UpdateMember(updatedUser);

            return RedirectToPage("/Admin/ManageUsers");
        }
    }
}
