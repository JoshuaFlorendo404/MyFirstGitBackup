using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactDB_OOP
{
    public class Company
    {
        private int _companyId;
        private string _companyName;

        public int company_id { get => _companyId; set => _companyId = value; }
        public string company_name { get => _companyName; set => _companyName = value; }
    }
}
