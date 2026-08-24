using LibraryApi.Data;
using LibraryApi.Interfaces;
using LibraryApi.Models;

namespace LibraryApi.Services;

public class MemberService: IMemberService
{
    private readonly LibraryDbContext _context;
    private readonly ILogger<MemberService> _logger;

    public MemberService(LibraryDbContext context, ILogger<MemberService> logger)
    {
        _context = context;
        _logger = logger;
    }
    public List<Member> GetMembers()
    {
        return _context.Members.ToList();
    }

    public Member? GetMemberById(int id)
    {
        return _context.Members.FirstOrDefault(x => x.MemberId == id);
    }

    public Member? AddMember(string name, string email)
    {
        if (_context.Members.Any(x => x.Email == email))
        {
            _logger.LogWarning("A member with the Email {Email} already exists.", email);
            return null;
        }
        
        var member = new Member
        {
            Name = name,
            Email = email,
        };
        
        _context.Members.Add(member);
        _context.SaveChanges();
        
        _logger.LogInformation("Member with id {MemberId} added.", member.MemberId);
        
        return member;
    }

    public bool DeleteMember(int id)
    {
        var member = _context.Members.FirstOrDefault(x => x.MemberId == id);

        if (member is null)
        {
            _logger.LogWarning("Member with id {MemberId} not found.", id);
            return false;
        }
        
        _context.Members.Remove(member);
        _context.SaveChanges();
        
        _logger.LogInformation("Member with id {MemberId} deleted.", member.MemberId);
        
        return true;
    }
}