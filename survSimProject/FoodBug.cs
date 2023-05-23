using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class FoodBug:Food
    {
        public FoodBug(int _quantity) : base(_quantity)
        {
            heal = 0;
            hungerBonus = 5;
            cookable = true;
            healCooked = 5;
            hungerBonusCooked = 10;
        }
        public override string ToString()
        {
            return "Bug";
        }
        public override string Desc()
        {
            return "Edible, cookable, small creature found in jungle";
        }
    }
}
