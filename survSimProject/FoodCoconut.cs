using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class FoodCoconut:Food
    {
        public FoodCoconut(int _quantity) : base(_quantity)
        {
            heal = 2;
            hungerBonus = 5;
            cookable = false;
            healCooked = 0;
            hungerBonusCooked = 0;
        }
        public override string ToString()
        {
            return "Coconut";
        }
        public override string Desc()
        {
            return "Edible, tasty but doesn't help much";
        }
    }
}
