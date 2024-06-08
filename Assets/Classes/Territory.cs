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
        public int Id; // identifier
        public int ResourceValue; // for future resources
        public int DefendersStrength; // for future troops
        public int Player; // player possesion (1, 2, 0)
        public int BasementPlayer; // starting basement of player (1, 2, 0)
        public List<Vector2> TerritoryBoundary; // for drawing the sides of territory

        public TerritoryController territoryController; // to access to territory game object
        public TroopController TroopController; // to find and update the troop of the territory if any

        public Territory(int id, int resourceValue, int defendersStrength, int player, int basementPlayer, List<Vector2> territoryBoundary)
        {
            Id = id;
            ResourceValue = resourceValue;
            DefendersStrength = defendersStrength;
            Player = player;
            BasementPlayer = basementPlayer;
            TerritoryBoundary = territoryBoundary;
        }
    }

}

