using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactDB_OOP
{
    public class Contact
    {
        // Private fields (Encapsulation)
        private int _userId;
        private string _userName;
        private DateTime _bday;
        private int _cityId;
        private int _companyId;
        private int _positionId;
        private int _genderId;
        private int _simId;
        private string _contactImage;

        // Public properties matching database field names exactly
        public int user_id { get => _userId; set => _userId = value; }
        public string user_name { get => _userName; set => _userName = value; }
        public DateTime bday { get => _bday; set => _bday = value; }
        public int city_id { get => _cityId; set => _cityId = value; }
        public int company_id { get => _companyId; set => _companyId = value; }
        public int position_id { get => _positionId; set => _positionId = value; }
        public int gender_id { get => _genderId; set => _genderId = value; }
        public int sim_id { get => _simId; set => _simId = value; }
        public string contact_image { get => _contactImage; set => _contactImage = value; }
    }
}
