using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class SceneSelectTopic:SceneSelect
    {
        private List<Selectable> plrsInConv;
        private List<Selectable> commonAlliances;
        private bool exactAlliance = false;
        
        public SceneSelectTopic(Game _game, Scene _sceneRef, List<Selectable> _players) : base(_game, _sceneRef)
        {
            
            header = "What do you want to talk about?";
            plrsInConv = _players;
            commonAlliances = new List<Selectable> { };
            //get common alliances
            foreach (Alliance a in gameRef.playerCharacter.alliances)
            {
                bool common = true;
                foreach (Player p in plrsInConv)
                {
                    if (!p.alliances.Contains(a))
                    {
                        common = false;
                        break;
                    }
                }
                if (common)
                {
                    commonAlliances.Add(a);
                }
            }
            //check if any of the alliances consists of only this exact players
            foreach(Alliance a in commonAlliances)
            {
                if (a.members.Count == plrsInConv.Count + 1)
                {
                    exactAlliance = true; 
                    break;
                }
            }
            //create topic list based on above 
            selectionList = new List<Selectable> { new TopicGeneral(plrsInConv, gameRef.playerCharacter) };
            if (plrsInConv.Count == 1)
            {
                selectionList.Add(new TopicRelationInd(plrsInConv));
                if (commonAlliances.Count > 0) selectionList.Add(new TopicAllianceTarget(plrsInConv));
                selectionList.Add(new TopicTargetInd(plrsInConv));
                selectionList.Add(new TopicWarn(plrsInConv));
            }
            else if (commonAlliances.Count > 0) selectionList.Add(new TopicAllianceTarget(plrsInConv));
            if (!exactAlliance) selectionList.Add(new TopicCreateAlliance(plrsInConv, gameRef.playerCharacter));
        }

        public override void Enter()
        {
            Topic action = (Topic)selectionList[Selection];
            if (action is TopicRelationInd)
            {
                action.players.Add(gameRef.playerCharacter);
                gameRef.scene = new SceneSelectOtherPlayer(gameRef, this, plrsInConv, "Who do you want to talk about?", action);
            }
            else if (action is TopicTargetInd)
            {
                UI.CleanSelections();
                action.players.Add(gameRef.playerCharacter);
                action.Selected(gameRef);
                gameRef.scene = new SceneSelectOtherPlayer(gameRef, this, plrsInConv, "Who is your biggest target?", new TopicYourTarget(plrsInConv));
            }
            else if (action is TopicAllianceTarget)
            {
                gameRef.scene = new SceneSelectAlliance(gameRef, this, commonAlliances, action);
            }
            else if (action is TopicWarn)
            {
                action.players.Add(gameRef.playerCharacter);
                gameRef.scene = new SceneSelectOtherPlayer(gameRef, this, plrsInConv, "Who is targeting that person?", action);
            }
            else if(action is TopicGeneral)
            {
                UI.CleanSelections();
                action.Selected(gameRef);
            }
            else if(action is TopicCreateAlliance)
            {
                UI.CleanSelections();
                action.Selected(gameRef);
            }
        }
        public override void Escape()
        {
            
        }
    }
}
