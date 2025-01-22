using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactDB_OOP
{
    public interface IGenderRepository
    {
        Task<IEnumerable<Gender>> GetAllGendersAsync();
        Task<Gender> GetGenderByIdAsync(int genderId);
        Task<bool> AddGenderAsync(string genderName);
        Task<bool> UpdateGenderAsync(Gender gender);
        Task<bool> DeleteGenderAsync(int genderId);
        Task<IEnumerable<Gender>> SearchGendersByNameAsync(string genderName);
    }
}
