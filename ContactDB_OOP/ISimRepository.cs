using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactDB_OOP
{
    public interface ISimRepository
    {
        Task<IEnumerable<Sim>> GetAllSimsAsync();
        Task<Sim> GetSimByIdAsync(int simId);
        Task<bool> AddSimAsync(string simName);
        Task<bool> UpdateSimAsync(Sim sim);
        Task<bool> DeleteSimAsync(int simId);
        Task<IEnumerable<Sim>> SearchSimsByNameAsync(string simName);
    }
}
