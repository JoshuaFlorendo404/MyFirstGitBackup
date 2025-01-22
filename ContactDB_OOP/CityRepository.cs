using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactDB_OOP
{
    public class CityRepository : BaseRepository, ICityRepository
    {
        public CityRepository(MyCon dbConnection) : base(dbConnection) { }

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

        public async Task<IEnumerable<City>> GetAllCitiesAsync()
        {
            const string query = "SELECT city_id, city_name FROM tbl_city";
            var cities = new List<City>();

            using (var command = await CreateCommandAsync(query))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    cities.Add(new City
                    {
                        city_id = reader.GetInt32(0),
                        city_name = reader.GetString(1)
                    });
                }
            }

            return cities;
        }

        public async Task<City> GetCityByIdAsync(int cityId)
        {
            const string query = "SELECT city_id, city_name FROM tbl_city WHERE city_id = @CityId";

            using (var command = await CreateCommandAsync(query, new MySqlParameter("@CityId", cityId)))
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new City
                    {
                        city_id = reader.GetInt32(0),
                        city_name = reader.GetString(1)
                    };
                }
            }

            return null;
        }

        public async Task<bool> AddCityAsync(string cityName)
        {
            const string query = "INSERT INTO tbl_city (city_name) VALUES (@CityName)";

            using (var command = await CreateCommandAsync(query, new MySqlParameter("@CityName", cityName)))
            {
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<bool> UpdateCityAsync(City city)
        {
            const string query = "UPDATE tbl_city SET city_name = @CityName WHERE city_id = @CityId";

            using (var command = await CreateCommandAsync(query,
                new MySqlParameter("@CityName", city.city_name),
                new MySqlParameter("@CityId", city.city_id)))
            {
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<bool> DeleteCityAsync(int cityId)
        {
            const string query = "DELETE FROM tbl_city WHERE city_id = @CityId";

            using (var command = await CreateCommandAsync(query, new MySqlParameter("@CityId", cityId)))
            {
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<IEnumerable<City>> SearchCitiesByNameAsync(string cityName)
        {
            const string query = "SELECT city_id, city_name FROM tbl_city WHERE city_name LIKE @CityName";
            var cities = new List<City>();

            using (var command = await CreateCommandAsync(query, new MySqlParameter("@CityName", $"%{cityName}%")))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    cities.Add(new City
                    {
                        city_id = reader.GetInt32(0),
                        city_name = reader.GetString(1)
                    });
                }
            }

            return cities;
        }
    }
}
