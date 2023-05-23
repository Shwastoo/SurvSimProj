using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class SceneSelectAlliance:SceneSelect
    {
        private Topic topic;
        public SceneSelectAlliance(Game _gameRef, Scene _sceneRef, List<Selectable> _alliances, Topic _topic):base(_gameRef, _sceneRef)
        {
            header = "Which alliance target do you want to discuss?";
            selectionList = _alliances;
            topic = _topic;
        }
        public override void Enter()
        {
            List<Selectable> plrsInConv = new List<Selectable> { };
            Alliance a = (Alliance)selectionList[Selection];
            //foreach (Player p in a.members) if(p!=gameRef.playerCharacter) plrsInConv.Add(p);
            foreach (Player p in a.members) plrsInConv.Add(p);
            TopicAllianceTarget t = (TopicAllianceTarget)topic;
            t.players = plrsInConv;
            t.alliance = a;
            gameRef.scene = new SceneSelectOtherPlayer(gameRef, this, plrsInConv, "Who do you want to target?", t);
        }
        public override void Escape()
        {

        }

    }
}
