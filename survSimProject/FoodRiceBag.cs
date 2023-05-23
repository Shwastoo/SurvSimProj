using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class FoodRiceBag:Food
    {
        public FoodRiceBag(int _quantity) : base(_quantity)
        {
            heal = 2;
            hungerBonus = 2;
            cookable = true;
            healCooked = 5;
            hungerBonusCooked = 5;
        }
        public override string ToString()
        {
            return "Rice Bag";
        }
        public override string Desc()
        {
            return "Edible, cookable, you won't find this in the jungle";
        }
    }
}
