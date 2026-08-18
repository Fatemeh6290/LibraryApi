using LibraryApi.Services;

namespace LibraryApiTests;

public class MemberServiceTests
{
    [Fact]
    public void AddMember_ShouldAddMember()
    {
        //Arrange
        MemberService memberservice = new ();

        //Act
        var result = memberservice.AddMember("Peter", "peter@gmail.com");
        var member = memberservice.GetMembers();
        
        //Assert
        Assert.True(result);
        Assert.Equal("Peter", member[0].Name);
    }
    [Fact]
    public void AddMember_ShouldReturnFalse_WhenEmailAlreadyExists()
    {
        //Arrange
        MemberService memberservice = new ();

        //Act
        memberservice.AddMember("Peter", "peter@gmail.com");
        var result = memberservice.AddMember("Peter", "peter@gmail.com");
        
        //Assert
        Assert.False(result);
    }
    [Fact]
    public void GetMembers_ShouldReturnAllMembers()
    {
        //Arrange
        MemberService memberservice = new ();

        //Act
        memberservice.AddMember("Peter", "peter@gmail.com");
        memberservice.AddMember("Tim", "tim@gmail.com");
        var result = memberservice.GetMembers();
        
        //Assert
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count);
    }
    [Fact]
    public void GetMemberById_ShouldReturnMember()
    {
        //Arrange
        MemberService memberservice = new ();

        //Act
        memberservice.AddMember("Peter", "peter@gmail.com");
        var result = memberservice.GetMemberById(1);
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal("Peter", result.Name);
        Assert.Equal("peter@gmail.com", result.Email);
    }
    [Fact]
    public void GetMemberById_ShouldReturnNull_WhenMemberDoesNotExist()
    {
        //Arrange
        MemberService memberservice = new ();

        //Act
        memberservice.AddMember("Peter", "peter@gmail.com");
        var result = memberservice.GetMemberById(2);
        
        //Assert
        Assert.Null(result);
    }
    [Fact]
    public void DeleteMember_ShouldDeleteMember()
    {
        //Arrange
        MemberService memberservice = new ();

        //Act
        memberservice.AddMember("Peter", "peter@gmail.com");
        var result = memberservice.DeleteMember(1);
        
        //Assert
        Assert.True(result);
    }
    [Fact]
    public void DeleteMember_ShouldReturnFalse_WhenMemberDoesNotExist()
    {
        //Arrange
        MemberService memberservice = new ();

        //Act
        memberservice.AddMember("Peter", "peter@gmail.com");
        var result = memberservice.DeleteMember(2);
        
        //Assert
        Assert.False(result);
        Assert.NotNull(memberservice.GetMemberById(1));
    }
}