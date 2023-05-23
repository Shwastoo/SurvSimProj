using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    static class UI
    {
        public static void DrawWindow()
        {
            for (int i = WinProps.windowLeft; i <= WinProps.windowRight; i++)
            {
                Console.SetCursorPosition(i, WinProps.windowTop);
                Console.Write(Tiles.WindowHor);
                Console.SetCursorPosition(i, WinProps.windowBot);
                Console.Write(Tiles.WindowHor);
            }
            for (int i = WinProps.windowTop; i <= WinProps.windowBot; i++)
            {
                Console.SetCursorPosition(WinProps.windowLeft, i);
                Console.Write(Tiles.WindowVer);
                Console.SetCursorPosition(WinProps.windowRight, i);
                Console.Write(Tiles.WindowVer);
            }
            Console.SetCursorPosition(WinProps.windowLeft, WinProps.windowTop);
            Console.Write(Tiles.WindowULCorner);
            Console.SetCursorPosition(WinProps.windowRight, WinProps.windowTop);
            Console.Write(Tiles.WindowURCorner);
            Console.SetCursorPosition(WinProps.windowLeft, WinProps.windowBot);
            Console.Write(Tiles.WindowDLCorner);
            Console.SetCursorPosition(WinProps.windowRight, WinProps.windowBot);
            Console.Write(Tiles.WindowDRCorner);
        }
        public static void CleanSelections()
        {
            // cleaner
            string cleaner = "";
            for (int i = 0; i < WinProps.selLength + 2; i++) cleaner += " ";
            for (int i = 0; i < GlobSettings.maxActions; i++)
            {
                Console.SetCursorPosition(WinProps.selectionsLeft - 2, WinProps.selectionsTop + i);
                Console.Write(cleaner);
            }

        }
        public static void KeyPressHandling(ConsoleKeyInfo key, Scene scene)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    scene.ArrowUp();
                    break;
                case ConsoleKey.DownArrow:
                    scene.ArrowDown();
                    break;
                case ConsoleKey.LeftArrow:
                    scene.ArrowLeft();
                    break;
                case ConsoleKey.RightArrow:
                    scene.ArrowRight();
                    break;
                case ConsoleKey.Enter:
                    scene.Enter();
                    break;
                case ConsoleKey.Escape:
                    scene.Escape();
                    break;
                case ConsoleKey.Spacebar:
                    scene.Space();
                    break;
                case ConsoleKey.Tab:
                    scene.Tab();
                    break;
                case ConsoleKey.I:
                    scene.IKey();
                    break;
                case ConsoleKey.F2:
                    if(scene is SceneMap)
                    {
                        SceneMap sc = (SceneMap)scene;
                        //sc.F2();
                    }
                    break;

            }
            while (Console.KeyAvailable)
                Console.ReadKey(true);
        }
    }
}
