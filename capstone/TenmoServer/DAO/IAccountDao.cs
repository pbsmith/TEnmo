using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountDao
    {

        Account GetBalance(User user);


    }
}
