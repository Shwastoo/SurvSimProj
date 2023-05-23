using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class SectorShore :Sector
    {
        public int fishChance;
        public SectorShore(int _id, int _xPos, int _yPos) : base(_id, _xPos, _yPos)
        {
            fishChance = Misc.rng.Next(1, 11);
            colorMarker = "#00d9ff";
        }
        public override void ResetActions()
        {
            possibleActions.Clear();
            possibleActions.Add(new ActionLookForAdv(this));
            possibleActions.Add(new ActionGatherWood(this));
            possibleActions.Add(new ActionFishing(this));
        }
        public override string Type()
        {
            return "Shore";
        }
    }
}
