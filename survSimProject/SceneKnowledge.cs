using System;
using System.Collections.Generic;
using System.Text;
using Pastel;
using System.Drawing;

namespace survSimProject
{
    class SceneKnowledge :Scene
    {
        private int playerID;
        private Scene sceneRef;
        public int PlayerID
        {
            get { return playerID; }
            set
            {
                if (value < 0) playerID = gameRef.players.Count - 1;
                else if (value > gameRef.players.Count - 1) playerID = 0;
                else playerID = value;
            }
        }
        public SceneKnowledge(Game _game, Scene _sceneRef):base(_game)
        {
            sceneRef = _sceneRef;
            playerID = 0;
        }
        public override void Display()
        {
            Player p = (Player)gameRef.players[PlayerID];
            CleanKnowledge();
            if (gameRef.testingMode)
            {
                Console.SetCursorPosition(WinProps.knowledgeLeft, WinProps.knowledgeTop);
                Console.Write(p.ToString());
                Console.SetCursorPosition(WinProps.knowledgeLeft, WinProps.knowledgeTop + 2);
                Console.Write("Endurance:\t " + p.endurance.ToString());
                Console.SetCursorPosition(WinProps.knowledgeLeft, WinProps.knowledgeTop + 3);
                Console.Write("Mental:\t " + p.mental.ToString());
                Console.SetCursorPosition(WinProps.knowledgeLeft, WinProps.knowledgeTop + 4);
                Console.Write("Charisma:\t " + p.charisma.ToString());
                Console.SetCursorPosition(WinProps.knowledgeLeft, WinProps.knowledgeTop + 5);
                Console.Write("Lying:\t " + p.lying.ToString());
                Console.SetCursorPosition(WinProps.knowledgeLeft, WinProps.knowledgeTop + 7);
                Console.Write("Relationships:");
                int offset = 0;
                for (int id = 0; id < gameRef.players.Count; id++)
                {
                    Console.SetCursorPosition(WinProps.knowledgeLeft, WinProps.knowledgeTop + 9 + offset);
                    if (id != p.id)
                    {
                        Console.Write(gameRef.players[id].ToString());
                        Console.SetCursorPosition(WinProps.knowledgeLeft + 30, WinProps.knowledgeTop + 9 + offset);
                        for (int j = -10; j <= 10; j++)
                        {
                            if (j == 0) Console.Write("|");
                            else if (p.relationships[id] < 0 && p.relationships[id] <= j && j < 0) Console.Write("=");
                            else if (p.relationships[id] > 0 && p.relationships[id] >= j && j > 0) Console.Write("=");
                            else Console.Write(" ");
                        }
                        Console.Write(p.relationships[id].ToString());
                        offset++;
                    }
                }
            }
            else
            {
                Console.SetCursorPosition(WinProps.knowledgeLeft, WinProps.knowledgeTop);
                Console.Write(p);
                Console.SetCursorPosition(WinProps.knowledgeLeft, WinProps.knowledgeTop + 2);
                Console.Write("Relationships:");
                PlayerKnowledge pk;
                int offset = 0;
                for (int id = 0; id < gameRef.players.Count; id++)
                {
                    if (id != p.id)
                    {
                        double relValue;
                        string relText;
                        bool lie = false;
                        if (p.id == 0 || id == 0)
                        {
                            relValue = p.relationships[id];
                            relText = p.relationships[id].ToString();
                        }
                        else
                        {
                            pk = gameRef.playerCharacter.knowledge[p.id];
                            relText = pk.relationships[id];
                            if (relText == "?") relValue = 0;
                            else if (relText.EndsWith("?"))
                            {
                                lie = true;
                                relValue = Convert.ToDouble(relText.Replace("?", ""));
                            }
                            else relValue = Convert.ToDouble(relText);
                        }
                        Player player = (Player)gameRef.players[id];
                        Console.SetCursorPosition(WinProps.knowledgeLeft, WinProps.knowledgeTop + 5 + offset);
                        Console.Write(player);
                        Console.SetCursorPosition(WinProps.knowledgeLeft + 30, WinProps.knowledgeTop + 5 + offset);
                        for (int j = -10; j <= 10; j++)
                        {
                            string m;
                            string l;
                            if (!lie)
                            {
                                m = "|";
                                l = "=";
                            }
                            else
                            {
                                m = "/";
                                l = "~";
                            }
                            if (j == 0) Console.Write(m.Pastel(player.tribe.color));
                            else if (relValue < 0 && relValue <= j && j < 0) Console.Write(l.Pastel(player.tribe.color));
                            else if (relValue > 0 && relValue >= j && j > 0) Console.Write(l.Pastel(player.tribe.color));
                            else Console.Write(" ");
                        }
                        Console.Write(relText.Pastel(player.tribe.color));
                        offset++;
                    }
                }
                pk = gameRef.playerCharacter.knowledge[playerID];
                Console.SetCursorPosition(WinProps.knowledgeLeft + 57, WinProps.knowledgeTop);
                Console.Write("Mistrust: " + pk.susLevel);
                Console.SetCursorPosition(WinProps.knowledgeLeft + 57, WinProps.knowledgeTop + 1);
                Console.Write("Challenge wins: " + p.compWins);
                Console.SetCursorPosition(WinProps.knowledgeLeft + 57, WinProps.knowledgeTop + 2);
                Console.Write("Correct votes: " + p.correctVotes);
                Console.SetCursorPosition(WinProps.knowledgeLeft + 57, WinProps.knowledgeTop + 3);
                Console.Write("Advantage saves: " + pk.advantageSaves);
                Console.SetCursorPosition(WinProps.knowledgeLeft + 57, WinProps.knowledgeTop + 4);
                if(pk.biggestTarget!=null)Console.Write("Biggest target: " + pk.biggestTarget);
                else Console.Write("Biggest target: unknown");
                Console.SetCursorPosition(WinProps.knowledgeLeft + 57, WinProps.knowledgeTop + 5);
                Console.Write("Common alliances: ");
                for (int i =0; i<pk.alliances.Count;i++)
                {
                    string targetText;
                    if (pk.alliances[i].target == null) targetText = "no target";
                    else targetText = pk.alliances[i].target.ToString();
                    Console.SetCursorPosition(WinProps.knowledgeLeft + 57, WinProps.knowledgeTop + 6 + i);
                    Console.Write(pk.alliances[i] + " => " + targetText);
                }
            }
            

        }
        private void CleanKnowledge()
        {
            // cleaner
            string cleaner = "";
            for (int i = 0; i < WinProps.knowledgeLength; i++) cleaner += " ";
            for (int i = 0; i < WinProps.knowledgeHeight + gameRef.players.Count-1; i++)
            {
                Console.SetCursorPosition(WinProps.knowledgeLeft, WinProps.knowledgeTop + i);
                Console.Write(cleaner);
            }

        }
        public override void ArrowLeft()
        {
            PlayerID--;
        }
        public override void ArrowRight()
        {
            PlayerID++;
        }
        public override void Tab()
        {
            CleanKnowledge();
            gameRef.scene = sceneRef;
        }
        public override void Escape()
        {
            CleanKnowledge();
            gameRef.scene = sceneRef;
        }
        
    }
}
