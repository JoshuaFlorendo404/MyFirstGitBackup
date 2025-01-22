using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactDB_OOP
{
    public class SimRepository : BaseRepository, ISimRepository
    {
        public SimRepository(MyCon dbConnection) : base(dbConnection) { }

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

        public async Task<IEnumerable<Sim>> GetAllSimsAsync()
        {
            const string query = "SELECT sim_id, sim_name FROM tbl_simcard";
            var sims = new List<Sim>();

            using (var command = await CreateCommandAsync(query))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    sims.Add(new Sim
                    {
                        sim_id = reader.GetInt32(0),
                        sim_name = reader.GetString(1)
                    });
                }
            }

            return sims;
        }

        public async Task<Sim> GetSimByIdAsync(int simId)
        {
            const string query = "SELECT sim_id, sim_name FROM tbl_simcard WHERE sim_id = @SimId";

            using (var command = await CreateCommandAsync(query, new MySqlParameter("@SimId", simId)))
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new Sim
                    {
                        sim_id = reader.GetInt32(0),
                        sim_name = reader.GetString(1)
                    };
                }
            }

            return null;
        }

        public async Task<bool> AddSimAsync(string simName)
        {
            const string query = "INSERT INTO tbl_simcard (sim_name) VALUES (@SimName)";

            using (var command = await CreateCommandAsync(query, new MySqlParameter("@SimName", simName)))
            {
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<bool> UpdateSimAsync(Sim sim)
        {
            const string query = "UPDATE tbl_simcard SET sim_name = @SimName WHERE sim_id = @SimId";

            using (var command = await CreateCommandAsync(query,
                new MySqlParameter("@SimName", sim.sim_name),
                new MySqlParameter("@SimId", sim.sim_id)))
            {
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<bool> DeleteSimAsync(int simId)
        {
            const string query = "DELETE FROM tbl_simcard WHERE sim_id = @SimId";

            using (var command = await CreateCommandAsync(query, new MySqlParameter("@SimId", simId)))
            {
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<IEnumerable<Sim>> SearchSimsByNameAsync(string simName)
        {
            const string query = "SELECT sim_id, sim_name FROM tbl_simcard WHERE sim_name LIKE @SimName";
            var sims = new List<Sim>();

            using (var command = await CreateCommandAsync(query, new MySqlParameter("@SimName", $"%{simName}%")))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    sims.Add(new Sim
                    {
                        sim_id = reader.GetInt32(0),
                        sim_name = reader.GetString(1)
                    });
                }
            }

            return sims;
        }
    }
}
