using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class AdvSafetyWithoutPower:Advantage
    {
        public AdvSafetyWithoutPower(int _quantity) : base(_quantity)
        {

        }
        public override string ToString()
        {
            return "Safety Without Power Advantage";
        }
        public override string Desc()
        {
            return "If played, leave the tribal council, you are safe, but you don't vote";
        }
    }
}
