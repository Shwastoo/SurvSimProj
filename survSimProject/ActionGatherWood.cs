using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace survSimProject
{
    class ActionGatherWood:Action
    {
        private Sector sector;
        private SectorCamp camp;
        public Player player;
        public ActionGatherWood(Sector _sector)
        {
            sector = _sector;
        }
        public override void Selected(Game gameRef)
        {
            camp = (SectorCamp)player.tribe.island.sectorList[player.tribe.island.campSectorID];
            int woodFound;
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            if(sector is SectorShore)
            {
                woodFound = Misc.rng.Next(0, 3);
            }
            else
            {
                woodFound = Misc.rng.Next(1, 6);
            }
            if (woodFound > 0)
            {
                if(player == gameRef.playerCharacter)Console.Write("You brought Wood x" + woodFound);
                if (camp.stash.OfType<ItemWood>().Any()) camp.stash.OfType<ItemWood>().First().quantity += woodFound;
                else camp.stash.Add(new ItemWood(woodFound));
            }
            else if (player == gameRef.playerCharacter) Console.Write("You didn't bring any Wood");
            if(sector.advantageHidden!=null && Misc.rng.Next(0,20) == 0)
            {
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop+1);
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
            return "Gather wood";
        }
        
    }
}
