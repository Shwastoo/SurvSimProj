using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class TopicCreateAlliance : Topic
    {
        private Player player1;
        public TopicCreateAlliance(List<Selectable> _plrs, Player p1) : base(_plrs)
        {
            player1 = p1;

        }
        public override void Selected(Game gameRef)
        {
            Alliance all = new Alliance(gameRef.alliances.Count);
            all.members.Add(player1);
            foreach (Player p in players) all.members.Add(p);
            if(all.members.Count == player1.tribe.members.Count)
            {
                if(player1 == gameRef.playerCharacter)
                {
                    Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                    Console.Write("You think that maybe you shouldn't create an alliance with all your tribe members...");
                    Console.ReadKey(true);
                    UI.CleanSelections();
                    gameRef.actionDone = true;
                }
                return;
            }

            foreach (Player p1 in all.members)
            {
                p1.alliances.Add(all);
                double loyaltySum = 0;
                foreach(Player p2 in all.members)
                {
                    if (p1 == p2) continue;
                    else
                    {
                        loyaltySum += p1.relationships[p2.id];
                        p1.knowledge[p2.id].alliances.Add(all);
                        p1.knowledge[p2.id].alliancesId.Add(all.id);
                    }
                }
                all.loyalty.Add(loyaltySum / (all.members.Count - 1));
            }
            gameRef.alliances.Add(all);
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            if(player1 == gameRef.playerCharacter)
            {
                Console.Write("You created " + all + " alliance!");
                Console.ReadKey(true);
                UI.CleanSelections();
                gameRef.actionDone = true;
            }
            else if (all.members.Contains(gameRef.playerCharacter))
            {
                Console.Write(player1 + " created and invited you to " + all + " alliance!");
                Console.ReadKey(true);
                UI.CleanSelections();
                gameRef.actionDone = true;
            }

        }
        public override string ToString()
        {
            return "Propose alliance";
        }
    }
}
