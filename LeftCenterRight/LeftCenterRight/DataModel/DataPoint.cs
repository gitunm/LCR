using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeftCenterRight.DataModel
{
    public class DataPoint
    {
        public DataPoint(double games, double turns)
        {
            Games = games;
            Turns = turns;
        }

        public double Games { get; set; }
        public double Turns { get; set; }
    }
}
