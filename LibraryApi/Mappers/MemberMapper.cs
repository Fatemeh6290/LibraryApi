using LibraryApi.DTOs;
using LibraryApi.Models;

namespace LibraryApi.Mapper;

public class MemberMapper
{
    public static MemberDto ToDto(Member member)
    {
        return new MemberDto
        {
            MemberId = member.MemberId,
            Name = member.Name,
            Email = member.Email
        };
    }
}