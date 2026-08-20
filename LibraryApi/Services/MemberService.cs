using LibraryApi.Interfaces;
using LibraryApi.Models;

namespace LibraryApi.Services;

public class MemberService: IMemberService
{
    private readonly List<Member> _members = new();
    private readonly ILogger<MemberService> _logger;
    private int _memberId = 1;

    public MemberService(ILogger<MemberService> logger)
    {
        _logger = logger;
    }
    public List<Member> GetMembers()
    {
        return _members.ToList();
    }

    public Member? GetMemberById(int id)
    {
        return _members.FirstOrDefault(x => x.MemberId == id);
    }

    public Member? AddMember(string name, string email)
    {
        if (_members.Any(x => x.Email == email))
        {
            _logger.LogWarning("A member with the Email {Email} is already exist.", email);
            return null;
        }
        
        var member = new Member
        {
            MemberId = _memberId++,
            Name = name,
            Email = email,
        };
        
        _members.Add(member);
        _logger.LogInformation("Member with id {MemberId} added.", member.MemberId);
        
        return member;
    }

    public bool DeleteMember(int id)
    {
        var member = _members.FirstOrDefault(x => x.MemberId == id);

        if (member is null)
        {
            _logger.LogWarning("Member with id {MemberId} not found.", id);
            return false;
        }
        
        _members.Remove(member);
        _logger.LogInformation("Member with id {MemberId} deleted.", member.MemberId);
        
        return true;
    }
}