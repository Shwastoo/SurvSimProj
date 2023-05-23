using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    abstract class Food:Item
    {
        public int heal;
        public int hungerBonus;
        public bool cookable;
        public int healCooked;
        public int hungerBonusCooked;
        public Food(int _quantity) : base(_quantity)
        {

        }
    }
}
