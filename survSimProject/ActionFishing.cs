using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace survSimProject
{
    class ActionFishing : Action
    {
        private SectorShore sector;
        private SectorCamp camp;
        public Player player;
        public ActionFishing(SectorShore _sector)
        {
            sector = _sector;
        }
        public override void Selected(Game gameRef)
        {
            camp = (SectorCamp)player.tribe.island.sectorList[player.tribe.island.campSectorID];
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            if (Misc.rng.Next(1, 11) <= sector.fishChance)
            {
                if (Misc.rng.Next(0, 10) == 0)
                {
                    if (player == gameRef.playerCharacter) Console.Write("You caught a big fish!!!");
                    if (camp.stash.OfType<FoodBigFish>().Any()) camp.stash.OfType<FoodBigFish>().First().quantity++;
                    else camp.stash.Add(new FoodBigFish(1));
                }
                else
                {
                    if (player == gameRef.playerCharacter) Console.Write("You caught a fish!");
                    if (camp.stash.OfType<FoodFish>().Any()) camp.stash.OfType<FoodFish>().First().quantity++;
                    else camp.stash.Add(new FoodFish(1));
                }
            }
            else
            {
                if (player == gameRef.playerCharacter) Console.Write("Unfortunately you didn't catch anything");
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
            return "Go fishing";
        }
    }
}
