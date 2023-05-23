using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace survSimProject
{
    class SceneSelectAction:SceneSelect
    {
        private Sector sector;
        public SceneSelectAction(Sector _sector, Game _game, Scene _sceneRef) : base(_game, _sceneRef)
        {
            sector = _sector;
            header = "What do you want to do?";
            selectionList = sector.possibleActions;
        }
        public override void Enter()
        {
            if (selectionList[Selection] is ActionTalkPrivately)
            {
                gameRef.scene = new SceneSelectPlayer(sector, gameRef, this);
            }
            else if(selectionList[Selection] is ActionTalkToGroup)
            {
                gameRef.scene = new SceneSelectMultiPlayer(sector, gameRef, this);
            }
            else if (selectionList[Selection] is ActionTalkToAll)
            {
                gameRef.scene = new SceneSelectTopic(gameRef, this, sector.plrsOnSector);
            }
            else if(selectionList[Selection] is ActionMakeFire)
            {
                UI.CleanSelections();
                ActionMakeFire a = (ActionMakeFire)selectionList[Selection];
                //a.gameRef = gameRef;
                a.player = gameRef.playerCharacter;
                a.Selected(gameRef);
            }
            else if (selectionList[Selection] is ActionEatFood)
            {
                UI.CleanSelections();
                ActionEatFood a = (ActionEatFood)selectionList[Selection];
                SectorCamp s = (SectorCamp)sector;
                List<Selectable> food = new List<Selectable> { };
                foreach (Selectable i in s.stash) if (i is Food) food.Add(i);
                gameRef.scene = new SceneSelectFood(gameRef, this, food, a);
            }
            else if(selectionList[Selection] is ActionCookFood)
            {
                UI.CleanSelections();
                ActionCookFood a = (ActionCookFood)selectionList[Selection];
                SectorCamp s = (SectorCamp)sector;
                List<Selectable> food = new List<Selectable> { };
                foreach (Selectable i in s.stash) if (i is Food) food.Add(i);
                gameRef.scene = new SceneSelectFoodCook(gameRef, this, food, a);
            }
            else if(selectionList[Selection] is ActionFishing)
            {
                UI.CleanSelections();
                ActionFishing a = (ActionFishing)selectionList[Selection];
                //a.gameRef = gameRef;
                a.player = gameRef.playerCharacter;
                a.Selected(gameRef);
            }
            else if (selectionList[Selection] is ActionLookForAdv)
            {
                UI.CleanSelections();
                ActionLookForAdv a = (ActionLookForAdv)selectionList[Selection];
                //a.gameRef = gameRef;
                a.player = gameRef.playerCharacter;
                a.Selected(gameRef);
            }
            else if(selectionList[Selection] is ActionGatherWood)
            {
                UI.CleanSelections();
                ActionGatherWood a = (ActionGatherWood)selectionList[Selection];
                //a.gameRef = gameRef;
                a.player = gameRef.playerCharacter;
                a.Selected(gameRef);
            }
            else if (selectionList[Selection] is ActionLookForFood)
            {
                UI.CleanSelections();
                ActionLookForFood a = (ActionLookForFood)selectionList[Selection];
                //a.gameRef = gameRef;
                a.player = gameRef.playerCharacter;
                a.Selected(gameRef);
            }
            else if (selectionList[Selection] is ActionTendFire)
            {
                UI.CleanSelections();
                ActionTendFire a = (ActionTendFire)selectionList[Selection];
                //a.gameRef = gameRef;
                a.player = gameRef.playerCharacter;
                a.Selected(gameRef);
            }
            else if (selectionList[Selection] is ActionViewStash)
            {
                SectorCamp camp = (SectorCamp)gameRef.playerCharacter.tribe.island.sectorList[gameRef.playerCharacter.tribe.island.campSectorID];
                gameRef.scene = new SceneSelectStashItems(gameRef, this, camp.stash);
            }
        }
        public override void Escape()
        {
            UI.CleanSelections();
            gameRef.scene = sceneRef;
        }
    }
}
