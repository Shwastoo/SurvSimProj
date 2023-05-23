using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class ItemWood:Item
    {
        public ItemWood(int _quantity) : base(_quantity)
        {

        }
        public override string ToString()
        {
            return "Wood";
        }
        public override string Desc()
        {
            return "Used as a fuel for fire";
        }
    }
}
