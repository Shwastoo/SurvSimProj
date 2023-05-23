using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class SceneSelectFinalist : SceneSelect
    {
        public SceneSelectFinalist(Game _game, Scene _sceneRef, List<Selectable> _players) : base(_game, _sceneRef)
        {
            selectionList = _players;
            header = "Who do you want to take to the final three?";
        }
        public override void Enter()
        {
            Player plr = (Player)selectionList[Selection];
            gameRef.playerCharacter.vote = plr;
            gameRef.actionDone = true;

        }
    } 
}
