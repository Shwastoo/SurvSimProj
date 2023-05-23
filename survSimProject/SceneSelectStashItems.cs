using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class SceneSelectStashItems:SceneSelect
    {
        public SceneSelectStashItems(Game _gameRef, Scene _sceneRef, List<Selectable> _items) : base(_gameRef, _sceneRef)
        {
            selectionList = _items;
            header = "Stash:";
        }

        public override void Display()
        {
            base.Display();
            if (selectionList.Count > 0)
            {
                Item item = (Item)selectionList[Selection];
                Console.SetCursorPosition(WinProps.selectionsLeft + 30, WinProps.selectionsTop);
                Console.Write("Quantity: " + item.quantity);
                Console.SetCursorPosition(WinProps.selectionsLeft + 30, WinProps.selectionsTop + 1);
                Console.Write(item.Desc());
            }
            else
            {
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop+1);
                Console.Write("Nothing here");
                Console.ReadKey(true);
                UI.CleanSelections();
                gameRef.scene = sceneRef;
            }
        }

        public override void Enter()
        {
            
        }
    }
}
