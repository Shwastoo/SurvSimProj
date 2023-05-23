using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;

namespace survSimProject
{
    class ChallengePuzzle:Challenge
    {
        public ChallengePuzzle():base()
        {
            name = "Giant Puzzle";
            desc = "First person to put a 25 piece puzzle together, wins immunity.";
            individual = true;
        }
        public override void PlayChallenge(List<Tribe> tribes)
        {
            UI.CleanSelections();
            Intro();
            List<int> pieces = new List<int> { };
            List<Player> players = new List<Player> { };
            List<int> ids = new List<int> { };
            List<Selectable> playersStillInGame = tribes.Last().members;
            foreach (Player p in playersStillInGame)
            {
                players.Add(p);
                pieces.Add(0);
            }
            int time = 10;
            UI.CleanSelections();
            int id = 0;
            for (int i = 0; i < players.Count; i++)
            {
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + i);
                Console.Write(players[i] + ": " + pieces[i] + " solved");
            }
            while (!pieces.Contains(25))
            {
                for (int i = 0; i < players.Count; i++) ids.Add(i);
                
                for(int i = 0; i < players.Count; i++)
                {
                    id = ids[Misc.rng.Next(0, ids.Count)];
                    int diceRoll = Misc.rng.Next(0, 25 - pieces[id]);
                    if (diceRoll < players[id].mental / 3) pieces[id]++;
                    Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop+id);
                    Console.Write(players[id] + ": " + pieces[id] + " solved");

                    Thread.Sleep(time); 
                    while (Console.KeyAvailable)
                        Console.ReadKey(true);
                    //if (time > 100) time -= 5;
                    if (pieces[id] == 25) break;
                }

            }
            Player winner = players[id];
            UI.CleanSelections(); 
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            Console.Write(winner + " wins " + (forImmunity ? "immunity!" : "reward!"));
            Console.ReadKey(true);
            if (forImmunity) winner.immune = true;
            else { }
            winner.compWins++;
        }
        public override string ToString()
        {
            return name;
        }
    }
}
