using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    abstract class Topic : Action
    {
        public Player target;
        public List<Selectable> players;
        public Topic(List<Selectable> _plrs)
        {
            players = _plrs;
        }
    }
}
