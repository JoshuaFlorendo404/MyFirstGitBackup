using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactDB_OOP
{
    public class PositionRepository : BaseRepository, IPositionRepository
    {
        public PositionRepository(MyCon dbConnection) : base(dbConnection) { }

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

        public async Task<IEnumerable<Position>> GetAllPositionsAsync()
        {
            const string query = "SELECT position_id, position_name FROM tbl_position";
            var positions = new List<Position>();

            using (var command = await CreateCommandAsync(query))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    positions.Add(new Position
                    {
                        position_id = reader.GetInt32(0),
                        position_name = reader.GetString(1)
                    });
                }
            }

            return positions;
        }

        public async Task<Position> GetPositionByIdAsync(int positionId)
        {
            const string query = "SELECT position_id, position_name FROM tbl_position WHERE position_id = @PositionId";

            using (var command = await CreateCommandAsync(query, new MySqlParameter("@PositionId", positionId)))
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new Position
                    {
                        position_id = reader.GetInt32(0),
                        position_name = reader.GetString(1)
                    };
                }
            }

            return null;
        }

        public async Task<bool> AddPositionAsync(string positionName)
        {
            const string query = "INSERT INTO tbl_position (position_name) VALUES (@PositionName)";

            using (var command = await CreateCommandAsync(query, new MySqlParameter("@PositionName", positionName)))
            {
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<bool> UpdatePositionAsync(Position position)
        {
            const string query = "UPDATE tbl_position SET position_name = @PositionName WHERE position_id = @PositionId";

            using (var command = await CreateCommandAsync(query,
                new MySqlParameter("@PositionName", position.position_name),
                new MySqlParameter("@PositionId", position.position_id)))
            {
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<bool> DeletePositionAsync(int positionId)
        {
            const string query = "DELETE FROM tbl_position WHERE position_id = @PositionId";

            using (var command = await CreateCommandAsync(query, new MySqlParameter("@PositionId", positionId)))
            {
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<IEnumerable<Position>> SearchPositionsByNameAsync(string positionName)
        {
            const string query = "SELECT position_id, position_name FROM tbl_position WHERE position_name LIKE @PositionName";
            var positions = new List<Position>();

            using (var command = await CreateCommandAsync(query, new MySqlParameter("@PositionName", $"%{positionName}%")))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    positions.Add(new Position
                    {
                        position_id = reader.GetInt32(0),
                        position_name = reader.GetString(1)
                    });
                }
            }

            return positions;
        }
    }
}
