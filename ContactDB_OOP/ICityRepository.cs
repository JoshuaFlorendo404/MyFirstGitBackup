using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactDB_OOP
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetAllCitiesAsync();
        Task<City> GetCityByIdAsync(int cityId);
        Task<bool> AddCityAsync(string cityName);
        Task<bool> UpdateCityAsync(City city);
        Task<bool> DeleteCityAsync(int cityId);
        Task<IEnumerable<City>> SearchCitiesByNameAsync(string cityName);
    }
}
