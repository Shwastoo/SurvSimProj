using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;

namespace survSimProject
{
    class ChallengeTugOfWar:Challenge
    {
        public ChallengeTugOfWar():base()
        {
            name = "Tug Of War";
            desc = "A team that wins tug of war against the other wins, it takes 5 meters to win.";
            twoTeams = true;
            
        }

        public override void PlayChallenge(List<Tribe> tribes)
        {
            UI.CleanSelections();
            Intro();
            List<Player> bench = Bench("end", tribes);
            double tribeAStr = 0;
            double tribeBStr = 0;
            foreach(Player p in tribes[0].members)
            {
                if (!bench.Contains(p))
                {
                    tribeAStr += p.endurance;
                }
            }
            foreach(Player p in tribes[1].members)
            {
                if (!bench.Contains(p))
                {
                    tribeBStr += p.endurance;
                }
            }
            
            int points = 0;
            int time = 500;
            Tribe winners = null;
            while (Math.Abs(points) < 5)
            {
                UI.CleanSelections();
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                int chance = Misc.rng.Next(0, (int)(tribeAStr + tribeBStr));
                if (chance < tribeAStr) points--;
                else points++;
                if (points == -5)
                {
                    Console.Write(tribes[0] + " wins " + (forImmunity ? "immunity!" : "reward!"));
                    winners = tribes[0];
                }
                else if (points == 5) 
                { 
                    Console.Write(tribes[1] + " wins " + (forImmunity ? "immunity!" : "reward!"));
                    winners = tribes[1];
                }
                else if (points < 0) Console.Write(tribes[0] + " is in the lead by " + Math.Abs(points) + " meters");
                else if (points > 0) Console.Write(tribes[1] + " is in the lead by " + Math.Abs(points) + " meters");
                else Console.Write("Both teams are neck and neck");
                Thread.Sleep(time);
                while (Console.KeyAvailable)
                    Console.ReadKey(true);
                if (time>100) time -= 25;
            }
            Console.ReadKey(true);
            if (forImmunity) winners.immune = true;
            else winners.island.camp.stash.Add(reward);

            foreach (Player p in winners.members) p.compWins++;
        }
        public override string ToString()
        {
            return name;
        }
    }
}
