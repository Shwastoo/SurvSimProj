using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class FoodPapaya:Food
    {
        public FoodPapaya(int _quantity) : base(_quantity)
        {
            heal = 10;
            hungerBonus = 5;
            cookable = false;
            healCooked = 0;
            hungerBonusCooked = 0;
        }
        public override string ToString()
        {
            return "Papaya";
        }
        public override string Desc()
        {
            return "Edible, sweet and tasty, but doesn't help much";
        }
    }
}
