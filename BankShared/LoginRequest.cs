using System;
using System.Collections.Generic;
using System.Text;

namespace BankShared
{
    public record LoginRequest(
        string Username, 
        string PasswordLogin);
}
