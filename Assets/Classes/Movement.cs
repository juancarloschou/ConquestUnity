using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class Movement
    {
        public int Player { get; set; }
        public int TerritoryIdOrigin { get; set; }
        public int TerritoryIdDestination { get; set; }
        //public Vector2 location { get; set; }
        //public DateTime timeDestination { get; set; }
        //public int Knights { get; set; }
        public int DefendersStrength { get; set; }

        public TroopController TroopController { get; set; }
    }
}
