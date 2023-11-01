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
        private readonly IUserDao dao;
        public TransferController(IUserDao userDao)
        {
            dao = userDao;

        }

        [HttpGet()]
        public ActionResult<IList<User>> GetUserList()
        {
            IList<User> users = dao.GetUsers();

            if(users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }

    }
}
