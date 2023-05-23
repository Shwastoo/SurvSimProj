using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class SceneSelectRelationLie:SceneSelect
    {
        public SceneSelectRelationLie(Game _game, Scene _sceneRef, Player _player2, Player _target):base(_game, _sceneRef)
        {
            header = "How do you want to answer?";
            selectionList = new List<Selectable> { };
            selectionList.Add(new ActionLie("Tell the truth", -1, true, _player2, _target));
            if(gameRef.playerCharacter.lying>=1) selectionList.Add(new ActionLie("Lie slightly", 0, true, _player2, _target));
            else selectionList.Add(new ActionLie("Lie slightly (REQUIRED LYING SKILL: 1)", 0, false, _player2, _target));
            if (gameRef.playerCharacter.lying >= 5) selectionList.Add(new ActionLie("Lie moderately", 3, true, _player2, _target));
            else selectionList.Add(new ActionLie("Lie moderately (REQUIRED LYING SKILL: 5)", 3, false,  _player2, _target));
            if (gameRef.playerCharacter.lying >= 8) selectionList.Add(new ActionLie("Lie strongly", 6, true, _player2, _target));
            else selectionList.Add(new ActionLie("Lie strongly (REQUIRED LYING SKILL: 8)", 6, false, _player2, _target));
        }
        public override void Enter()
        {
            ActionLie action = (ActionLie)selectionList[Selection];

            if (action.selectable)
            {
                UI.CleanSelections();
                action.Selected(gameRef);
                UI.CleanSelections();
                gameRef.actionDone = true;
            }
        }
        public override void Escape()
        {

        }


    }
}
