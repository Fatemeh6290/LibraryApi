using LibraryApi.Controllers;
using LibraryApi.DTOs;
using LibraryApi.Interfaces;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryApiTests;

public class MemberControllerUnitTests
{
    [Fact]
    public void GetMembers_ShouldReturnAllMembers()
    {
        //Arrange
        var members = new List<Member>
        {
            new Member
            {
                MemberId = 1,
                Name = "Jahn",
                Email = "jahn@gmail.com"
            },
            new Member
            {
                MemberId = 2,
                Name = "Jane",
                Email = "jane@gmail.com"
            }
        };
        
        var mockMemberService = new Mock<IMemberService>();
        mockMemberService.Setup(x => x.GetMembers()).Returns(members);
        var controller = new MemberController(mockMemberService.Object);
        
        //Act
        var result = controller.GetMembers();
        
        //Assert
        Assert.NotNull(result.Value);
        
        Assert.Equal(2, result.Value.Count);
        Assert.Equal(1, result.Value[0].MemberId);
        Assert.Equal("Jahn", result.Value[0].Name);
        Assert.Equal("jahn@gmail.com", result.Value[0].Email);
        
        mockMemberService.Verify(x => x.GetMembers(), Times.Once);    
    }
    
    [Fact]
    public void GetMemberById_ShouldReturnMember()
    {
        //Arrange
        var members = new List<Member>
        {
            new Member
            {
                MemberId = 1,
                Name = "Jahn",
                Email = "jahn@gmail.com"
            }
        };
        
        var mockMemberService = new Mock<IMemberService>();
        mockMemberService.Setup(x => x.GetMemberById(1)).Returns(members[0]);
        var controller = new MemberController(mockMemberService.Object);
        
        //Act
        var result = controller.GetMemberById(1);
        
        //Assert
        Assert.NotNull(result.Value);
        Assert.Equal(1, result.Value.MemberId);
        Assert.Equal("Jahn", result.Value.Name);
        Assert.Equal("jahn@gmail.com", result.Value.Email);
        
        mockMemberService.Verify(x => x.GetMemberById(1), Times.Once);    
    }
    
    [Fact]
    public void GetMemberById_ShouldReturnNotFound()
    {
        var mockMemberService = new Mock<IMemberService>();
        mockMemberService.Setup(x => x.GetMemberById(9999)).Returns((Member?)null);
        var controller = new MemberController(mockMemberService.Object);
        
        //Act
        var result = controller.GetMemberById(9999);
        
        //Assert
        Assert.IsType<NotFoundResult>(result.Result);
        
        mockMemberService.Verify(x => x.GetMemberById(9999), Times.Once); 
    }
    
    [Fact]
    public void AddMember_ShouldReturnCreatedMember()
    {
        //Arrange
        var member = new Member
        {
            MemberId = 1,
            Name = "John",
            Email = "john@gmail.com"
        };
        
        var mockMemberService = new Mock<IMemberService>();
        mockMemberService.Setup(x => x.AddMember("John", "john@gmail.com"))
            .Returns(member);
        var controller = new MemberController(mockMemberService.Object);

        var request = new CreateMemberDto
        {
            Name = "John",
            Email = "john@gmail.com"
        };
        
        //Act
        var result = controller.AddMember(request);
        
        //Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdMember = Assert.IsType<MemberDto>(createdResult.Value);
        
        Assert.Equal("John", createdMember.Name);
        Assert.Equal("john@gmail.com", createdMember.Email);
        
        mockMemberService.Verify(x => x.AddMember("John", "john@gmail.com"), Times.Once);     
    }
    
    [Fact]
    public void AddMember_ShouldReturnBadRequest()
    {
        var mockMemberService = new Mock<IMemberService>();
        mockMemberService.Setup(x => x.AddMember("John", "john@gmail.com"))
            .Returns((Member?)null);
        var controller = new MemberController(mockMemberService.Object);
        
        var request = new CreateMemberDto
        {
            Name = "John",
            Email = "john@gmail.com"
        };
        
        //Act
        var result = controller.AddMember(request);
        
        //Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
        
        mockMemberService.Verify(x => x.AddMember("John", "john@gmail.com"), Times.Once);
    }
    
    [Fact]
    public void DeleteMember_ShouldReturnNoContent()
    {
        var mockMemberService = new Mock<IMemberService>();
        mockMemberService.Setup(x => x.DeleteMember(1))
            .Returns(true);
        var controller = new MemberController(mockMemberService.Object);
        
        //Act
        var result = controller.DeleteMember(1);
        
        //Assert
        Assert.IsType<NoContentResult>(result);
        
        mockMemberService.Verify(x => x.DeleteMember(1), Times.Once);
    }
    
    [Fact]
    public void DeleteMember_ShouldReturnNotFound()
    {
        var mockMemberService = new Mock<IMemberService>();
        mockMemberService.Setup(x => x.DeleteMember(9999))
            .Returns(false);
        var controller = new MemberController(mockMemberService.Object);
        
        //Act
        var result = controller.DeleteMember(9999);
        
        //Assert
        Assert.IsType<NotFoundResult>(result);
        
        mockMemberService.Verify(x => x.DeleteMember(9999), Times.Once);
    }
}