using System;
using System.Collections.Generic;
using System.Text;
using Pastel;

namespace survSimProject
{
    class SceneMap:Scene
    {
        private int xPos;
        private int yPos;
        private int selSector;
        private Island island;
        private Sector highlighted;
        private Tribe tribe;
        public int SelSector
        {
            get { return selSector; }
            set
            {
                if (value < GlobSettings.sectorCount && value >= 0)
                {
                    selSector = value;
                    highlighted = island.sectorList[selSector];
                }
            }
        }
        public int XPos
        {
            get { return xPos; }
            set {
                if (value < GlobSettings.islandSize && value >= 0)
                {
                    xPos = value;
                    //highlighted = island.sectorList[xPos + GlobSettings.islandSize*YPos];
                    SelSector = XPos + GlobSettings.islandSize * YPos;
                } 
            }
        }
        public int YPos
        {
            get { return yPos; }
            set 
            {
                if (value < GlobSettings.islandSize && value >= 0)
                { 
                    yPos = value;
                    //highlighted = island.sectorList[xPos + GlobSettings.islandSize * YPos];
                    SelSector = XPos + GlobSettings.islandSize * YPos;
                }
            }
        }
        public SceneMap(Island _island, Game _game):base(_game)
        {
            island = _island;
            tribe = gameRef.tribes[island.id];
            SelSector = 0;
            Clear();
        }

        private void Clear()
        {
            foreach (Sector sec in island.sectorList)
            {
                for (int i = 0; i < WinProps.sectorWidth; i++)
                {
                    for (int j = 0; j < WinProps.sectorWidth; j++)
                    {
                        if (i == 0 || i == WinProps.sectorWidth - 1 || j == 0 || j == WinProps.sectorWidth - 1)
                        {
                        }
                        else if (i == WinProps.sectorWidth - 2 && j == WinProps.sectorWidth - 2 && sec.Type() != "Sector")
                        {
                        }
                        else
                        {
                            Console.SetCursorPosition(WinProps.mapLeft + sec.xPos * WinProps.sectorWidth + j, WinProps.mapTop + sec.yPos * WinProps.sectorWidth + i);
                            Console.Write(" ");
                        }
                    }
                }
            }
        }
        public override void Display()
        {
            //Draw map
            foreach (Sector sec in island.sectorList)
            {
                for (int i = 0; i < WinProps.sectorWidth; i++)
                {
                    for (int j = 0; j < WinProps.sectorWidth; j++)
                    {
                        if (i == 0 || i == WinProps.sectorWidth - 1 || j == 0 || j == WinProps.sectorWidth - 1)
                        {
                            Console.SetCursorPosition(WinProps.mapLeft + sec.xPos * WinProps.sectorWidth + j, WinProps.mapTop + sec.yPos * WinProps.sectorWidth + i);
                            if (sec.xPos == XPos && sec.yPos == YPos) Console.Write(Tiles.SecBord.Pastel("#456789"));
                            else Console.Write(Tiles.SecBord.Pastel("#ABCDEF"));
                        }
                        else if (i == WinProps.sectorWidth - 2 && j == WinProps.sectorWidth - 2 && sec.Type() != "Sector")
                        {
                            Console.SetCursorPosition(WinProps.mapLeft + sec.xPos * WinProps.sectorWidth + j, WinProps.mapTop + sec.yPos * WinProps.sectorWidth + i);
                            Console.Write(Tiles.SecMarker.Pastel(sec.colorMarker));
                        }
                        else if ((i-1) * GlobSettings.islandSize + j <= sec.plrsOnSector.Count)
                        {
                            Console.SetCursorPosition(WinProps.mapLeft + sec.xPos * WinProps.sectorWidth + j, WinProps.mapTop + sec.yPos * WinProps.sectorWidth + i);
                            Console.Write(Tiles.Player.Pastel("#FF0000"));
                        }
                    }
                }
            }
            Console.SetCursorPosition(WinProps.mapLeft, WinProps.mapTop - 2);
            Console.Write(tribe + ", Day " + gameRef.day + ", Time: " + gameRef.time + ":00    ");
            Console.SetCursorPosition(WinProps.mapInfoLeft, WinProps.mapTop - 2);
            Console.Write("Player's stats: Health: " + gameRef.playerCharacter.Health + " Hunger: " + gameRef.playerCharacter.Hunger + " Starving? " + (gameRef.playerCharacter.starving ? "yes" : "no") + "      ");
            //Console.Write("\n");
            SideInfo();
        }
        private void SideInfo()
        {
            //clearing
            string cleaner = "";
            for (int i = 0; i < WinProps.infoLength; i++) cleaner += " ";
            for (int i = 0; i < WinProps.sectorWidth*GlobSettings.islandSize; i++)
            {
                Console.SetCursorPosition(WinProps.mapInfoLeft, WinProps.mapInfoTop+i);
                Console.Write(cleaner);
            }

            // writing id and coords
            Console.SetCursorPosition(WinProps.mapInfoLeft, WinProps.mapInfoTop);
            Console.Write(highlighted.Coords());
            // writing type
            Console.SetCursorPosition(WinProps.mapInfoLeft, WinProps.mapInfoTop + 1);
            Console.Write(highlighted.Type());
            // listing players
            Console.SetCursorPosition(WinProps.mapInfoLeft, WinProps.mapInfoTop + 2);
            Console.Write("Players on sector:");
            if (highlighted.plrsOnSector.Count == 0)
            {
                Console.SetCursorPosition(WinProps.mapInfoLeft, WinProps.mapInfoTop + 3);
                Console.Write("None");
            }
            for (int i = 0; i<highlighted.plrsOnSector.Count;i++)
            {
                Player p = (Player)highlighted.plrsOnSector[i];
                Console.SetCursorPosition(WinProps.mapInfoLeft, WinProps.mapInfoTop + 3 + i);
                Console.Write(p);
            }
        }

        
        public override void ArrowLeft()
        {
            XPos--;
        }
        public override void ArrowRight()
        {
            XPos++;
        }
        public override void ArrowUp()
        {
            YPos--;
        }
        public override void ArrowDown()
        {
            YPos++;
        }
        public override void Enter()
        {
            
            for (int i = 0; i < WinProps.sectorWidth; i++)
            {
                for (int j = 0; j < WinProps.sectorWidth; j++)
                {
                    if (i == 0 || i == WinProps.sectorWidth - 1 || j == 0 || j == WinProps.sectorWidth - 1)
                    {
                        Console.SetCursorPosition(WinProps.mapLeft + highlighted.xPos * WinProps.sectorWidth + j, WinProps.mapTop + highlighted.yPos * WinProps.sectorWidth + i);
                        if (highlighted.xPos == XPos && highlighted.yPos == YPos) Console.Write(Tiles.SecBord.Pastel("#00AA00"));
                    }
                }
            }
            gameRef.scene = new SceneSelectAction(highlighted, gameRef, this);
        }
        public override void Tab()
        {
            gameRef.scene = new SceneKnowledge(gameRef, this);
        }
        public override void IKey()
        {
            gameRef.scene = new SceneSelectInventory(gameRef, this, gameRef.playerCharacter.inventory);
        }

        public void F2()
        {
            if (island.id == 0)
            {
                island = gameRef.islands[1];
                tribe = gameRef.tribes[1];
            }
            else if (island.id == 1)
            {
                island = gameRef.islands[0];
                tribe = gameRef.tribes[0];
            }
            highlighted = island.sectorList[selSector];
            Clear();
        }
    }
}
