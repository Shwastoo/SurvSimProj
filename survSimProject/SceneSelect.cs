using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    abstract class SceneSelect : Scene
    {
        protected Scene sceneRef;
        private int selection;
        protected string header;
        protected List<Selectable> selectionList;
        public int Selection
        {
            get { return selection; }
            set
            {
                if (value < 0) selection = selectionList.Count - 1;
                else if (value > selectionList.Count - 1) selection = 0;
                else selection = value;
            }
        }

        public SceneSelect(Game _game, Scene _sceneRef):base(_game)
        {
            sceneRef = _sceneRef;
            selection = 0;
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
            }

        }
        public override void ArrowUp()
        {
            Selection--;
        }
        public override void ArrowDown()
        {
            Selection++;
        }
        public override void Tab()
        {
            UI.CleanSelections();
            gameRef.scene = new SceneKnowledge(gameRef, this);
        }
        public override void Escape()
        {
            UI.CleanSelections();
            gameRef.scene = sceneRef;
        }
    }
}
