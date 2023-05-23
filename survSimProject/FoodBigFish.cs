using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class FoodBigFish:Food
    {
        public FoodBigFish(int _quantity) : base(_quantity)
        {
            heal = 15;
            hungerBonus = 15;
            cookable = true;
            healCooked = 60;
            hungerBonusCooked = 60;
        }

        public override string ToString()
        {
            return "Big Fish";
        }

        public override string Desc()
        {
            return "Edible, cookable, more rare than a regular fish";
        }

    }
}
