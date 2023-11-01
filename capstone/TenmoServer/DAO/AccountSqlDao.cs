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

        public Account GetBalance(User user)
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
    }
}
