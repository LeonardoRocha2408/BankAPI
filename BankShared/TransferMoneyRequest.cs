using System;
using System.Collections.Generic;
using System.Text;

namespace BankShared
{
    public record TransferMoneyRequest(float MoneyToTransfer, string UserToTransfer, string PasswordTransaction) { }
}
