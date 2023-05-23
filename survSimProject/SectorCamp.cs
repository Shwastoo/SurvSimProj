using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class SectorCamp:Sector
    {
        public List<Selectable> stash = new List<Selectable>();
        private bool campfireLit = false;
        public bool CampfireLit
        {
            get { return campfireLit; }
            set
            {
                if (value) 
                {
                    campfireHealth = 100;
                    campfireLit = true;
                }
            }
        }
        private int campfireHealth = 0;
        public int CampfireHealth
        {
            get { return campfireHealth; }
            set
            {
                if (value >= 100) campfireHealth = 100;
                else if (value <= 0)
                {
                    campfireHealth = 0;
                    campfireLit = false;
                }
                else campfireHealth = value;
            }
        }

        public SectorCamp(int _id, int _xPos, int _yPos) : base(_id, _xPos, _yPos)
        {
            stash.Add(new FoodRiceBag(40));
            colorMarker = "#fca103";
        }
        public override void ResetActions()
        {
            possibleActions.Clear();
            possibleActions.Add(new ActionLookForAdv(this));
            possibleActions.Add(new ActionEatFood(this));
            if (campfireLit)
            {
                possibleActions.Add(new ActionCookFood(this));
                possibleActions.Add(new ActionTendFire(this));
            }
            else possibleActions.Add(new ActionMakeFire(this));
            possibleActions.Add(new ActionViewStash());
        }

        public override string Type()
        {
            return "Camp";
        }
    }
}
