using BusinessObject;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace eStore.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MemberController : ControllerBase
	{
		private readonly MemberRepository _memberRepository = new MemberRepository();

		
		[HttpGet]
		public IActionResult GetAllMembers()
		{
			var members = _memberRepository.GetAllMembers();
			return Ok(members);
		}

		
		[HttpGet("{id}")]
		public IActionResult GetMemberById(int id)
		{
			var member = _memberRepository.GetMemberById(id);
			if (member == null)
			{
				return NotFound($"Member with ID {id} not found.");
			}
			return Ok(member);
		}

		
		[HttpPost]
		public IActionResult CreateMember([FromBody] Member member)
		{
			if (member == null)
			{
				return BadRequest("Invalid member data.");
			}

			_memberRepository.AddMember(member);
			return CreatedAtAction(nameof(GetMemberById), new { id = member.MemberId }, member);
		}

		
		[HttpPut("{id}")]
		public IActionResult UpdateMember(int id, [FromBody] Member member)
		{
			if (member == null || member.MemberId != id)
			{
				return BadRequest("Invalid member data.");
			}

			var existingMember = _memberRepository.GetMemberById(id);
			if (existingMember == null)
			{
				return NotFound($"Member with ID {id} not found.");
			}

			_memberRepository.UpdateMember(member);
			return NoContent();
		}

		
		[HttpDelete("{id}")]
		public IActionResult DeleteMember(int id)
		{
			var existingMember = _memberRepository.GetMemberById(id);
			if (existingMember == null)
			{
				return NotFound($"Member with ID {id} not found.");
			}

			_memberRepository.RemoveMember(id);
			return NoContent();
		}
	}
    

}
