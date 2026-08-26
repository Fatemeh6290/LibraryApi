using LibraryApi.Controllers;
using LibraryApi.DTOs;
using LibraryApi.Interfaces;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryApiTests;

public class LoanControllerUnitTests
{
    [Fact]
    public void GetLoans_ShouldReturnLoans()
    {
        // Arrange
        var loans = new List<Loan>
        {
            new Loan
            {
                LoanId = 1,
                BookId = 10,
                MemberId = 20
            }
        };

        var mockLoanService = new Mock<ILoanService>();
        mockLoanService.Setup(x => x.GetLoans()).Returns(loans);
        var controller = new LoanController(mockLoanService.Object);

        // Act
        var result = controller.GetLoans();

        // Assert
        Assert.NotNull(result.Value);
        Assert.Single(result.Value);
        Assert.Equal(10, result.Value[0].BookId);
        Assert.Equal(20, result.Value[0].MemberId);
        
        mockLoanService.Verify(x => x.GetLoans(), Times.Once);
    }

    [Fact]
    public void GetLoanById_ShouldReturnLoan()
    {
        //Arrange
        var loan = new Loan
        {
            LoanId = 1,
            BookId = 10,
            MemberId = 20
        };
        
        var mockLoanService = new Mock<ILoanService>();
        mockLoanService.Setup(x => x.GetLoanById(1)).Returns(loan);
        var controller = new LoanController(mockLoanService.Object);
        
        //Act
        var result = controller.GetLoanById(1);
        
        //Assert
        Assert.NotNull(result.Value);
        Assert.Equal(1, result.Value.LoanId);
        Assert.Equal(10, result.Value.BookId);
        Assert.Equal(20, result.Value.MemberId);
        
        mockLoanService.Verify(x => x.GetLoanById(1), Times.Once);
    }

    [Fact]
    public void GetLoanById_ShouldReturnNotFound()
    {
        //Arrange
        var mockLoanService = new Mock<ILoanService>();
        mockLoanService.Setup(x => x.GetLoanById(9999)).Returns((Loan?)null);
        var controller = new LoanController(mockLoanService.Object);
        
        //Act
        var result = controller.GetLoanById(9999);
        
        //Assert
        Assert.IsType<NotFoundResult>(result.Result);
        
        mockLoanService.Verify(x => x.GetLoanById(9999), Times.Once);
    }
    
    [Fact]
    public void AddLoan_ShouldReturnCreatedLoan()
    {
        //Arrange
        var loan = new Loan
        {
            LoanId = 1,
            BookId = 10,
            MemberId = 20
        };
        
        var mockLoanService = new Mock<ILoanService>();
        mockLoanService.Setup(x => x.AddLoan(10, 20)).Returns(loan);
        var controller = new LoanController(mockLoanService.Object);

        var request = new CreateLoanDto
        {
            BookId = 10,
            MemberId = 20
        };
        
        //Act
        var result = controller.AddLoan(request);
        
        //Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);

        var createdLoan = Assert.IsType<LoanDto>(createdResult.Value);

        Assert.Equal(1, createdLoan.LoanId);
        Assert.Equal(10, createdLoan.BookId);
        Assert.Equal(20, createdLoan.MemberId);
        
        mockLoanService.Verify(x => x.AddLoan(10, 20), Times.Once);
    }

    [Fact]
    public void GetLoans_ShouldReturnEmpty_WhenNoLoansExist()
    {
        //Arrange
        var mockLoanService = new Mock<ILoanService>();
        mockLoanService.Setup(x => x.GetLoans()).Returns(new List<Loan>());
        var controller = new LoanController(mockLoanService.Object);
        
        //Act
        var result = controller.GetLoans();
        
        //Assert
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value);
        
        mockLoanService.Verify(x => x.GetLoans(), Times.Once);
    }
    
    [Fact]
    public void AddLoan_ShouldReturnBadRequest_WhenLoanCannotBeCreated()
    {
        //Arrange
        var mockLoanService = new Mock<ILoanService>();
        mockLoanService.Setup(x => x.AddLoan(9999, 20)).Returns((Loan?)null);
        var controller = new LoanController(mockLoanService.Object);

        var request = new CreateLoanDto
        {
            BookId = 9999,
            MemberId = 20
        };
        
        //Act
        var result = controller.AddLoan(request);
        
        //Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
        
        mockLoanService.Verify(x => x.AddLoan(9999, 20), Times.Once);
    }
    
    [Fact]
    public void ReturnLoan_ShouldReturnNoContent()
    {
        // Arrange
        var mockLoanService = new Mock<ILoanService>();
        mockLoanService.Setup(x => x.ReturnBook(1)).Returns(true);
        var controller = new LoanController(mockLoanService.Object);

        // Act
        var result = controller.ReturnBook(1);

        // Assert
        Assert.IsType<NoContentResult>(result);

        mockLoanService.Verify(x => x.ReturnBook(1), Times.Once);
    }
    
    [Fact]
    public void ReturnLoan_ShouldReturnNotFound()
    {
        // Arrange
        var mockLoanService = new Mock<ILoanService>();
        mockLoanService.Setup(x => x.ReturnBook(9999)).Returns(false);
        var controller = new LoanController(mockLoanService.Object);

        // Act
        var result = controller.ReturnBook(9999);

        // Assert
        Assert.IsType<NotFoundResult>(result);

        mockLoanService.Verify(x => x.ReturnBook(9999), Times.Once);
    }
}