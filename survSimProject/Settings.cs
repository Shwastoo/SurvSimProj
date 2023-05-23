using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    static class GlobSettings
    {
        public static int maxAbilityPoints = 20;
        public static int playerCount = 16;
        public static int islandSize = 5;
        public static int sectorCount = islandSize*islandSize;
        public static int maxActions = 20;
    }

    static class WinProps
    {
        public static int windowLeft = 0;
        public static int windowRight = 111;
        public static int windowTop = 0;
        public static int windowBot = 91;

        public static int sectorWidth = 7;
        public static int mapLeft = windowLeft + 5;
        public static int mapTop = windowTop + 5;

        public static int mapInfoLeft = mapLeft + sectorWidth*GlobSettings.islandSize + 3;
        public static int mapInfoTop = mapTop;

        public static int infoLength = 65;
        public static int selLength = 100;
        public static int knowledgeLength = 90;
        public static int knowledgeHeight = 10;

        public static int selectionsTop = sectorWidth * GlobSettings.islandSize + 5 + 5;
        public static int selectionsLeft = mapLeft;

        public static int knowledgeLeft = selectionsLeft;
        public static int knowledgeTop = selectionsTop;

        public static int inventoryLeft = mapInfoLeft;
        public static int inventoryTop = mapInfoTop;

    }

    static class Misc
    {
        public static Random rng = new Random();
        public static int color1 = rng.Next(20, 256);
        public static int color2 = rng.Next(20, 256);
        public static int color3 = rng.Next(20, 256);
    }

    static class Tiles
    {
        public static string SecBord = "█";
        public static string WindowULCorner = "╔";
        public static string WindowURCorner = "╗";
        public static string WindowDLCorner = "╚";
        public static string WindowDRCorner = "╝";
        public static string SecMarker = "o";
        public static string Player = "@";
        public static string WindowHor = "═";
        public static string WindowVer = "║";
    }
}
