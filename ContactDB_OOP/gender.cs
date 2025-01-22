using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactDB_OOP
{
    public class Gender
    {
        // Private fields
        private int _genderId;
        private string _genderName;

        // Public properties
        public int gender_id { get => _genderId; set => _genderId = value; }
        public string gender_name { get => _genderName; set => _genderName = value; }
    }
}
