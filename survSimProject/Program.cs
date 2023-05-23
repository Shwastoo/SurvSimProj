using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using AVS.CoreLib.PowerConsole;
using AVS.CoreLib.PowerConsole.Utilities;

namespace survSimProject
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.WindowWidth = 130;
                    Console.WindowHeight = 100;
                    Console.BufferWidth = 130;
                    Console.BufferHeight = 100;
                }
                catch
                {
                    PowerConsole.SetFont("Consolas",10);
                    Console.WindowWidth = 80;
                    Console.WindowHeight = 50;
                    Console.BufferWidth = 130;
                    Console.BufferHeight = 100;
                }
                Console.CursorVisible = false;
                //PowerConsole.SetFont("Terminal",8);
                //ConsoleHelper.SetCurrentFontTerminal();
                //FontInfo what = PowerConsole.GetFont();
                UI.DrawWindow();
                bool saving = false;
                bool start = false;
                while (!start)
                {
                    Console.SetCursorPosition((WinProps.windowRight - 1) / 2 - 15, (WinProps.windowBot - 1) / 2 - 2);
                    Console.Write("Survivor Simulator (ALPHA BUILD)");
                    Console.SetCursorPosition((WinProps.windowRight - 1) / 2 - 10, (WinProps.windowBot - 1) / 2);
                    Console.Write("Press ENTER to start");
                    Console.SetCursorPosition((WinProps.windowRight - 1) / 2 - 8, (WinProps.windowBot - 1) / 2 + 2);
                    Console.Write("Press Q to quit");
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.Enter:
                            start = true;
                            break;
                        case ConsoleKey.Q:
                            System.Environment.Exit(0);
                            break;
                        default:
                            break;
                    }
                }
                Console.Clear();
                UI.DrawWindow();
                if (!saving)
                {
                    Game game = new Game();
                    game.Start();

                }
                else
                {
                    Game game;
                    try
                    {
                        string save = File.ReadAllText(@".\..\..\..\save.json");
                        game = JsonConvert.DeserializeObject<Game>(save);
                        foreach (Tribe t in game.tribes)
                        {
                            game.islands.Add(t.island);
                        }
                        foreach (Alliance a in game.alliances)
                        {
                            a.target = (Player)game.players[a.targetId];
                            foreach (Player p in a.members)
                            {
                                p.alliances.Add(a);
                            }
                        }
                        foreach (Player p in game.players)
                        {
                            p.tribe = game.tribes.Find(x => x.id == p.tribeId);
                            foreach (PlayerKnowledge pk in p.knowledge)
                            {
                                pk.biggestTarget = (Player)game.players[pk.biggestTargetId];
                                foreach (int aId in pk.alliancesId)
                                {
                                    pk.alliances.Add((Alliance)game.alliances[aId]);
                                }
                            }
                        }
                        game.Start();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        Console.ReadKey(true);
                        game = new Game();
                        game.Start();
                    }
                }
                
                //foreach (Player p in game.players) Console.WriteLine(p);
                Console.Clear();
                UI.DrawWindow();
                Console.SetCursorPosition((WinProps.windowRight - 1) / 2 - 3, (WinProps.windowBot - 1) / 2);
                Console.Write("THE END");
                Console.ReadKey(true);
            }
        }

    }
}
