using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class TopicYourTarget:Topic
    {
        public TopicYourTarget(List<Selectable> _plrs) : base(_plrs)
        {
        }
        public override void Selected(Game gameRef)
        {
            Player p2 = (Player)players[0];
            int higherTLvlCount = 0;
            
            foreach(PlayerKnowledge pk in p2.knowledge)
            {
                if (pk.id == gameRef.playerCharacter.id || pk.id == p2.id) continue;
                if(pk.targetPoints > p2.knowledge[target.id].targetPoints)
                {
                    higherTLvlCount++;
                }
            }

            int diceRoll = Misc.rng.Next(1, 7);
            if (diceRoll + gameRef.playerCharacter.lying / 3 < higherTLvlCount)
            {
                p2.knowledge[gameRef.playerCharacter.id].susLevel *= 1.5;
            }
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            Console.Write("You told " + p2 + " that your biggest target is " + target);
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop+1);
            Console.Write("You hope that he believed you...");
            p2.knowledge[gameRef.playerCharacter.id].biggestTarget = target;
            p2.knowledge[gameRef.playerCharacter.id].biggestTargetId = target.id;
            Console.ReadKey(true);
            UI.CleanSelections();
            gameRef.actionDone = true;
        }
    }
}
