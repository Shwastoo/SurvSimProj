using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class Sector
    {
        public int id;
        public int xPos;
        public int yPos;
        public string colorMarker;
        public List<Selectable> plrsOnSector;
        public List<Selectable> possibleActions;
        public Advantage advantageHidden = null;
        public Sector(int _id, int _xPos, int _yPos)
        {
            id = _id;
            xPos = _xPos;
            yPos = _yPos;
            plrsOnSector = new List<Selectable> { };
            possibleActions = new List<Selectable> { };
        }

        public virtual void ResetActions()
        {
            possibleActions.Clear();
            possibleActions.Add(new ActionLookForAdv(this));
            possibleActions.Add(new ActionGatherWood(this));
            possibleActions.Add(new ActionLookForFood(this));
        }
        public virtual string Coords()
        {
            return "SECTOR ID: " + id + " X: " + xPos + " Y: " + yPos;
        }

        public virtual string Type()
        {
            return "Sector";
        }
    }
}
