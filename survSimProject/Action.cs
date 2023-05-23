using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    abstract class Action : Selectable
    {
        public abstract void Selected(Game gameRef);
    }
}
