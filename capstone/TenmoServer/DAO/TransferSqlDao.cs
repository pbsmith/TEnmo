using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TenmoServer.Exceptions;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDao : ITransferDao
    {
        private readonly string connectionString;

        public TransferSqlDao(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Transfer CreateTransfer(Transfer transfer)
        {
            Transfer newTransfer = new Transfer();
            int newTransferId;

            string sql = "INSERT INTO transfer (transfer_type_id, transfer_status_id, account_from, " +
                "account_to, amount) " +
                "OUTPUT INSERTED.transfer_id " +
                "VALUES (@transfer_type_id, @transfer_status_id, @account_from, @account_to, @amount);";

            try
            {
                using(SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@transfer_type_id", transfer.TransferTypeId);
                    cmd.Parameters.AddWithValue("@transfer_status_id", transfer.TransferStatusId);
                    cmd.Parameters.AddWithValue("@account_from", transfer.AccountFrom);
                    cmd.Parameters.AddWithValue("@account_to", transfer.AccountTo);
                    cmd.Parameters.AddWithValue("@amount", transfer.Amount);

                    newTransferId = Convert.ToInt32(cmd.ExecuteScalar());
                }
                newTransfer = GetTransferById(newTransferId);
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
            return transfer;
        }

        public Transfer GetTransferById(int id)
        {
            Transfer transfer = new Transfer();

            string sql = "SELECT * FROM transfer WHERE transfer_id = @transfer_id;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@transfer_id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        transfer.TransferId = Convert.ToInt32(reader["transfer_id"]);
                        transfer.TransferTypeId = Convert.ToInt32(reader["transfer_type_id"]);
                        transfer.TransferStatusId = Convert.ToInt32(reader["transfer_status_id"]);
                        transfer.AccountFrom = Convert.ToInt32(reader["account_from"]);
                        transfer.AccountTo = Convert.ToInt32(reader["account_to"]);
                        transfer.Amount = Convert.ToDecimal(reader["amount"]);
                    }

                }

            }
            catch(SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
            return transfer; 

        }

        public List<Transfer> ListTransfers()
        {
            List<Transfer> transferList = new List<Transfer>();

            string sql = "SELECT * FROM transfer;";

            try
            {
                using(SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while(reader.Read())
                    {
                        Transfer transfer = new Transfer();

                        transfer.TransferId = Convert.ToInt32(reader["transfer_id"]);
                        transfer.TransferTypeId = Convert.ToInt32(reader["transfer_type_id"]);
                        transfer.TransferStatusId = Convert.ToInt32(reader["transfer_status_id"]);
                        transfer.AccountFrom = Convert.ToInt32(reader["account_from"]);
                        transfer.AccountTo = Convert.ToInt32(reader["account_to"]);
                        transfer.Amount = Convert.ToDecimal(reader["amount"]);
                        transferList.Add(transfer);
                    }

                }
            }
            catch(SqlException ex)
            {
                throw new DaoException("Sql exception occurred", ex);
            }

            return transferList;
        }

        public Transfer TransferDetails(int id)
        {
            Transfer transfer = new Transfer();

            string sql = "SELECT transfer_id, account_from, account_to, transfer.transfer_type_id, transfer.transfer_status_id, transfer_type_desc, transfer_status_desc, amount " +
               "FROM transfer " +
               "JOIN transfer_status ON transfer_status.transfer_status_id = transfer.transfer_status_id " +
               "JOIN transfer_type ON transfer_type.transfer_type_id = transfer.transfer_type_id " +
               "WHERE transfer_id = @transfer_id;"; 

            try
            {
                using(SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@transfer_id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if(reader.Read())
                    {
                        transfer.TransferId = Convert.ToInt32(reader["transfer_id"]);
                        transfer.AccountFrom = Convert.ToInt32(reader["account_from"]);
                        transfer.AccountTo = Convert.ToInt32(reader["account_to"]);
                        transfer.Amount = Convert.ToDecimal(reader["amount"]);
                        transfer.TransferStatus = Convert.ToString(reader["transfer_status_desc"]);
                        transfer.TransferType = Convert.ToString(reader["transfer_type_desc"]);
                        transfer.TransferTypeId = Convert.ToInt32(reader["transfer_type_id"]);
                        transfer.TransferStatusId = Convert.ToInt32(reader["transfer_status_id"]);
                    }
                }
            }
            catch(SqlException ex)
            {
                throw new DaoException("SqlException occurred", ex);
            }
            return transfer;
        }
    }
}
