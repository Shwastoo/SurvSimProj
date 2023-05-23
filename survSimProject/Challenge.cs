using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace survSimProject
{
    abstract class Challenge
    {
        public string name;
        public string desc;
        public bool forImmunity;
        public Item reward;

        public bool individual = false;
        public bool twoTeams = false;
        public bool threeTeams = false;
        public bool fourTeams = false;

        protected void Intro()
        {
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            Console.Write("Come on in guys! Time for " + (forImmunity ? "an Immunity Challenge." : "a Reward Challenge."));
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop+1);
            Console.Write("This challenge is called " + name);
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 2);
            Console.Write(desc);
            if(reward != null)
            {
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 3);
                Console.Write("You wanna know what you're playing for? The reward is " + reward);
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 4);
                Console.Write("Survivors ready?");
            }
            else
            {
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 3);
                Console.Write("Survivors ready?");
            }
            Console.ReadKey(true);
        }

        protected List<Player> Bench(string mode, List<Tribe> tribes)
        {
            List<Player> bench = new List<Player> { };
            int playersMax = tribes.Max(x => x.members.Count);
            int playersPlaying = tribes.Min(x => x.members.Count);
            if (playersMax != playersPlaying)
            {
                foreach(Tribe t in tribes)
                {
                    int playersOnBench = Math.Abs(t.members.Count-playersPlaying);

                    for (int i = 0; i < playersOnBench; i++)
                    {
                        Player weakest = null;
                        foreach(Player p in t.members)
                        {
                            if (weakest == null)
                            {
                                weakest = p;
                                continue;
                            }
                            if (bench.Contains(p)) continue;
                            if (mode == "end") 
                            {
                                if (weakest != null && weakest.endurance > p.endurance)
                                {
                                    weakest = p;
                                }
                            }
                            else if(mode == "men")
                            {
                                if (weakest != null && weakest.mental > p.mental)
                                {
                                    weakest = p;
                                }
                            }
                            else
                            {
                                if (weakest != null && weakest.mental+weakest.endurance > p.mental+p.endurance)
                                {
                                    weakest = p;
                                }
                            }
                        }
                        bench.Add(weakest);
                    }
                }
            }
            return bench;
        }
        abstract public void PlayChallenge(List<Tribe> tribes);
    }
}
