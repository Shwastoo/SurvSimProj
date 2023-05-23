using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class AdvLegacy:Advantage
    {
        public AdvLegacy(int _quantity) : base(_quantity)
        {

        }
        public override string ToString()
        {
            return "Legacy Advantage";
        }
        public override string Desc()
        {
            return "If played, any votes cast against you will not count, if you get voted out, give this to another player";
        }
    }
}
