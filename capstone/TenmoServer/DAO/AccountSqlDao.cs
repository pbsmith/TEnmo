using System;
using System.Data.SqlClient;
using TenmoServer.Exceptions;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class AccountSqlDao : IAccountDao
    {
        private readonly string connectionString;

        public AccountSqlDao(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Account GetAccountByUsername(User user)
        {
            Account account = new Account();

            string sql = "SELECT * FROM account " +
                "JOIN tenmo_user ON tenmo_user.user_id = account.user_id " +
                "WHERE username = @username;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        account.AccountId = Convert.ToInt32(reader["account_id"]);
                        account.UserId = Convert.ToInt32(reader["user_id"]);
                        account.Balance = Convert.ToDecimal(reader["balance"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("Unable to access account.", ex);
            }
            return account;
        }

        public Account UpdateRecipient(Account updatedAccount)
        {
            Account account = null;

            string sql = "UPDATE account SET balance = @balance WHERE user_id = @user_id;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@balance", updatedAccount.Balance);
                    cmd.Parameters.AddWithValue("@user_id", updatedAccount.UserId);

                    int count = cmd.ExecuteNonQuery();

                    if (count == 0)
                    {
                        return null;
                    }
                    account = GetAccountById(updatedAccount.AccountId);
                }
                return account;
            }
            catch (SqlException ex)
            {
                throw new DaoException("Unable to access account or account does not exist.", ex);
            }
        }

        public Account GetAccountById(int accountId)
        {
            Account account = new Account();

            string sql = "SELECT * FROM account WHERE account_id = @account_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@account_id", accountId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        account.AccountId = Convert.ToInt32(reader["account_id"]);
                        account.Balance = Convert.ToDecimal(reader["balance"]);
                        account.UserId = Convert.ToInt32(reader["user_id"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return account;
        }
        public Account GetAccountByUserId(int userId)
        {
            Account account = new Account();

            string sql = "SELECT * FROM account WHERE user_id = @user_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if(reader.Read())
                    {
                        account.AccountId = Convert.ToInt32(reader["account_id"]);
                        account.Balance = Convert.ToDecimal(reader["balance"]);
                        account.UserId = Convert.ToInt32(reader["user_id"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return account;
        }
    }
}
