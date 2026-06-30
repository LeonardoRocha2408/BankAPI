using System;
using System.Collections.Generic;
using System.Text;

namespace BankShared
{
    public record DeleteAccountRequest(
        string Username,
        string PasswordLogin);
}
