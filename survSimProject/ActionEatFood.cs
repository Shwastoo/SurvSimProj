using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class ActionEatFood:Action
    {
        public Food food;
        public Player player;
        private SectorCamp sector;
        public ActionEatFood(SectorCamp _sector)
        {
            sector = _sector;
        }
        public override void Selected(Game gameRef)
        {
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            if (player == gameRef.playerCharacter) Console.Write("You ate a " + food);
            player.Health += food.heal;
            player.Hunger += food.hungerBonus;
            player.starving = false;
            player.hoursSinceEating = 0;
            if (food.quantity > 1) food.quantity--;
            else sector.stash.Remove(food);
            if (player == gameRef.playerCharacter)
            {
                Console.ReadKey(true);
                UI.CleanSelections();
                gameRef.actionDone = true;
            }
        }
        public override string ToString()
        {
            return "Eat food from stash";
        }
    }
}
