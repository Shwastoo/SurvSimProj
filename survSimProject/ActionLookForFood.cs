using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace survSimProject
{
    class ActionLookForFood:Action
    {
        private Sector sector;
        private SectorCamp camp;
        public Player player;
        public ActionLookForFood(Sector _sector)
        {
            sector = _sector;
        }
        public override void Selected(Game gameRef)
        {
            camp = (SectorCamp)player.tribe.island.sectorList[player.tribe.island.campSectorID];
            int diceRoll = Misc.rng.Next(0, 20);
            int foodFound = Misc.rng.Next(1,3);
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            if (diceRoll < 9)
            {
                if (player == gameRef.playerCharacter) Console.Write("You found Coconut x" + foodFound);
                if (camp.stash.OfType<FoodCoconut>().Any()) camp.stash.OfType<FoodCoconut>().First().quantity += foodFound;
                else camp.stash.Add(new FoodCoconut(foodFound));
            }
            else if (diceRoll < 16)
            {
                if (player == gameRef.playerCharacter) Console.Write("You found Papaya x" + foodFound);
                if (camp.stash.OfType<FoodPapaya>().Any()) camp.stash.OfType<FoodPapaya>().First().quantity += foodFound;
                else camp.stash.Add(new FoodPapaya(foodFound));
            }
            else
            {
                if (player == gameRef.playerCharacter) Console.Write("You found Bug x" + foodFound);
                if (camp.stash.OfType<FoodBug>().Any()) camp.stash.OfType<FoodBug>().First().quantity += foodFound;
                else camp.stash.Add(new FoodBug(foodFound));
            }
            if (sector.advantageHidden != null && Misc.rng.Next(0, 20) == 0)
            {
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 1);
                if (player == gameRef.playerCharacter) Console.Write("You got lucky and accidentally found the " + sector.advantageHidden + "!");
                player.inventory.Add(sector.advantageHidden);
                sector.advantageHidden = null;
            }
            if (player == gameRef.playerCharacter)
            {
                Console.ReadKey(true);
                UI.CleanSelections();
                gameRef.actionDone = true;
            }
        }
        public override string ToString()
        {
            return "Look for food";
        }

    }
}
