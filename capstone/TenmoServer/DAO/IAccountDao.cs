using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountDao
    {

        Account GetAccountByUsername(User user);

        Account UpdateRecipient(Account updatedAccount);

        Account GetAccountByUserId(int id);

        Account GetAccountById(int accountId);
    }
}
