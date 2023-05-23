using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class TopicWarn : Topic
    {
        public TopicWarn(List<Selectable> _plrs) : base(_plrs)
        {
        }
        public override void Selected(Game gameRef)
        {
            Player player1 = (Player)players[1];
            Player player2 = (Player)players[0];
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            if(player1 == gameRef.playerCharacter) Console.Write("You told " + player2 + " that he is being targetted by " + target);
            if(player2 == gameRef.playerCharacter) Console.Write(player1 + " told you that you are being targetted by " + target);
            double diceRoll = Misc.rng.Next(0, 20) + player1.lying - player2.relationships[target.id]; //in case of a lie
            if (player1.knowledge[target.id].biggestTarget == player2 || diceRoll>=15)
            {
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 1);
                if (player1 == gameRef.playerCharacter) Console.Write("He believed you");
                if (player2 == gameRef.playerCharacter) Console.Write("He seemed genuine about it");
                if (player2.knowledge[target.id].pointsForTargetting == 0)
                {
                    player2.knowledge[target.id].pointsForTargetting += 30;
                    double relChange = Misc.rng.Next(-200, -50);
                    relChange /= 100;
                    double relLevel = player2.relationships[target.id];
                    relLevel += relChange;
                    if (relLevel > 10) relLevel = 10;
                    else if (relLevel < -10) relLevel = -10;
                    player2.relationships[target.id] = Math.Round(relLevel, 2);  // (-2 - -0.5)
                    target.relationships[player2.id] = Math.Round(relLevel, 2);
                }
                else
                {
                    Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 2);
                    if (player1 == gameRef.playerCharacter) Console.Write("Also, he already heard about it from someone");
                    if (player2 == gameRef.playerCharacter) Console.Write("However, you already heard about that");
                }
            }
            else
            {
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 1);
                if (player1 == gameRef.playerCharacter) Console.Write("He didn't believe and called you out publicly with " + target);
                if (player2 == gameRef.playerCharacter) Console.Write("You didn't believe and called them out publicly with " + target);
                if (target == gameRef.playerCharacter) Console.Write("You heard that " + player1 + " said that you are targetting " + player2 + ", so you called him out");
                player2.knowledge[player1.id].susLevel *= 1.5 * 1.5;
                target.knowledge[player1.id].susLevel *= 1.5 * 1.5;
                double relChange = Misc.rng.Next(-300, -100);
                relChange /= 100;
                double relLevel = player2.relationships[player1.id];
                relLevel += relChange;
                if (relLevel > 10) relLevel = 10;
                else if (relLevel < -10) relLevel = -10;
                player2.relationships[player1.id] = Math.Round(relLevel, 2); // (-2 - -0.5)
                player1.relationships[player2.id] = Math.Round(relLevel, 2);
                relChange = Misc.rng.Next(-300, -100);
                relChange /= 100;
                relLevel = target.relationships[player1.id];
                relLevel += relChange;
                if (relLevel > 10) relLevel = 10;
                else if (relLevel < -10) relLevel = -10;
                target.relationships[player1.id] = Math.Round(relLevel, 2); // (-2 - -0.5)
                player1.relationships[target.id] = Math.Round(relLevel, 2);
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 2);
                if (player1 == gameRef.playerCharacter) Console.Write("Your relationship with both of them suffered deeply");
                if (player2 == gameRef.playerCharacter || target == gameRef.playerCharacter) Console.Write("Theirs relationship with both of you suffered deeply");
            }
            if (player1 == gameRef.playerCharacter)
            {
                Console.ReadKey(true);
                UI.CleanSelections();
                gameRef.actionDone = true;
            }
        }
        public override string ToString()
        {
            return "Warn about being targeted";
        }
    }
}
