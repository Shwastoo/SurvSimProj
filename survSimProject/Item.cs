using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    abstract class Item:Selectable
    {
        public int quantity;
        public Item(int _quantity)
        {
            quantity = _quantity;
        }
        public abstract string Desc();
    }
}
