using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ContactDB_OOP
{
    public class ContactRepository : BaseRepository, IContactRepository
    {
        public ContactRepository(MyCon dbConnection) : base(dbConnection)
        {
        }

        public async Task<IEnumerable<Contact>> GetAllContactsAsync()
        {
            const string query = @"
                SELECT user_id, user_name, bday, city_id, 
                       company_id, position_id, gender_id, 
                       sim_id, contact_image 
                FROM tbl_contact";

            var contacts = new List<Contact>();
            DbConnection connection = null;
            DbCommand command = null;
            DbDataReader reader = null;

            try
            {
                connection = _dbConnection.GetConnection();
                command = connection.CreateCommand();
                command.CommandText = query;

                await connection.OpenAsync();
                reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    contacts.Add(MapContact(reader));
                }

                return contacts;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                if (command != null) command.Dispose();
                if (connection != null) connection.Dispose();
            }
        }

        public async Task<Contact> GetContactByIdAsync(int userId)
        {
            const string query = @"
                SELECT user_id, user_name, bday, city_id, 
                       company_id, position_id, gender_id, 
                       sim_id, contact_image 
                FROM tbl_contact 
                WHERE user_id = @UserId";

            DbConnection connection = null;
            DbCommand command = null;
            DbDataReader reader = null;

            try
            {
                connection = _dbConnection.GetConnection();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.Add(new MySqlParameter("@UserId", userId));

                await connection.OpenAsync();
                reader = await command.ExecuteReaderAsync();

                return await reader.ReadAsync() ? MapContact(reader) : null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                if (command != null) command.Dispose();
                if (connection != null) connection.Dispose();
            }
        }

        private static Contact MapContact(DbDataReader reader)
        {
            return new Contact
            {
                user_id = Convert.ToInt32(reader["user_id"]),
                user_name = reader["user_name"] == DBNull.Value ? string.Empty : Convert.ToString(reader["user_name"]),
                bday = reader["bday"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["bday"]),
                city_id = reader["city_id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["city_id"]),
                company_id = reader["company_id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["company_id"]),
                position_id = reader["position_id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["position_id"]),
                gender_id = reader["gender_id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["gender_id"]),
                sim_id = reader["sim_id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["sim_id"]),
                contact_image = reader["contact_image"] == DBNull.Value ? string.Empty : Convert.ToString(reader["contact_image"])
            };
        }

        public async Task<bool> AddContactAsync(string name, DateTime bday, int genderId, int cityId, int positionId, int simId, int companyId, string contactImage = null)
        {
            const string query = @"
        INSERT INTO tbl_contact 
            (user_name, bday, city_id, company_id, 
             position_id, gender_id, sim_id, contact_image)
        VALUES 
            (@UserName, @Bday, @CityId, @CompanyId,
             @PositionId, @GenderId, @SimId, @ContactImage)";

            DbConnection connection = null;
            DbCommand command = null;

            try
            {
                connection = _dbConnection.GetConnection();
                command = connection.CreateCommand();
                command.CommandText = query;

                // Create parameters directly from method parameters
                command.Parameters.Add(new SqlParameter("@UserName", name));
                command.Parameters.Add(new SqlParameter("@Bday", bday));
                command.Parameters.Add(new SqlParameter("@CityId", cityId));
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));
                command.Parameters.Add(new SqlParameter("@PositionId", positionId));
                command.Parameters.Add(new SqlParameter("@GenderId", genderId));
                command.Parameters.Add(new SqlParameter("@SimId", simId));
                command.Parameters.Add(new SqlParameter("@ContactImage", contactImage ?? (object)DBNull.Value));

                await connection.OpenAsync();
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
            finally
            {
                if (command != null) command.Dispose();
                if (connection != null) connection.Dispose();
            }
        }

        public async Task<bool> UpdateContactAsync(Contact contact)
        {
            const string query = @"
        UPDATE tbl_contact 
        SET user_name = @UserName,
            bday = @Bday,
            city_id = @CityId,
            company_id = @CompanyId,
            position_id = @PositionId,
            gender_id = @GenderId,
            sim_id = @SimId,
            contact_image = @ContactImage
        WHERE user_id = @UserId";

            DbConnection connection = null;
            DbCommand command = null;

            try
            {
                connection = _dbConnection.GetConnection();
                command = connection.CreateCommand();
                command.CommandText = query;

                // Create parameters from the contact object
                command.Parameters.Add(new MySqlParameter("@UserName", contact.user_name));
                command.Parameters.Add(new MySqlParameter("@Bday", contact.bday == DateTime.MinValue ? (object)DBNull.Value : contact.bday));
                command.Parameters.Add(new MySqlParameter("@CityId", contact.city_id));
                command.Parameters.Add(new MySqlParameter("@CompanyId", contact.company_id));
                command.Parameters.Add(new MySqlParameter("@PositionId", contact.position_id));
                command.Parameters.Add(new MySqlParameter("@GenderId", contact.gender_id));
                command.Parameters.Add(new MySqlParameter("@SimId", contact.sim_id));
                command.Parameters.Add(new MySqlParameter("@ContactImage", contact.contact_image ?? (object)DBNull.Value)); // Handle optional contact image
                command.Parameters.Add(new MySqlParameter("@UserId", contact.user_id)); // Add user_id for the WHERE clause

                await connection.OpenAsync();
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
            finally
            {
                if (command != null) command.Dispose();
                if (connection != null) connection.Dispose();
            }
        }

        public async Task<bool> DeleteContactAsync(int userId)
        {
            const string query = "DELETE FROM tbl_contact WHERE user_id = @UserId";
            DbConnection connection = null;
            DbCommand command = null;

            try
            {
                connection = _dbConnection.GetConnection();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.Add(new MySqlParameter("@UserId", userId));

                await connection.OpenAsync();

                // Check if the contact exists before attempting to delete
                var existsQuery = "SELECT COUNT(*) FROM tbl_contact WHERE user_id = @UserId";
                using (var existsCommand = connection.CreateCommand())
                {
                    existsCommand.CommandText = existsQuery;
                    existsCommand.Parameters.Add(new MySqlParameter("@UserId", userId));

                    var count = Convert.ToInt32(await existsCommand.ExecuteScalarAsync());
                    if (count == 0)
                    {
                        return false; // Contact does not exist
                    }
                }

                // Execute the delete command
                var result = await command.ExecuteNonQueryAsync();
                return result > 0; // Return true if a row was deleted
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return false; // Indicate failure
            }
            finally
            {
                if (command != null) command.Dispose();
                if (connection != null) connection.Dispose();
            }
        }

        public async Task<IEnumerable<Contact>> SearchContactsByUserNameAsync(string userName)
        {
            const string query = @"
        SELECT user_id, user_name, bday, city_id, 
               company_id, position_id, gender_id, 
               sim_id, contact_image 
        FROM tbl_contact 
        WHERE user_name LIKE @UserName";

            var contacts = new List<Contact>();
            DbConnection connection = null;
            DbCommand command = null;
            DbDataReader reader = null;

            try
            {
                connection = _dbConnection.GetConnection();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.Add(new MySqlParameter("@UserName", $"%{userName}%")); // Use LIKE for partial matches

                await connection.OpenAsync();
                reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    contacts.Add(MapContact(reader));
                }

                return contacts;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                if (command != null) command.Dispose();
                if (connection != null) connection.Dispose();
            }
        }

        private static List<MySqlParameter> CreateParameters(Contact contact)
        {
            return new List<MySqlParameter>
            {
                new MySqlParameter("@UserName", contact.user_name),
                new MySqlParameter("@Bday", contact.bday),
                new MySqlParameter("@CityId", contact.city_id),
                new MySqlParameter("@CompanyId", contact.company_id),
                new MySqlParameter("@PositionId", contact.position_id),
                new MySqlParameter("@GenderId", contact.gender_id),
                new MySqlParameter("@SimId", contact.sim_id),
                new MySqlParameter("@ContactImage", contact.contact_image)
            };
        }
    }
}