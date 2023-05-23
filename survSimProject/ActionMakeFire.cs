using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class ActionMakeFire:Action
    {
        private SectorCamp sector;
        public Player player;
        public ActionMakeFire(SectorCamp _sector)
        {
            sector = _sector;
        }
        public override void Selected(Game gameRef)
        {
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            if (Misc.rng.Next(0, 30) < player.mental)
            {
                sector.CampfireLit = true;
                player.firesMade++;
                foreach(Player p in player.tribe.members)
                {
                    p.relationships[player.id] += 1;
                    player.relationships[p.id] += 1;
                }
                if (player == gameRef.playerCharacter) Console.Write("After several attempts you managed to make fire! Everyone appreciates it");
                else if(player.tribe == gameRef.playerCharacter.tribe)
                {
                    Console.Write(player + " managed to make fire! Everyone appreciates it");
                    Console.ReadKey(true);
                }
            }
            else
            {
                if (player == gameRef.playerCharacter) Console.Write("Even though you tried a lot, you couldn't make fire.");
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
            return "Try to make fire";
        }
    }
}
