using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class SceneSelectMultiPlayer : SceneSelectMulti
    {
        private Sector sector;
        public SceneSelectMultiPlayer(Sector _sector, Game _game, Scene _sceneRef) : base(_game, _sceneRef)
        {
            sector = _sector;
            header = "Who do you want to talk to?";
            selectionList = sector.plrsOnSector;
        }
        public override void Enter()
        {
            List<Selectable> plrs = new List<Selectable> { };
            foreach (int i in selected) plrs.Add(selectionList[i]);
            if (plrs.Count != 0)
            {
                //RelChange(plrs);
                gameRef.scene = new SceneSelectTopic(gameRef, this, plrs);
            }
        }
    }
}
