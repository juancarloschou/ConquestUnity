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
        public string territoryID;
        public int resourceValue;
        public int defendersStrength;
        public List<Vector2> territoryBoundary;

        public Territory(string id, int resource, int strength, List<Vector2> boundary)
        {
            territoryID = id;
            resourceValue = resource;
            defendersStrength = strength;
            territoryBoundary = boundary;
        }
    }

}

