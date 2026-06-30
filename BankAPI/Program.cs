using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using BankShared;
using BankAPI.Entities;
using BankAPI.Methods;
using BankAPI.Enums;

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

User CurrentUser = new User();

app.MapPost("/users/create-account", async ([FromBody] CreateAccountRequest request, UserServices user) =>
{
    var userCreated = await user.CreateAccount(request.Username, request.PasswordLogin, request.PasswordTransaction);

    if (userCreated == null)
    {
        return Results.BadRequest("Account already exists");
    }
    CurrentUser = userCreated;
    return Results.Accepted("Account created successfully");
});

app.MapPost("/users/login", async ([FromBody] LoginRequest request, UserServices user) =>
{
    var userLogin = await user.LoginAccount(request.Username, request.PasswordLogin);

    if (userLogin == null)
    {
        return Results.BadRequest("Account not exists"); 
    }
    CurrentUser = userLogin;
    return Results.Ok("Login successfully"); 
});

app.MapPatch("/users/transfer-money", async ([FromBody] TransferMoneyRequest request, UserServices user) =>
{
    TransferResult result = await user.TransferMoney(CurrentUser, request.MoneyToTransfer, request.UserToTransfer, request.PasswordTransaction);

    if (CurrentUser == null) 
    {
        return Results.Unauthorized();
    }
    return result switch
    {
        TransferResult.UserNotFound => Results.BadRequest("User not found"),

        TransferResult.InvalidPassword => Results.Unauthorized(),

        TransferResult.InsufficientBalance => Results.UnprocessableEntity("Balance is insufficient for the transaction"),

        TransferResult.Sucess => Results.Accepted("Your transfer happened successfully"),

        _ => Results.BadRequest("Unknown error")
    };
});

app.MapDelete("/users/delete", async ([FromBody] DeleteAccountRequest request, UserServices user) => 
{
    DeleteAccountResult result = await user.DeleteAccount(CurrentUser, request.Username, request.PasswordLogin);

    if (CurrentUser == null)
    {
        return Results.Unauthorized();
    }
    return result switch
    {
        DeleteAccountResult.AccountNotFound => Results.BadRequest("Data is not correct"),
        DeleteAccountResult.AccountDeletedSuccessfully => Results.Accepted("Account delete with successfully"),
        _ => Results.BadRequest("Unknow error")
    };
});
app.Run();
