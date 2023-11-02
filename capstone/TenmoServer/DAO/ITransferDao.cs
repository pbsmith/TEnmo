using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {
        Transfer CreateTransfer();

        List<Transfer> ListTransfers();

        Transfer TransferDetails();
    }
}
