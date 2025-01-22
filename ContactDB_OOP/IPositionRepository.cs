using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactDB_OOP
{
    public interface IPositionRepository
    {
        Task<IEnumerable<Position>> GetAllPositionsAsync();
        Task<Position> GetPositionByIdAsync(int positionId);
        Task<bool> AddPositionAsync(string positionName);
        Task<bool> UpdatePositionAsync(Position position);
        Task<bool> DeletePositionAsync(int positionId);
        Task<IEnumerable<Position>> SearchPositionsByNameAsync(string positionName);
    }
}
