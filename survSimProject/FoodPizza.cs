using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class FoodPizza:Food
    {
        public FoodPizza(int _quantity) : base(_quantity)
        {
            heal = 10;
            hungerBonus = 50;
            cookable = false;
        }
        public override string ToString()
        {
            return "Pizza";
        }
        public override string Desc()
        {
            return "Delicious pizza...";
        }
    }
}
