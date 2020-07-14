using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.DAO
{
    public class AccountSqlDAO : IAccountDAO
    {
        private readonly string connectionString;
        public AccountSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        public Account GetAccount(int userID)
        {
            Account account = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT balance, user_id, account_id FROM accounts WHERE user_id = @user_id", conn);
                    cmd.Parameters.AddWithValue("@user_id", userID);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows && reader.Read())
                    {
                        account = GetAccountFromReader(reader);
                    }

                }
            }
            catch (SqlException)
            {
                throw;
            }
            return account;
        }

        public decimal GetBalance(int userID)
        {
            Account account = new Account();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT balance FROM accounts JOIN users ON users.user_id = accounts.account_id WHERE users.user_id = @user_id", conn);
                    cmd.Parameters.AddWithValue("@user_id", userID);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            account.Balance = Convert.ToDecimal(reader["balance"]);
                        }
                    }

                }
            }
            catch (SqlException)
            {
                throw;
            }
            return account.Balance;
        }

        public Account GetAccountFromReader(SqlDataReader reader)
        {
            Account account = new Account()
            {
                AccountID = Convert.ToInt32(reader["account_id"]),
                UserID = Convert.ToInt32(reader["user_id"]),
                Balance = Convert.ToDecimal(reader["balance"]),
            };
            return account;
        }
    }
}
