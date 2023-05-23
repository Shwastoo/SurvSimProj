using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class TopicTargetInd :Topic
    {
        private bool sus = false;
        public TopicTargetInd(List<Selectable> _plrs) : base(_plrs)
        {
        }
        public override void Selected(Game gameRef)
        {
            Player player1 = (Player)players[1];
            Player player2 = (Player)players[0];
            Player targetP = GetBiggestTarget(player2, player1, gameRef.players);
            if (targetP == null) return;
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            if(player1 == gameRef.playerCharacter) Console.Write(player2 + " told you that his biggest target is " + targetP);
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 1);
            if (sus)
            {
                if (player1 == gameRef.playerCharacter) Console.Write("You feel like he is not telling the truth..."); 
                player1.knowledge[player2.id].susLevel *= 1.5;
            }
            player1.knowledge[player2.id].biggestTarget = targetP;
            player1.knowledge[player2.id].biggestTargetId = targetP.id;
            if (player1 == gameRef.playerCharacter)
            {
                Console.ReadKey(true);
            }

        }

        private Player GetBiggestTarget(Player p2, Player p1, List<Selectable> players)
        {
            Player currentTarget = null;
            double currentTargetPoints = 0;
            int targetsSkipped = 0;
            foreach(PlayerKnowledge tk in p2.knowledge)
            {
                if (!p2.tribe.members.Contains(players[tk.id])) continue;
                double targetPoints = 0;
                Player p = (Player)players[tk.id];
                foreach (string k in tk.relationships)
                {
                    if (!k.Contains("?")) targetPoints += Convert.ToInt32(k);
                }
                targetPoints += p.correctVotes * 5;
                targetPoints += p.compWins * 10;
                targetPoints += tk.advantageSaves * 15;
                targetPoints += tk.susLevel;
                targetPoints += tk.pointsForTargetting;

                tk.targetPoints = targetPoints;
                if (tk.id == p2.id || tk.id == p1.id) continue;
                if (targetPoints > currentTargetPoints)
                {
                    string relationshipWT = p2.knowledge[p1.id].relationships[tk.id] ;
                    //string relationshipWP2 = p2.knowledge[p1.id].relationships[p2.id];
                    string relationshipWP2 = p2.relationships[p1.id].ToString();
                    if((relationshipWT.Contains("?") || Convert.ToDouble(relationshipWT) < Convert.ToDouble(relationshipWP2))&&tk.alliances.Count==0)
                    {
                        currentTargetPoints = targetPoints;
                        currentTarget = (Player)players[tk.id];
                    }
                    else
                    {
                        targetsSkipped++;
                    }
                }
            }

            if (currentTarget == null) 
            {
                int loopGuard = 0;
                int id = p2.id;
                while (id == p2.id || id == p1.id || !p2.tribe.members.Contains(players[id]))
                {
                    id = Misc.rng.Next(0, players.Count);
                    loopGuard++;
                    if (loopGuard == 100) return null;
                }
                currentTarget = (Player)players[id];
            }
            int diceRoll = Misc.rng.Next(1, 7);
            if (diceRoll + p2.lying / 3 < targetsSkipped) sus = true;

            return currentTarget;
        }
        public override string ToString()
        {
            return "Ask about their target";
        }
    }
}
