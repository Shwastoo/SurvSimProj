using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace survSimProject
{
    class TopicAllianceTarget:Topic
    {
        public Alliance alliance;
        public TopicAllianceTarget(List<Selectable> _plrs) :base(_plrs)
        {
        }
        public override void Selected(Game gameRef)
        {
            // pobranie targetow od pozostalych czlonkow sojuszu oraz ustalenie finalnie kto bedzie celem sojuszu

            List<Player> allTargets = new List<Player> { };
            int offset = 0;
            foreach (Player p in players)
            {
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + offset);
                if (p == gameRef.playerCharacter)
                {
                    allTargets.Add(target);
                    Console.Write("You proposed to target " + target);
                }
                else
                {
                    Player pTarget = GetBiggestTarget(p, gameRef.players);
                    allTargets.Add(pTarget);
                    if(players.Contains(gameRef.playerCharacter)) Console.Write(p + " proposed to target " + pTarget);
                }
                offset++;
            }
            Dictionary<Player, int> targetCounter = new Dictionary<Player, int> { };
            foreach(Player t in allTargets)
            {
                if (targetCounter.ContainsKey(t))
                {
                    targetCounter[t]=targetCounter[t]+1;
                }
                else
                {
                    targetCounter.Add(t, 1);
                }
            }
            List<Player> tied = new List<Player> { };
            int number = 0;
            foreach(KeyValuePair<Player, int> kvp in targetCounter)
            {
                if(kvp.Value > number)
                {
                    number = kvp.Value;
                    tied.Clear();
                    tied.Add(kvp.Key);
                }
                else if(kvp.Value == number)
                {
                    tied.Add(kvp.Key);
                }
            }
            Player finalTarget = tied[Misc.rng.Next(0, tied.Count)];
            alliance.target = finalTarget;
            alliance.targetId = finalTarget.id;

            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + offset);
            if (players.Contains(gameRef.playerCharacter))
            {
                Console.Write("Your alliance ultimately decided to target " + finalTarget);
                Console.ReadKey(true);
                UI.CleanSelections();
                gameRef.actionDone = true;
            }


        }
        private Player GetBiggestTarget(Player p2, List<Selectable> allPlayers)
        {
            List<int> pIDs = new List<int> { };
            foreach (Player p in players) pIDs.Add(p.id);
            Player currentTarget = null;
            double currentTargetPoints = 0;
            foreach (PlayerKnowledge tk in p2.knowledge)
            {
                if (!p2.tribe.members.Contains(allPlayers[tk.id])) continue;
                double targetPoints = 0;
                Player p = (Player)allPlayers[tk.id];
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
                //if (tk.id == p2.id || tk.id == gameRef.playerCharacter.id) continue;
                if (pIDs.Contains(tk.id)) continue; 
                string relationshipWT;
                string relationshipWP2;
                bool ok = true;
                if (targetPoints > currentTargetPoints)
                {
                    foreach(int id in pIDs)
                    {
                        if (id == p2.id) continue;
                        else
                        {
                            relationshipWT = p2.knowledge[id].relationships[tk.id];
                            relationshipWP2 = p2.relationships[id].ToString();
                            if ((relationshipWT.Contains("?") || Convert.ToDouble(relationshipWT) < Convert.ToDouble(relationshipWP2)) && tk.alliances.Count == 0)
                            {
                                continue;
                            }
                            else
                            {
                                ok = false;
                                break;
                            }
                        }
                    
                    }
                    if (ok)
                    {
                        currentTargetPoints = targetPoints;
                        currentTarget = (Player)allPlayers[tk.id];
                    }
                }
            }

            if (currentTarget == null)
            {
                int id = p2.id;
                while (pIDs.Contains(id) || !p2.tribe.members.Contains(allPlayers[id]))
                {
                    id = Misc.rng.Next(0, allPlayers.Count);
                }
                currentTarget = (Player)allPlayers[id];
            }

            return currentTarget;
        }
        public override string ToString()
        {
            return "Gather your alliance";
        }
    }
}
