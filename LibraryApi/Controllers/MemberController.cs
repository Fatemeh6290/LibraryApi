using LibraryApi.DTOs;
using LibraryApi.Interfaces;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MemberController : ControllerBase
{
    private readonly IMemberService _memberService;
    
    public MemberController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    [HttpGet]
    public ActionResult<List<MemberDto>> GetMembers()
    {
        var members = _memberService.GetMembers();

        var result = members.Select(member => new MemberDto
        {
            MemberId = member.MemberId,
            Name = member.Name,
            Email = member.Email
        }).ToList();
        
        return result;
    }

    [HttpGet("{id}")]
    public ActionResult<MemberDto> GetMemberById(int id)
    {
        var member = _memberService.GetMemberById(id);
        
        if (member == null)
            return NotFound();

        var result = new MemberDto
        {
            MemberId = member.MemberId,
            Name = member.Name,
            Email = member.Email
        };
        
        return result;
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteMember(int id)
    {
        var result = _memberService.DeleteMember(id);
        
        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpPost]
    public ActionResult<Member> AddMember(CreateMemberDto dto)
    {
        var member = _memberService.AddMember(
             dto.Name,
             dto.Email);
        
        if (member is null)
            return BadRequest("Member already exists.");

        var result = new MemberDto
        {
            MemberId = member.MemberId,
            Name = member.Name,
            Email = member.Email
        };
        
        return CreatedAtAction(nameof(GetMemberById), 
            new { id = member.MemberId }, 
            result);
    }
}