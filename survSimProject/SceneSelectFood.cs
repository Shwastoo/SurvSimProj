using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class SceneSelectFood:SceneSelect
    {
        private ActionEatFood action;
        public SceneSelectFood(Game _gameRef, Scene _sceneRef, List<Selectable> _food, ActionEatFood _action):base(_gameRef, _sceneRef)
        {
            selectionList = _food;
            action = _action;
            header = "What do you want to eat?";
        }
        public override void Display()
        {
            action.food = (Food)selectionList[Selection];
            base.Display();
            Console.SetCursorPosition(WinProps.selectionsLeft + 50, WinProps.selectionsTop);
            Console.Write(action.food + " x" + action.food.quantity);
            Console.SetCursorPosition(WinProps.selectionsLeft + 50, WinProps.selectionsTop + 1);
            Console.Write("Health: +" + action.food.heal);
            Console.SetCursorPosition(WinProps.selectionsLeft + 50, WinProps.selectionsTop + 2);
            Console.Write("Hunger: +" + action.food.hungerBonus);
        }
        public override void Enter()
        {
            UI.CleanSelections();
            action.food = (Food)selectionList[Selection];
            action.player = gameRef.playerCharacter;
            action.Selected(gameRef);

        }
        public override void Escape()
        {
            
        }
    }
}
