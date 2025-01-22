using ContactDB_OOP;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using System.Windows.Forms;

public class GenderRepository : BaseRepository, IGenderRepository
{
    public GenderRepository(MyCon dbConnection) : base(dbConnection)
    {
    }

    private async Task<DbCommand> CreateCommandAsync(string query, params MySqlParameter[] parameters)
    {
        var connection = _dbConnection.GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = query;

        if (parameters != null && parameters.Length > 0)
        {
            command.Parameters.AddRange(parameters);
        }

        await connection.OpenAsync();
        return command;
    }

    public async Task<IEnumerable<Gender>> GetAllGendersAsync()
    {
        const string query = "SELECT gender_id, gender_name FROM tbl_gender";
        var genders = new List<Gender>();

        using (var command = await CreateCommandAsync(query))
        using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                genders.Add(new Gender
                {
                    gender_id = reader.GetInt32(0),
                    gender_name = reader.GetString(1)
                });
            }
        }

        return genders;
    }

    public async Task<Gender> GetGenderByIdAsync(int genderId)
    {
        const string query = "SELECT gender_id, gender_name FROM tbl_gender WHERE gender_id = @GenderId";
        DbConnection connection = null;
        DbCommand command = null;
        DbDataReader reader = null;

        try
        {
            connection = _dbConnection.GetConnection();
            command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new MySqlParameter("@GenderId", genderId));

            await connection.OpenAsync();
            reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Gender
                {
                    gender_id = reader.GetInt32(0),
                    gender_name = reader.GetString(1)
                };
            }

            return null; // Gender not found
        }
        finally
        {
            if (reader != null) reader.Dispose();
            if (command != null) command.Dispose();
            if (connection != null) connection.Dispose();
        }
    }

    public async Task<bool> AddGenderAsync(string genderName)
    {
        const string query = "INSERT INTO tbl_gender (gender_name) VALUES (@GenderName)";
        DbConnection connection = null;
        DbCommand command = null;

        try
        {
            connection = _dbConnection.GetConnection();
            command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new MySqlParameter("@GenderName", genderName));

            await connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();
            return result > 0; // Return true if a row was inserted
        }
        finally
        {
            if (command != null) command.Dispose();
            if (connection != null) connection.Dispose();
        }
    }

    public async Task<bool> UpdateGenderAsync(Gender gender)
    {
        const string query = "UPDATE tbl_gender SET gender_name = @GenderName WHERE gender_id = @GenderId";
        DbConnection connection = null;
        DbCommand command = null;

        try
        {
            connection = _dbConnection.GetConnection();
            command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new MySqlParameter("@GenderName", gender.gender_name));
            command.Parameters.Add(new MySqlParameter("@GenderId", gender.gender_id));

            await connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();
            return result > 0; // Return true if a row was updated
        }
        finally
        {
            if (command != null) command.Dispose();
            if (connection != null) connection.Dispose();
        }
    }

    public async Task<bool> DeleteGenderAsync(int genderId)
    {
        const string query = "DELETE FROM tbl_gender WHERE gender_id = @GenderId";
        DbConnection connection = null;
        DbCommand command = null;

        try
        {
            connection = _dbConnection.GetConnection();
            command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new MySqlParameter("@GenderId", genderId));

            await connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();
            return result > 0; // Return true if a row was deleted
        }
        finally
        {
            if (command != null) command.Dispose();
            if (connection != null) connection.Dispose();
        }
    }

    public async Task<IEnumerable<Gender>> SearchGendersByNameAsync(string genderName)
    {
        const string query = "SELECT gender_id, gender_name FROM tbl_gender WHERE gender_name LIKE @GenderName";
        var genders = new List<Gender>();
        DbConnection connection = null;
        DbCommand command = null;
        DbDataReader reader = null;

        try
        {
            connection = _dbConnection.GetConnection();
            command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new MySqlParameter("@GenderName", $"%{genderName}%")); // Use LIKE for partial matches

            await connection.OpenAsync();
            reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                genders.Add(new Gender
                {
                    gender_id = reader.GetInt32(0),
                    gender_name = reader.GetString(1)
                });
            }

            return genders;
        }
        finally
        {
            if (reader != null) reader.Dispose();
            if (command != null) command.Dispose();
            if (connection != null) connection.Dispose();
        }
    }
}