using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserDao daoUser;
        public UserController(IUserDao userDao)
        {
            daoUser = userDao;
        }

        [HttpGet]
        public ActionResult<IList<User>> GetUserList()
        {
            IList<User> users = daoUser.GetUsers();

            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }
    }
}
