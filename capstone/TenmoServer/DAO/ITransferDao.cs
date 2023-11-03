using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {
        Transfer CreateTransfer(Transfer transfer);

        List<Transfer> ListTransfers();


        Transfer GetTransferById(int id);

        Transfer TransferDetails(int id);
    }
}
