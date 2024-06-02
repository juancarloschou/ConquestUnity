using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    [System.Serializable]
    public class Territory
    {
        public string Id; // identifier
        public int ResourceValue; // for future resources
        public int DefendersStrength; // for future troops
        public int Player; // player possesion (1, 2, 0)
        public int BasementPlayer; // starting basement of player (1, 2, 0)
        public List<Vector2> TerritoryBoundary; // for drawing the sides of territory

        public Territory(string id, int resource, int strength, List<Vector2> boundary)
        {
            Id = id;
            ResourceValue = resource;
            DefendersStrength = strength;
            TerritoryBoundary = boundary;
        }
    }

}

