using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactDB_OOP
{
    public interface IContactRepository
    {
        Task<IEnumerable<Contact>> GetAllContactsAsync();
        Task<Contact> GetContactByIdAsync(int userId);
        Task<bool> AddContactAsync(string name, DateTime bday, int genderId, int cityId, int positionId, int simId, int companyId, string contactImage = null);
        Task<bool> UpdateContactAsync(Contact contact);
        Task<bool> DeleteContactAsync(int userId);
        Task<IEnumerable<Contact>> SearchContactsByUserNameAsync(string userName);
    }
}
