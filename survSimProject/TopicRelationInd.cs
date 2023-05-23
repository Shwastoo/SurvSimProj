using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class TopicRelationInd : Topic
    {
        public TopicRelationInd(List<Selectable> _plrs) : base(_plrs)
        {
        }
        public override void Selected(Game gameRef)
        {
            Player player1 = (Player)players[1];
            Player player2 = (Player)players[0];
            int targetID = target.id;
            double relationshipT = player2.relationships[targetID];
            double relationshipP = player2.relationships[0];
            double relAnswer = relationshipT;
            bool lieCheck = true;
            //player asking npcA about npcB
            if (relationshipT > relationshipP)
            {
                //if A has better relationship with B than with us, they may lie
                int diceRoll = Misc.rng.Next(1, 11);
                double result = diceRoll + player2.lying;
                if (result < 11)
                {
                    //no lie, setting lieCheck to true but it behaves like telling the truth anyways
                    lieCheck = true;
                }
                else if (result < 15)
                {
                    //small lie
                    lieCheck = Lie(0, player2);
                    if (relAnswer <= 0) relAnswer += 3;
                    else relAnswer -= 3;
                }
                else if (result < 18)
                {
                    //medium lie
                    lieCheck = Lie(3, player2);
                    if (relAnswer <= 0) relAnswer += 6;
                    else relAnswer -= 6;
                }
                else if (result <= 20)
                {
                    //big lie
                    lieCheck = Lie(6, player2);
                    if (relAnswer <= 0) relAnswer += 9;
                    else relAnswer -= 9;
                }
                else
                {
                    Console.WriteLine("Error 0_____0");
                }

            }
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            if (relAnswer < -7)
            {
                if (player1 == gameRef.playerCharacter) Console.Write(player2 + " says that he hates " + target);
                relAnswer = -9;
            }
            else if (relAnswer >= -7 && relAnswer < -4)
            {
                if (player1 == gameRef.playerCharacter) Console.Write(player2 + " says that he strongly dislikes " + target);
                relAnswer = -6;
            }
            else if (relAnswer >= -4 && relAnswer < -1)
            {
                if (player1 == gameRef.playerCharacter) Console.Write(player2 + " says that he slightly dislikes " + target);
                relAnswer = -3;
            }
            else if (relAnswer >= -1 && relAnswer <= 1)
            {
                if (player1 == gameRef.playerCharacter) Console.Write(player2 + " says that he feels neutral about " + target);
                relAnswer = 0;
            }
            else if (relAnswer > 1 && relAnswer <= 4)
            {
                if (player1 == gameRef.playerCharacter) Console.Write(player2 + " says that he slightly likes " + target);
                relAnswer = 3;
            }
            else if (relAnswer > 4 && relAnswer <= 7)
            {
                if (player1 == gameRef.playerCharacter) Console.Write(player2 + " says that he strongly likes " + target);
                relAnswer = 6;
            }
            else if (relAnswer > 7)
            {
                if (player1 == gameRef.playerCharacter) Console.Write(player2 + " says that he adores " + target);
                relAnswer = 9;
            }
            if (!lieCheck)
            {
                //if lie unsuccessful, we will see that
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 1);
                if (player1 == gameRef.playerCharacter) Console.Write("You feel like his answer was not genuine...");
                player1.knowledge[player2.id].relationships[target.id] = relAnswer.ToString() + "?";
                player1.knowledge[player2.id].susLevel *= 1.5;
            }
            else
            {
                //if lie successful, no indicator of lying
                player1.knowledge[player2.id].relationships[target.id] = relAnswer.ToString();
            }
            if (player1 == gameRef.playerCharacter)
            {
                Console.ReadKey(true);
            }
        }
        private bool Lie(int diffMod, Player liar)
        {
            int diceRoll = Misc.rng.Next(1, 9);
            double result = diceRoll + diffMod;
            if(result <= liar.lying) return true;
            else return false;
        } 
        public override string ToString()
        {
            return "Talk about another player...";
        }
    }
}
