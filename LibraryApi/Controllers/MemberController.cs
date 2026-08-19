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
    public ActionResult<List<Member>> GetMembers()
    {
        return _memberService.GetMembers();
    }

    [HttpGet("{id}")]
    public ActionResult<Member> GetMemberById(int id)
    {
        var member = _memberService.GetMemberById(id);
        
        if (member == null)
            return NotFound();
        
        return member;
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteMember(int id)
    {
        var result = _memberService.DeleteMember(id);
        
        if (!result)
            return NotFound();

        return NoContent();
    }
}