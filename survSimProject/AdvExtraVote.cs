using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class AdvExtraVote:Advantage
    {
        public AdvExtraVote(int _quantity) : base(_quantity)
        {

        }
        public override string ToString()
        {
            return "Extra Vote";
        }
        public override string Desc()
        {
            return "If played, you will have an extra vote for one tribal council";
        }
    }
}
