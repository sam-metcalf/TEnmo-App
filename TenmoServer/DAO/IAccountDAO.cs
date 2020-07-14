using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.DAO
{
    public interface IAccountDAO
    {
        Account GetAccount(int userID);

        decimal GetBalance(int userID);
    }
}
