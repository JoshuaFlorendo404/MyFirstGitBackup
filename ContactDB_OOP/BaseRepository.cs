using System;
using System.Data.Common;
using System.Drawing;
using System.Threading.Tasks;

namespace ContactDB_OOP
{
    public abstract class BaseRepository
    {
        protected readonly MyCon _dbConnection;

        protected BaseRepository(MyCon dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        protected async Task<T> ExecuteScalarAsync<T>(string query, params DbParameter[] parameters)
        {
            DbConnection connection = null;
            DbCommand command = null;
            try
            {
                connection = _dbConnection.GetConnection();
                command = connection.CreateCommand();

                command.CommandText = query;
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                await connection.OpenAsync();
                var result = await command.ExecuteScalarAsync();
                return (T)Convert.ChangeType(result, typeof(T));
            }
            finally
            {
                if (command != null) command.Dispose();
                if (connection != null) connection.Dispose();
            }
        }

        protected async Task<int> ExecuteNonQueryAsync(string query, params DbParameter[] parameters)
        {
            DbConnection connection = null;
            DbCommand command = null;
            try
            {
                connection = _dbConnection.GetConnection();
                command = connection.CreateCommand();

                command.CommandText = query;
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                await connection.OpenAsync();
                return await command.ExecuteNonQueryAsync();
            }
            finally
            {
                if (command != null) command.Dispose();
                if (connection != null) connection.Dispose();
            }
        }
    }
}
