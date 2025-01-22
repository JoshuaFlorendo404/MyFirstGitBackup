using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactDB_OOP
{
    public class Sim
    {
        private int _simId;
        private string _simName;

        public int sim_id { get => _simId; set => _simId = value; }
        public string sim_name { get => _simName; set => _simName = value; }
    }
}
