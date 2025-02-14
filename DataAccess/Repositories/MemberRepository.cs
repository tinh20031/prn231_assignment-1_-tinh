using BusinessObject;
using DataAccess.DAO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DataAccess.Repositories
{
	public class MemberRepository
	{

		public List<Member> GetAllMembers() => MemberDAO.GetMembers();


		public Member GetMemberById(int id) => MemberDAO.FindMemberById(id);


		public void AddMember(Member member) => MemberDAO.SaveMember(member);


		public void UpdateMember(Member member) => MemberDAO.UpdateMember(member);


		public void RemoveMember(int id) => MemberDAO.DeleteMember(id);

		public Member Authenticate(string email, string password)
		{
			return MemberDAO.Authenticate(email, password);
		}
       


    }
}
