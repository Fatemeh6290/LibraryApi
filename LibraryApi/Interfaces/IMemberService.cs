using LibraryApi.Models;

namespace LibraryApi.Interfaces;

public interface IMemberService
{
    List<Member> GetMembers();
    Member? GetMemberById(int id);
    Member? AddMember(string name, string email);
    bool DeleteMember(int id);
}