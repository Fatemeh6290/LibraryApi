namespace LibraryApi.DTOs;

public class MemberDto
{
    public int MemberId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}