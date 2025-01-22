using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactDB_OOP
{
    public class City
    {
        private int _cityId;
        private string _cityName;

        public int city_id { get => _cityId; set => _cityId = value; }
        public string city_name { get => _cityName; set => _cityName = value; }
    }
}
