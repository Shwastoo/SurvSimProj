using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class ActionLookForAdv:Action
    {
        private Sector sector;
        public Player player;
        public ActionLookForAdv(Sector _sector)
        {
            sector = _sector;
        }
        public override void Selected(Game gameRef)
        {
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            if (sector.advantageHidden != null && Misc.rng.Next(0, 15) < player.mental)
            {
                if (player == gameRef.playerCharacter) Console.Write("You have found the " + sector.advantageHidden + "!");
                player.inventory.Add(sector.advantageHidden);
                sector.advantageHidden = null;
            }
            else
            {
                if (player == gameRef.playerCharacter) Console.Write("You didn't find anything");
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
            return "Look for advantages";
        }
    }
}
