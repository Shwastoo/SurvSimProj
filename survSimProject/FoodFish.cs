using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class FoodFish:Food
    {
        public FoodFish(int _quantity) : base(_quantity)
        {
            heal = 5;
            hungerBonus = 5;
            cookable = true;
            healCooked = 20;
            hungerBonusCooked = 20;
        }
        public override string ToString()
        {
            return "Fish";
        }
        public override string Desc()
        {
            return "Edible, cookable, the most basic fish you can catch";
        }
    }
}
