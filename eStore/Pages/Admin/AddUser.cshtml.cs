using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject;
using DataAccess.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace eStore.Pages.Admin
{
    public class AddUserModel : PageModel
    {
        private readonly MemberRepository _memberRepository = new MemberRepository();

        public IActionResult OnPost()
        {
            // Lấy dữ liệu từ form
            string email = Request.Form["email"];
            string password = Request.Form["password"];
            string companyName = Request.Form["companyName"];
            string city = Request.Form["city"];
            string country = Request.Form["country"];

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(companyName) || string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(country))
            {
                ModelState.AddModelError(string.Empty, "Vui lòng điền đầy đủ thông tin.");
                return Page();
            }

            // Tạo đối tượng Member mà KHÔNG mã hóa mật khẩu
            var newUser = new Member
            {
                Email = email,
                Password = password, // Lưu mật khẩu trực tiếp (Không hash)
                CompanyName = companyName,
                City = city,
                Country = country
            };

            // Lưu vào database
            _memberRepository.AddMember(newUser);

            // Chuyển hướng sau khi thêm thành công
            return RedirectToPage("/Admin/ManageUsers");
        }
       
    }
}
