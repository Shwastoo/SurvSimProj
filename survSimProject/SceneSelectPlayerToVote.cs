using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class SceneSelectPlayerToVote:SceneSelect
    {
        public SceneSelectPlayerToVote(Game _game, Scene _sceneRef, Tribe _tribe, List<Player> onlyEligible) : base(_game, _sceneRef)
        {
            header = "Who do you vote for?"; ;
            selectionList = new List<Selectable> { };
            if (onlyEligible == null)
            {
                foreach (Player p in _tribe.members)
                {
                    if (!p.immune && p != gameRef.playerCharacter) selectionList.Add(p);
                }
            }
            else
            {
                foreach (Player p in _tribe.members)
                {
                    if (onlyEligible.Contains(p)) selectionList.Add(p);
                }
            }
        }

        public override void Enter()
        {
            Player plr = (Player)selectionList[Selection];
            gameRef.playerCharacter.vote = plr;
            gameRef.actionDone = true;
            
        }
    }
}
