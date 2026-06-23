namespace BankShared
{
    public record CreateAccountRequest(
        string Username,
        string PasswordLogin,
        string PasswordTransaction);
}
