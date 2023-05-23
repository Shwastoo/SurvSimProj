using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class SceneSelectMulti : SceneSelect
    {
        protected List<int> selected;
        public SceneSelectMulti(Game _game, Scene _sceneRef) : base(_game, _sceneRef)
        {
            selected = new List<int> { };
        }
        public override void Display()
        {
            UI.CleanSelections();

            // write options
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            Console.Write(header);
            for (int i = 0; i < selectionList.Count; i++)
            {
                Selectable obj = selectionList[i];
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 1 + i);
                Console.Write(obj.ToString());
                Console.SetCursorPosition(WinProps.selectionsLeft - 2, WinProps.selectionsTop + 1 + i);
                if (i == Selection) Console.Write(">");
                else Console.Write(" ");
                if (selected.Contains(i)) Console.Write("+");
                else Console.Write(" ");
            }

        }
        public override void Space()
        {
            if (selected.Contains(Selection)) selected.Remove(Selection);
            else selected.Add(Selection);
        }
        public override void Enter()
        {

        }
    }
}
