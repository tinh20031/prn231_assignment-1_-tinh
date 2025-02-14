using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using DataAccess.Repositories;
using BusinessObject;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace eStore.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly MemberRepository _memberRepository = new MemberRepository();
        private readonly IConfiguration _configuration;

        public LoginModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public IActionResult OnPost()
        {
            // ✅ Kiểm tra tài khoản Admin từ appsettings.json
            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];

            if (Email == adminEmail && Password == adminPassword)
            {
                HttpContext.Session.SetString("UserEmail", Email);
                HttpContext.Session.SetInt32("MemberId", 0); // ID admin mặc định
                HttpContext.Session.SetString("IsAdmin", "true");
                return Redirect("/Admin/AdminDashboard");
            }

            // ✅ Kiểm tra user trong database
            var member = _memberRepository.Authenticate(Email, Password);
            if (member != null)
            {
                HttpContext.Session.SetString("UserEmail", member.Email);
                HttpContext.Session.SetInt32("MemberId", member.MemberId);
                HttpContext.Session.SetString("IsAdmin", "false");

                return RedirectToPage("/Index");
            }

            ErrorMessage = "Email hoặc mật khẩu không đúng!";
            return Page();
        }
    }
}
