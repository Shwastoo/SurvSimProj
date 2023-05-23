using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace survSimProject
{
    class ActionTendFire:Action
    {
        private SectorCamp camp;
        public Player player;
        public ActionTendFire(SectorCamp _camp)
        {
            camp = _camp;
        }
        public override void Selected(Game gameRef)
        {
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            int woodNeeded = Convert.ToInt32(Math.Floor(Convert.ToDecimal((100 - camp.CampfireHealth) / 10)));
            if (woodNeeded == 0)
            {
                if (player == gameRef.playerCharacter) Console.Write("No need to heal the fire");
            }
            else if (camp.stash.OfType<ItemWood>().Any())
            {
                if (camp.stash.OfType<ItemWood>().First().quantity > woodNeeded)
                {
                    camp.CampfireHealth += woodNeeded * 10;
                    camp.stash.OfType<ItemWood>().First().quantity -= woodNeeded;
                    if (player == gameRef.playerCharacter) Console.Write("You fully healed the fire using Wood x" + woodNeeded);
                }
                else
                {
                    camp.CampfireHealth += camp.stash.OfType<ItemWood>().First().quantity * 10;
                    if (player == gameRef.playerCharacter) Console.Write("You healed the fire using Wood x" + camp.stash.OfType<ItemWood>().First().quantity);
                    camp.stash.Remove(camp.stash.OfType<ItemWood>().First());
                    Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop+1);
                    if (player == gameRef.playerCharacter) Console.Write("There is no wood left in the stash");
                }
                if (player == gameRef.playerCharacter) gameRef.actionDone = true;
            }
            else
            {
                if (player == gameRef.playerCharacter) Console.Write("There is no wood in the stash");
            }
            if (player == gameRef.playerCharacter)
            {
                Console.ReadKey(true);
                UI.CleanSelections();
            }

        }

        public override string ToString()
        {
            return "Tend the fire (Health: " + camp.CampfireHealth + ")";
        }
    }
}
