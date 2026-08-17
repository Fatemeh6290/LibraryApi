using LibraryApi.Interfaces;
using LibraryApi.Models;

namespace LibraryApi.Services;

public class MemberService: IMemberService
{
    private readonly List<Member> _members = new();
    private int _memberId = 1;
    public List<Member> GetMembers()
    {
        return _members.ToList();
    }

    public Member? GetMemberById(int id)
    {
        return _members.FirstOrDefault(x => x.MemberId == id);
    }

    public bool AddMember(string name, string email)
    {
        if (_members.Any(x => x.Email == email))
            return false;
        
        _members.Add(new Member
        {
            MemberId = _memberId++,
            Name = name,
            Email = email,
        });
        return true;
    }

    public bool DeleteMember(int id)
    {
        var member = _members.FirstOrDefault(x => x.MemberId == id);
        
        if (member is null)
            return false;
        
        _members.Remove(member);
        return true;
    }
}