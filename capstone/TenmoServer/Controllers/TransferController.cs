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
        public TransferController(IAccountDao accountDao)
        {
            daoAccount = accountDao;
        }

        [HttpPut("{id}")]
        public ActionResult<Account> UpdateAccount(int id, Account account)
        {
            account.UserId = id;

            try
            {
                Account result = daoAccount.UpdateRecipient(account);
                return Ok(result);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet]
        public ActionResult<List<Transfer>> ListTransfers()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public ActionResult<Transfer> GetTransferById(int id)
        {
            return Ok();
        }

        [HttpPost]
        public ActionResult CreateTransfer(Transfer transfer)
        {
            return Ok();
        }
    }
}
