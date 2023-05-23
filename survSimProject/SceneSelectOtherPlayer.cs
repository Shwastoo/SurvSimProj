using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class SceneSelectOtherPlayer:SceneSelect
    {
        private List<Selectable> plrsInConv;
        private Topic topic;
        public SceneSelectOtherPlayer(Game _game, Scene _sceneRef, List<Selectable> _players, string _header, Topic _topic) : base(_game, _sceneRef)
        {
            header = _header;
            plrsInConv = _players;
            selectionList = new List<Selectable> { };
            foreach (Player p in gameRef.PlayersStillInGame) if (!plrsInConv.Contains(p) && p!=gameRef.playerCharacter && gameRef.playerCharacter.tribe.members.Contains(p)) selectionList.Add(p);
            topic = _topic;
        }
        public override void Enter()
        {
            Player p = (Player)selectionList[Selection];
            //SceneSelect ss = (SceneSelect)sceneRef;
            topic.target = p;
            UI.CleanSelections();
            topic.Selected(gameRef);
            if(topic is TopicRelationInd)
            {
                gameRef.scene = new SceneSelectRelationLie(gameRef, this, (Player)topic.players[0], topic.target);
            }
        }
        public override void Escape()
        {

        }
    }
}
