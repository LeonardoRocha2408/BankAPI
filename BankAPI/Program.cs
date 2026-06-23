using Microsoft.EntityFrameworkCore;
using BankShared;
using BankAPI.Entities;
using BankAPI.Methods;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<BankDbContext>(contextOptions =>
    contextOptions.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<UserServices>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapPost("/users/create-account", (CreateAccountRequest request, UserServices user) =>
{
    var userCreated = user.CreateAccount(request.Username, request.PasswordLogin, request.PasswordTransaction);

    if (userCreated == null)
    {
        return Results.BadRequest("Account already exists");
    }
    return Results.Accepted("Account created sucessfully");
});
app.Run();
