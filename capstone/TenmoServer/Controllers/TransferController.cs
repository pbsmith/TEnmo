using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly IAccountDao daoAccount;
        private readonly ITransferDao daoTransfer;
        public TransferController(IAccountDao accountDao, ITransferDao transferDao)
        {
            daoAccount = accountDao;
            daoTransfer = transferDao;
        }

        [HttpGet]
        public ActionResult<List<Transfer>> ListTransfers()
        {

            List<Transfer> transfers = daoTransfer.ListTransfers();

            if(transfers == null)
            {
                return NotFound();
            }
            return Ok(transfers);
        }

        [HttpGet("{id}")]
        public ActionResult<Transfer> GetTransferById(int id)
        {
            Transfer transfer = daoTransfer.GetTransferById(id);

            if(transfer == null)
            {
                return NotFound();
            }
            return transfer;
        }

        [HttpPost]
        public ActionResult<Transfer> CreateTransfer(Transfer transfer)
        {
            Transfer newTransfer = daoTransfer.CreateTransfer(transfer);

            Account accountFrom = daoAccount.GetAccountById(newTransfer.AccountFrom);
            Account accountTo = daoAccount.GetAccountById(newTransfer.AccountTo);

            
            accountFrom.Balance -= newTransfer.Amount;
            accountTo.Balance += newTransfer.Amount;

            daoAccount.UpdateRecipient(accountFrom);
            daoAccount.UpdateRecipient(accountTo);

            return Created($"/transfer/{newTransfer.TransferId}", newTransfer);

        }

        [HttpGet("{id}/details")]
        public ActionResult<Transfer> GetTransferDetails(int id)
        {
            Transfer transfer = daoTransfer.TransferDetails(id);

            if (transfer == null)
            {
                return NotFound();
            }
            return transfer;
        }
    }
}
