using BankAPI.Entities;
namespace BankAPI.Methods;

public class UserServices
{
    private readonly BankDbContext _context;

    public UserServices(BankDbContext context)
    {
        _context = context;
    }

    public User? CreateAccount(string Username, string PasswordLogin, string PasswordTransaction)
    {
        User? user = _context.Users.Where(user => user.Username == Username).FirstOrDefault();
        
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
}
