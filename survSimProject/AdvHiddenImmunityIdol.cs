using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class AdvHiddenImmunityIdol:Advantage
    {
        public AdvHiddenImmunityIdol(int _quantity) : base(_quantity)
        {

        }
        public override string ToString()
        {
            return "Hidden Immunity Idol";
        }
        public override string Desc()
        {
            return "If played, any votes cast against you will not count";
        }
    }
}
