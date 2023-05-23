using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class TopicGeneral : Topic
    {
        private Player player1;
        public TopicGeneral(List<Selectable> _plrs, Player p1) : base(_plrs)
        {
            player1 = p1;
        }
        public override void Selected(Game gameRef)
        {
            players.Add(player1);
            int reductor = players.Count - 1;
            for (int i = 0; i<players.Count;i++)
            {
                for(int j = 0; j < i; j++)
                {
                    Player p1 = (Player)players[i];
                    Player p2 = (Player)players[j];

                    int avCharisma = Convert.ToInt32(Math.Round((p1.charisma + p2.charisma) / 2));

                    double relChange = Misc.rng.Next(-20+avCharisma*2, 101+avCharisma*10); //(-0.2 - 1.0)
                    relChange /= 100 * reductor;

                    double relLevel = p1.relationships[p2.id];
                    double rellevelCheck = p2.relationships[p1.id];
                    if (relLevel != rellevelCheck) Environment.Exit(2137);
                    relLevel += relChange;

                    if (relLevel > 10) relLevel = 10;
                    else if (relLevel < -10) relLevel = -10;
                    p1.relationships[p2.id] = Math.Round(relLevel,2);
                    p2.relationships[p1.id] = Math.Round(relLevel,2);
                }
            }
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            if (players.Contains(gameRef.playerCharacter))
            {
                Console.Write("You talked with them for a while");
                Console.ReadKey(true);
                UI.CleanSelections();
                gameRef.actionDone = true;
            }
        }
        public override string ToString()
        {
            return "Just talk";
        }
    }
}
