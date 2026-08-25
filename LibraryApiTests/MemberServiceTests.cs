using LibraryApi.Data;
using LibraryApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace LibraryApiTests;

public class MemberServiceTests
{
    private readonly LibraryDbContext _context;
    private readonly MemberService _memberService;
    public MemberServiceTests()
    {
        var options = new DbContextOptionsBuilder<LibraryDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        _context = new LibraryDbContext(options);
        _memberService = new (_context, NullLogger<MemberService>.Instance);
    }
    
    [Fact]
    public void AddMember_ShouldAddMember()
    {
        //Act
        var result = _memberService.AddMember("Peter", "peter@gmail.com");
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal("Peter", result.Name);
        Assert.Equal("peter@gmail.com", result.Email);
    }
    
    [Fact]
    public void AddMember_ShouldReturnNull_WhenEmailAlreadyExists()
    {
        //Arrange
        _memberService.AddMember("Peter", "peter@gmail.com");
        
        //Act
        var result = _memberService.AddMember("Peter", "peter@gmail.com");
        
        //Assert
        Assert.Null(result);
    }
    
    [Fact]
    public void GetMembers_ShouldReturnAllMembers()
    {
        //Arrange
        _memberService.AddMember("Peter", "peter@gmail.com");
        _memberService.AddMember("Tim", "tim@gmail.com");
        
        //Act
        var result = _memberService.GetMembers();
        
        //Assert
        Assert.Equal(2, result.Count);
    }
    
    [Fact]
    public void GetMemberById_ShouldReturnMember()
    {
        //Arrange
        _memberService.AddMember("Peter", "peter@gmail.com");
        
        //Act
        var result = _memberService.GetMemberById(1);
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal("Peter", result.Name);
        Assert.Equal("peter@gmail.com", result.Email);
    }
    
    [Fact]
    public void GetMemberById_ShouldReturnNull_WhenMemberDoesNotExist()
    {
        //Arrange
        _memberService.AddMember("Peter", "peter@gmail.com");
        
        //Act
        var result = _memberService.GetMemberById(2);
        
        //Assert
        Assert.Null(result);
    }
    
    [Fact]
    public void DeleteMember_ShouldDeleteMember()
    {
        //Arrange
        _memberService.AddMember("Peter", "peter@gmail.com");
        
        //Act
        var result = _memberService.DeleteMember(1);
        
        //Assert
        Assert.True(result);
        Assert.Null(_memberService.GetMemberById(1));
    }
    
    [Fact]
    public void DeleteMember_ShouldReturnFalse_WhenMemberDoesNotExist()
    {
        //Arrange
        _memberService.AddMember("Peter", "peter@gmail.com");
        
        //Act
        var result = _memberService.DeleteMember(2);
        
        //Assert
        Assert.False(result);
        Assert.NotNull(_memberService.GetMemberById(1));
    }
    
    [Fact]
    public void GetMembers_ShouldReturnEmpty_WhenNoMembersExist()
    {
        // Act
        var result = _memberService.GetMembers();

        // Assert
        Assert.Empty(result);
    }
}