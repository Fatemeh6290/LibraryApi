using LibraryApi.Interfaces;
using LibraryApi.Middleware;
using LibraryApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IBookService, BookService>();
builder.Services.AddSingleton<IMemberService, MemberService>();
builder.Services.AddSingleton<ILoanService, LoanService>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();