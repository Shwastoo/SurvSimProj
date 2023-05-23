using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class ActionLie: Action
    {
        private string optionText;
        private int diffMod;
        public bool selectable;
        private Player player2;
        private Player target;
        public ActionLie(string _text, int _diffMod, bool _selectable, Player _player2, Player _target)
        {
            optionText = _text;
            diffMod = _diffMod;
            selectable = _selectable;
            player2 = _player2;
            target = _target;
        }
        public override void Selected(Game gameRef)
        {
            double relAnswer = gameRef.playerCharacter.relationships[target.id];
            if (diffMod == 0)
            {
                if (relAnswer <= 0) relAnswer += 3;
                else relAnswer -= 3;
            }
            else if(diffMod == 3)
            {
                if (relAnswer <= 0) relAnswer += 6;
                else relAnswer -= 6;
            }
            else if(diffMod == 6)
            {
                if (relAnswer <= 0) relAnswer += 9;
                else relAnswer -= 9;
            }

            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            if (relAnswer >= -10 && relAnswer < -7)
            {
                Console.Write("You told " + player2 + " that you hate " + target);
                relAnswer = -9;
            }
            else if (relAnswer >= -7 && relAnswer < -4)
            {
                Console.Write("You told " + player2 + " that you strongly dislike " + target);
                relAnswer = -6;
            }
            else if (relAnswer >= -4 && relAnswer < -1)
            {
                Console.Write("You told " + player2 + " that you slightly dislike " + target);
                relAnswer = -3;
            }
            else if (relAnswer >= -1 && relAnswer <= 1)
            {
                Console.Write("You told " + player2 + " that you feel neutral about " + target);
                relAnswer = 0;
            }
            else if (relAnswer > 1 && relAnswer <= 4)
            {
                Console.Write("You told " + player2 + " that you slightly like " + target);
                relAnswer = 3;
            }
            else if (relAnswer > 4 && relAnswer <= 7)
            {
                Console.Write("You told " + player2 + " that you strongly like " + target);
                relAnswer = 6;
            }
            else if (relAnswer > 7 && relAnswer <= 10)
            {
                Console.Write("You told " + player2 + " that you adore " + target);
                relAnswer = 9;
            }
            player2.knowledge[gameRef.playerCharacter.id].relationships[target.id] = relAnswer.ToString();
            if (!Lie(gameRef.playerCharacter)) player2.knowledge[gameRef.playerCharacter.id].susLevel *= 1.5;
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop+1);
            if(diffMod!=-1) Console.Write("You wonder if he believed you...");
            Console.ReadKey(true);
        }
        private bool Lie(Player p)
        {
            int diceRoll = Misc.rng.Next(1, 9);
            double result = diceRoll + diffMod;
            if (result <= p.lying) return true;
            else return false;
        }
        public override string ToString()
        {
            return optionText;
        }
    }
}
