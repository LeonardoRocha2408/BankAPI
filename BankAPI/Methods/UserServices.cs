using BankAPI.Entities;
using BankAPI.Enums;
using Microsoft.EntityFrameworkCore;
namespace BankAPI.Methods;

public class UserServices
{
    private readonly BankDbContext _context;

    public UserServices(BankDbContext context)
    {
        _context = context;
    }

    public async Task<User?> CreateAccount(string Username, string PasswordLogin, string PasswordTransaction)
    {
        User? user = await _context.Users.Where(user => user.Username == Username).FirstOrDefaultAsync();

        if (user == null)
        {
            user = new User()
            {
                Username = Username,
                PasswordLogin = PasswordLogin,
                PasswordTransaction = PasswordTransaction
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }
        return user = null;
    }

    public async Task<User?> LoginAccount(string Username, string PasswordLogin)
    {
        User? user = await _context.Users
            .Where(user => user.Username == Username && user.PasswordLogin == PasswordLogin)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return user;
        }
        return user;
    }

    public async Task<TransferResult> TransferMoney(User CurrentUser, float MoneyToTransfer, string UserToTransfer, string PasswordTransaction)
    {
        User? CurrentUserFromDB = await _context.Users
            .Where(user => user.Id == CurrentUser.Id)
            .FirstOrDefaultAsync();

        User? User = await _context.Users
            .Where(user => user.Username == UserToTransfer)
            .FirstOrDefaultAsync();

        if (User == null)
        {
            return TransferResult.UserNotFound;
        }
        else
        {
            if (PasswordTransaction != CurrentUser.PasswordTransaction)
            {
                return TransferResult.InvalidPassword;
            }
            else if (MoneyToTransfer > CurrentUser.Balance || MoneyToTransfer <= 0)
            {
                return TransferResult.InsufficientBalance;
            }
            
            CurrentUserFromDB?.Balance -= MoneyToTransfer;
            User.Balance += MoneyToTransfer;
            await _context.SaveChangesAsync();
        }
        return TransferResult.Sucess;
    }
}