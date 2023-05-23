using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class SceneSelectPlayer : SceneSelect
    {
        private Sector sector;
        public SceneSelectPlayer(Sector _sector, Game _game, Scene _sceneRef) : base(_game, _sceneRef)
        {
            sector = _sector;
            header = "Who do you want to talk to?";
            selectionList = sector.plrsOnSector;
        }
        public override void Enter()
        {
            List<Selectable> plr = new List<Selectable> { selectionList[Selection] };
            gameRef.scene = new SceneSelectTopic(gameRef, this, plr);
        }
    }
}
