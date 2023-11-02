using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountDao dao;

        public AccountController(IAccountDao accountDao)
        {
            dao = accountDao;

        }

        [HttpGet()]
        public ActionResult<Account> GetAccountBalance()
        {
            User user = new User();

            user.Username = User.Identity.Name;

            if (User.Identity.Name == null)
            {
                return NotFound();
            }
            else
            {
                return dao.GetAccountByUsername(user);
            }

        }
     
        [HttpGet("{id}")]
        public ActionResult<Account> GetAccount(int id)
        {
            Account account = dao.GetAccountByUserId(id);

            if(account == null)
            {
                return NotFound();
            }
            return account;

        }

    }
}
