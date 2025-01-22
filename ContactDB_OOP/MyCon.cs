using MySql.Data.MySqlClient;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ContactDB_OOP
{
    public class MyCon
    {
        private readonly string connectionString;

        public MyCon ()
        {
            connectionString = "Server=localhost;" +
                             "Port=3306;" +
                             "Database=contact_db;" +
                             "User ID=root;" +
                             "Password=;";
        }

        public DbConnection GetConnection()
        {
            try
            {
                return new MySqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create database connection.", ex);
            }
        }
    }
}
