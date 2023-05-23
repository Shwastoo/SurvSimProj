using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class Names
    {
        public List<string> playerNames { get; set; }
        public List<string> tribeNames { get; set; }

        public Names()
        {
            playerNames = new List<string> { };
            tribeNames = new List<string> { };
        }

    }
}
