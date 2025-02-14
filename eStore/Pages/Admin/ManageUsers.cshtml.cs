using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject;
using DataAccess.Repositories;
using System.Collections.Generic;

namespace eStore.Pages.Admin
{
    public class ManageUsersModel : PageModel
    {
        private readonly MemberRepository _memberRepository = new MemberRepository();

        public List<Member> Users { get; set; }
        [BindProperty]
        public Member NewUser { get; set; }

        [BindProperty]
        public Member EditUser { get; set; }

  public IActionResult OnGet()
        {
            Users = _memberRepository.GetAllMembers();
            if (Users == null)
            {
                Users = new List<Member>(); // Tránh lỗi NullReferenceException
            }
            return Page();
        }


        public IActionResult OnPost(int memberId)
        {
            var member = _memberRepository.GetMemberById(memberId);
            if (member != null)
            {
                _memberRepository.RemoveMember(memberId);
            }
            return RedirectToPage("/Admin/ManageUsers");
        }
        public IActionResult OnPostDeleteUser(int memberId)
        {
            var member = _memberRepository.GetMemberById(memberId);
            if (member != null)
            {
                _memberRepository.RemoveMember(memberId);
            }
            return RedirectToPage("/Admin/ManageUsers");
        }

        public IActionResult OnPostAddUser()
        {
            if (NewUser != null)
            {
                _memberRepository.AddMember(NewUser);
            }
            return RedirectToPage();
        }
        public IActionResult OnPostEditUser()
        {
            var user = _memberRepository.GetMemberById(EditUser.MemberId);
            if (user != null)
            {
                _memberRepository.UpdateMember(EditUser);
            }
            return RedirectToPage();
        }

    }
}
