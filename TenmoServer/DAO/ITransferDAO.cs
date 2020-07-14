using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer
{
    public interface ITransferDAO
    {
        Transfer InsertTransfer(Transfer transfer);

        List<Transfer> ListTransfers();

        Transfer GetTransfer(int id);

        Transfer UpdateBalance(Transfer transfer);
    }
}
