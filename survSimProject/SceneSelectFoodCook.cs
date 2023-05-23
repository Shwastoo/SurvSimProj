using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class SceneSelectFoodCook:SceneSelect
    {
        private ActionCookFood action;
        public SceneSelectFoodCook(Game _gameRef, Scene _sceneRef, List<Selectable> _food, ActionCookFood _action) : base(_gameRef, _sceneRef)
        {
            selectionList = _food;
            action = _action;
            header = "What do you want to cook?";
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
            Console.SetCursorPosition(WinProps.selectionsLeft + 50, WinProps.selectionsTop + 4);
            if (action.food.cookable)
            {
                Console.Write("If cooked:");
                Console.SetCursorPosition(WinProps.selectionsLeft + 50, WinProps.selectionsTop + 5);
                Console.Write("Health: +" + action.food.healCooked);
                Console.SetCursorPosition(WinProps.selectionsLeft + 50, WinProps.selectionsTop + 6);
                Console.Write("Hunger: +" + action.food.hungerBonusCooked);
            }
            else
            {
                Console.Write("You can't cook it");
            }
        }
        public override void Enter()
        {
            UI.CleanSelections();
            //action.gameRef = gameRef;
            action.food = (Food)selectionList[Selection];
            action.player = gameRef.playerCharacter;
            if (action.food.cookable) action.Selected(gameRef);
        }
        public override void Escape()
        {

        }
    }
}
