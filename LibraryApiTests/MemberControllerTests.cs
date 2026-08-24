using System.Net;
using System.Net.Http.Json;
using LibraryApi.DTOs;

namespace LibraryApiTests;

public class MemberControllerTests : IClassFixture<CustomWebApplicationFactory> 
{
    private readonly HttpClient _client;
    public MemberControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }
    [Fact]
    public async Task GetMembers_ShouldReturnSuccess()
    {
        //Act
        var respone = await _client.GetAsync("/api/Member");
        
        //Assert
        Assert.Equal(HttpStatusCode.OK, respone.StatusCode);
    }
    
    [Fact]
    public async Task GetMembers_ShouldReturnMembers()
    {
        //Arrang
        var request = new CreateMemberDto
        {
            Name = "John Doe",
            Email = "john.doe@example.com"
        };
        await _client.PostAsJsonAsync("/api/Member", request);
        
        //Act
        var respone = await _client.GetAsync("/api/Member");
        var members = await respone.Content.ReadFromJsonAsync<List<MemberDto>>();
        
        //Assert
        Assert.NotNull(members);
        Assert.Contains(members, x => x.Name == "John Doe" && x.Email == "john.doe@example.com");
    }
    
    [Fact]
    public async Task GetMemberById_ShouldReturnMember()
    {
        //Arrang
        var request = new CreateMemberDto
        {
            Name = "Tim Müller",
            Email = "tim@example.com"
        };
        var createdResponse = await _client.PostAsJsonAsync("/api/Member", request);
        
        //Act
        var createdMember = await createdResponse.Content.ReadFromJsonAsync<MemberDto>();
        var respone = await _client.GetAsync($"/api/Member/{createdMember!.MemberId}");
        var member = await respone.Content.ReadFromJsonAsync<MemberDto>();
        
        //Assert
        Assert.NotNull(member);
        Assert.Equal("Tim Müller", member.Name);
        Assert.Equal("tim@example.com", member.Email);
    }
    
    [Fact]
    public async Task GetMemberById_ShouldReturnNotFound()
    {
        //Act
        var response = await _client.GetAsync("/api/Member/9999");
        
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task CreateMember_ShouldReturnCreatedMember()
    {
        //Arrang
        var request = new CreateMemberDto
        {
            Name = "Peter Stein",
            Email = "peter@example.com"
        };
        var createdResponse = await _client.PostAsJsonAsync("/api/Member", request);
        
        //Act
        var createdMember = await createdResponse.Content.ReadFromJsonAsync<MemberDto>();
        var respone = await _client.GetAsync($"/api/Member/{createdMember!.MemberId}");
        var member = await respone.Content.ReadFromJsonAsync<MemberDto>();
        
        //Assert
        Assert.NotNull(member);
        Assert.Equal(request.Name, member.Name);
        Assert.Equal(request.Email, member.Email);
    }
    
    [Fact]
    public async Task CreateMember_ShouldReturnBadRequest_WhenInvalidData()
    {
        //Arrang
        var request = new CreateMemberDto
        {
            Name = "",
            Email = "john.doe@example.com"
        };
        
        //Act
        var createdResponse = await _client.PostAsJsonAsync("/api/Member", request);
        
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, createdResponse.StatusCode);
    }
    
    [Fact]
    public async Task CreateMember_ShouldReturnBadRequest_WhenEmailAlreadyExists()
    {
        //Arrang
        var request1 = new CreateMemberDto
        {
            Name = "John Doe",
            Email = "john.doe@example.com"
        };
        
        var request2 = new CreateMemberDto
        {
            Name = "John Doe",
            Email = "john.doe@example.com"
        };
        
        //Act
        await _client.PostAsJsonAsync("/api/Member", request1);
        var createdResponse = await _client.PostAsJsonAsync("/api/Member", request2);
        
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, createdResponse.StatusCode);
    }
    
    [Fact]
    public async Task DeleteMember_ShouldReturnSuccess()
    {
        //Arrang
        var request = new CreateMemberDto
        {
            Name = "John Doe",
            Email = "john.doe@example.com"
        };
        
        var createdResponse = await _client.PostAsJsonAsync("/api/Member", request);
        var createdMember = await createdResponse.Content.ReadFromJsonAsync<MemberDto>();

        //Act
        var response = await _client.DeleteAsync($"/api/Member/{createdMember!.MemberId}");
        
        //Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteMember_ShouldReturnNotFound()
    {
        //Act
        var deletedMember = await _client.DeleteAsync("/api/Member/999999");
        
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, deletedMember.StatusCode);
    }
}