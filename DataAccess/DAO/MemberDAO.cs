using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.DAO
{
	public class MemberDAO
	{
		// ✅ Lấy danh sách tất cả thành viên
		public static List<Member> GetMembers()
		{
			using (var context = new eStoreDbContext())
			{
				return context.members.ToList();
			}
		}
        private readonly eStoreDbContext _context;

        public MemberDAO(eStoreDbContext context)
        {
            _context = context;
        }

        // ✅ Tìm thành viên theo ID
        public static Member FindMemberById(int id)
		{
			using (var context = new eStoreDbContext())
			{
				return context.members.Find(id);
			}
		}

        // ✅ Thêm thành viên mới
        public static void SaveMember(Member member)
        {
            using (var context = new eStoreDbContext())
            {
                context.members.Add(member);
                context.SaveChanges();
            }
        }


        // ✅ Cập nhật thông tin thành viên
        public static void UpdateMember(Member member)
		{
			using (var context = new eStoreDbContext())
			{
				var existingMember = context.members.Find(member.MemberId);
				if (existingMember != null)
				{
					context.Entry(existingMember).CurrentValues.SetValues(member);
					context.SaveChanges();
				}
				else
				{
					throw new Exception($"Member with ID {member.MemberId} not found.");
				}
			}
		}

		// ✅ Xóa thành viên
		public static void DeleteMember(int id)
		{
			using (var context = new eStoreDbContext())
			{
				var member = context.members.Find(id);
				if (member != null)
				{
					context.members.Remove(member);
					context.SaveChanges();
				}
				else
				{
					throw new Exception($"Member with ID {id} not found.");
				}
			}
		}




        public static Member Authenticate(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Email and Password cannot be empty.");
            }

            using (var context = new eStoreDbContext())
            {
                var member = context.members
     .ToList() // Chuyển dữ liệu về bộ nhớ
     .FirstOrDefault(m => m.Email.Trim().ToLowerInvariant() == email.Trim().ToLowerInvariant()
                       && m.Password == password);


                return member ?? throw new UnauthorizedAccessException("Invalid email or password.");
            }
        }

        public async Task<Member> GetMemberByEmailAsync(string email)
        {
            return await _context.members.FirstOrDefaultAsync(m => m.Email == email);
        }

        public async Task UpdateMemberAsync(Member member)
        {
            // Nếu member không được attach vào DbContext, bạn có thể thực hiện:
            _context.Attach(member);
            _context.Entry(member).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

    }




}

