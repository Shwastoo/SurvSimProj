using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class AdvVoteSteal:Advantage
    {
        public AdvVoteSteal(int _quantity) : base(_quantity)
        {

        }
        public override string ToString()
        {
            return "Vote Steal Advantage";
        }
        public override string Desc()
        {
            return "If played, steal somebody's votes, you now vote two times";
        }
    }
}
