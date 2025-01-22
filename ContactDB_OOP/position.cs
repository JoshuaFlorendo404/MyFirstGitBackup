using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactDB_OOP
{
    public class Position
    {
        private int _positionId;
        private string _positionName;

        public int position_id { get => _positionId; set => _positionId = value; }
        public string position_name { get => _positionName; set => _positionName = value; }
    }
}
