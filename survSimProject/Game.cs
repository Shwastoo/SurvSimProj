using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Linq;
using Newtonsoft.Json;

namespace survSimProject
{
    class Game
    {
        public bool testingMode = false;

        public int startingTribes = 2;
        public List<Tribe> tribes = new List<Tribe> { };

        [JsonIgnore]
        public List<Island> islands = new List<Island> { };

        public List<Selectable> players = new List<Selectable> { };
        public List<Selectable> PlayersStillInGame = new List<Selectable> { };
        public List<Selectable> PlayersInJury = new List<Selectable> { };
        public List<Selectable> PlayersInPreJury = new List<Selectable> { };

        public Scene scene;
        public List<Selectable> alliances = new List<Selectable> { };
        public Player playerCharacter;

        public int day = 1;
        public int time = 8;
        public bool actionDone = false;
        public bool tribal = false;

        public bool endgame = false;

        public int swapDay = 5;
        public int mergeDay = 7;

        public bool merged = false;
        public bool jury = false;
        public int playersEliminated = 0;

        public Names names;

        public void Start()
        {
            if (tribes.Count == 0)
            {
                LoadNamesFromFile();

                CreatePlayers();
                CreateTribes();
            }

            while (!endgame)
            {
                StartRound();
                day++;
            }
        }
        private void LoadNamesFromFile()
        {
            try
            {
                string jsonFile = File.ReadAllText(@".\..\..\..\Names.json");
                names = System.Text.Json.JsonSerializer.Deserialize<Names>(jsonFile);
            }
            catch
            {
                Console.WriteLine("OJEJ");
                Console.ReadKey();
            }
            
        }
        private void CreatePlayers()
        {
            List<Selectable> plrs = new List<Selectable> { };
            for (int i = 0; i < GlobSettings.playerCount; i++)
            {
                string name = names.playerNames[Misc.rng.Next(0, names.playerNames.Count)];
                names.playerNames.Remove(name);
                if (i == 0) name += " (YOU)";
                plrs.Add(new Player(i,name));
                PlayersStillInGame.Add(plrs[i]);
            }
            playerCharacter = (Player)plrs[0];
            for (int i = 0; i<GlobSettings.playerCount;i++)
            {
                Player p = (Player)plrs[i];
                p.CreateKnowledge(plrs);
                //know[0].relationships[i] = playerCharacter.relationships[i].ToString();
            }
            players = plrs;
            //knowledge = know;
            FirstImpressions();
        }
        private void FirstImpressions()
        {
            for(int i = 0; i < players.Count; i++)
            {
                for(int j = 0; j < i; j++)
                {
                    
                    Player p1 = (Player)players[i];
                    Player p2 = (Player)players[j];
                    double fImp = (Misc.rng.Next(0, 101)-50);
                    fImp /= 100;
                    p1.relationships[p2.id] += fImp;
                    p2.relationships[p1.id] += fImp;
                }
            }
        }
        private void CreateTribes()
        {
            //List<string> colors = new List<string> { "#888888", "#FFFF00" };
            for(int i = 0; i < startingTribes; i++)
            {
                string name = names.tribeNames[Misc.rng.Next(0, names.tribeNames.Count)];
                names.tribeNames.Remove(name);
                Color color = Color.FromArgb(Misc.color1, Misc.color2, Misc.color3);

                tribes.Add(new Tribe(name, color, new Island(i)));
                islands.Add(tribes[i].island);
                int swap = Misc.color1;
                Misc.color1 = Misc.color2;
                Misc.color2 = Misc.color3;
                Misc.color3 = swap;
            }
            HideAdvantages();
            int playersPerTribe = players.Count / startingTribes;
            int currentTribeID = 0;
            List<int> ids = new List<int> { };
            for (int i = 0; i < players.Count; i++) ids.Add(i);
            while (ids.Count > 0)
            {
                int randId = ids[Misc.rng.Next(0, ids.Count)];
                Player p = (Player)players[randId];
                tribes[currentTribeID].members.Add(p);
                p.tribe = tribes[currentTribeID];
                p.tribeId = p.tribe.id;
                ids.Remove(randId);
                if (tribes[currentTribeID].members.Count == playersPerTribe) currentTribeID++;
            }
        }
        private void HideAdvantages()
        {
            foreach(Island i in islands)
            {
                List<Advantage> pool = new List<Advantage> { new AdvHiddenImmunityIdol(1), new AdvExtraVote(1) };
                foreach(Advantage a in pool)
                {
                    Sector randSector = null;
                    while (randSector == null || randSector.advantageHidden != null)
                    {
                        randSector = i.sectorList[Misc.rng.Next(0, i.sectorList.Count)];
                    }
                    randSector.advantageHidden = a;
                }
            }
        }
        private void UpdateTimeAndIsland()
        {
            Console.SetCursorPosition(WinProps.mapLeft, WinProps.mapTop - 2);
            Console.Write(playerCharacter.tribe + ", Day " + day + ", Time: " + time + ":00    "); 
            Console.SetCursorPosition(WinProps.mapInfoLeft, WinProps.mapTop - 2);
            Console.Write("Player's stats: Health: " + playerCharacter.Health + " Hunger: " + playerCharacter.Hunger + " Starving? " + (playerCharacter.starving ? "yes" : "no") + "      ");
            foreach (Island isl in islands)
            {
                foreach (Sector s in isl.sectorList)
                {
                    s.plrsOnSector.Clear();
                    s.ResetActions();
                }
            }
        }
        private void SaveTest()
        {
            /*
            SaveTest st = new SaveTest(playerCharacter);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(st, options);
            File.WriteAllText(@".\..\..\..\test.json", json);
            */
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(@".\..\..\..\save.json", json);
        }
        private void StartRound()
        {
            UI.CleanSelections();
            time = 8;
            actionDone = false;
            tribal = false;
            SaveTest();
            Island playersIsland = playerCharacter.tribe.island;
            scene = new SceneMap(playersIsland, this);

            scene.Display();
            if (day == 1)
            {
                UI.CleanSelections();
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                Console.Write("Welcome to Survivor! Your goal is to outwit, outplay and outlast other players");
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 1);
                Console.Write("But first you have to be divided into " + startingTribes + " tribes - each living on a different island");
                Console.ReadKey(true);
                foreach (Tribe t in tribes)
                {
                    UI.CleanSelections();
                    Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                    Console.Write("Following players will be members of " + t + ": ");
                    for (int i = 0; i < t.members.Count; i++)
                    {

                        Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 1 + i);
                        Console.Write(t.members[i]);
                    }
                    Console.ReadKey(true);
                }
                UI.CleanSelections();
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                Console.Write("All right then! Let the game begin. " + (players.Count - 1) + " days, " + players.Count + " people, 1 survivor...");
                Console.ReadKey(true);
                UI.CleanSelections();

            }

            if (day == swapDay)
            {
                Swap();
                playersIsland = playerCharacter.tribe.island;
            }
            else if (day == mergeDay)
            {
                Merge();
                merged = true;
                jury = true;
                playersIsland = playerCharacter.tribe.island;
            }

            while (time < 24)
            {
                UpdateTimeAndIsland();
                //actionDone = false;
                if (time == 15)
                {
                    scene = new SceneMap(playersIsland, this);
                    scene.Display();
                    Challenge ch;
                    if (!merged) ch = new ChallengeTugOfWar();
                    else ch = new ChallengePuzzle();
                    ch.forImmunity = true;
                    ch.PlayChallenge(tribes);
                    time++;
                    UI.CleanSelections();
                }
                else if (time == 10)
                {
                    scene = new SceneMap(playersIsland, this);
                    scene.Display();
                    Challenge ch;
                    if (!merged) ch = new ChallengeTugOfWar();
                    else ch = new ChallengePuzzle();
                    ch.forImmunity = false;
                    ch.reward = new FoodPizza(8);
                    ch.PlayChallenge(tribes);
                    time++;
                    UI.CleanSelections();
                }
                else if (time == 23)
                {
                    tribal = true;
                    RoundEvents();
                }
                else
                {
                    RoundEvents();
                }
            }
            if (!endgame) AfterTribalReset();
            if (PlayersInPreJury[PlayersInPreJury.Count - 1] == playerCharacter && !jury)
            {
                UI.CleanSelections();
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                Console.Write("YOU WERE VOTED OUT PRE-JURY");
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 1);
                Console.Write("That means, you won't have any impact on the rest of the game");
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 2);
                Console.Write("However, if you wish, you may observe how it plays out");
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 3);
                Console.Write("Do you want to continue? (Y/N)");
                while (true)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Y) break;
                    else if (key == ConsoleKey.N)
                    {
                        endgame = true;
                        break;
                    }
                }
                UI.CleanSelections();
            }
            else if (PlayersInJury.Count > 0 && PlayersInJury[PlayersInJury.Count - 1] == playerCharacter && !endgame)
            {
                UI.CleanSelections();
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                Console.Write("YOU WERE VOTED OUT, BUT YOU ARE A JUROR");
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 1);
                Console.Write("That means, you will have a vote on who will win the game");
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 2);
                Console.Write("However, if you wish, you may quit the game right now");
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 3);
                Console.Write("Do you want to continue? (Y/N)");
                while (true)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Y) break;
                    else if (key == ConsoleKey.N)
                    {
                        endgame = true;
                        break;
                    }
                }
                UI.CleanSelections();
            }


        }
        private void Swap()
        {
            UI.CleanSelections();
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            Console.Write("Drop your buffs, we are switching tribes!");
            Console.ReadKey(true);
            UI.CleanSelections();
            foreach (Tribe t in tribes) t.members.Clear();
            int newTribes = 2;
            int playersPerTribe = PlayersStillInGame.Count / newTribes;
            int currentTribeID = 0;
            List<int> ids = new List<int> { };
            for (int i = 0; i < players.Count; i++) if (PlayersStillInGame.Contains(players[i])) ids.Add(i);
            while (ids.Count > 0)
            {
                int randId = ids[Misc.rng.Next(0, ids.Count)];
                Player p = (Player)players[randId];
                tribes[currentTribeID].members.Add(p);
                p.tribe = tribes[currentTribeID];
                p.tribeId = p.tribe.id;
                ids.Remove(randId);
                if (tribes[currentTribeID].members.Count == playersPerTribe) currentTribeID++;
            }
            //napisac jeszcze kto w ktorej druzynie jest tak dla atmosfery
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            Console.Write("New members of " + tribes[0] + " are:");
            for (int i = 0; i < tribes[0].members.Count; i++)
            {
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + (i + 1));
                Console.Write(tribes[0].members[i]);
            }
            Console.ReadKey(true);
            UI.CleanSelections();
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            Console.Write("New members of " + tribes[1] + " are:");
            for (int i = 0; i < tribes[1].members.Count; i++)
            {
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + (i + 1));
                Console.Write(tribes[1].members[i]);
            }
            Console.ReadKey(true);
            UI.CleanSelections();
            List<Alliance> newAlls = new List<Alliance> { };
            foreach (Alliance a in alliances)
            {
                Alliance a1 = new Alliance(alliances.Count + newAlls.Count + 1);
                Alliance a2 = new Alliance(alliances.Count + newAlls.Count + 2);
                if (tribes[0].members.Contains(a.target)) { a1.target = a.target; a1.targetId = a.target.id; }
                else if (tribes[1].members.Contains(a.target)) { a2.target = a.target; a2.targetId = a.target.id; }
                a.target = null;
                a.targetId = -1;
                foreach (Player p in a.members)
                {
                    if (tribes[0].members.Contains(p)) a1.members.Add(p);
                    else if (tribes[1].members.Contains(p)) a2.members.Add(p);
                }
                if (a1.members.Count == 0 || a2.members.Count == 0) continue;
                else
                {
                    newAlls.Add(a1);
                    newAlls.Add(a2);
                    foreach (Player p in a1.members)
                    {
                        p.alliances.Add(a1);
                        double loyaltySum = 0;
                        foreach (Player p2 in a1.members)
                        {
                            if (p == p2) continue;
                            else
                            {
                                loyaltySum += p.relationships[p2.id];
                                p.knowledge[p2.id].alliances.Add(a1);
                                p.knowledge[p2.id].alliancesId.Add(a1.id);
                            }
                        }
                        a1.loyalty.Add(loyaltySum / (a1.members.Count - 1));
                    }
                    foreach (Player p in a2.members)
                    {
                        p.alliances.Add(a2);
                        double loyaltySum = 0;
                        foreach (Player p2 in a2.members)
                        {
                            if (p == p2) continue;
                            else
                            {
                                loyaltySum += p.relationships[p2.id];
                                p.knowledge[p2.id].alliances.Add(a2);
                                p.knowledge[p2.id].alliancesId.Add(a2.id);
                            }
                        }
                        a2.loyalty.Add(loyaltySum / (a2.members.Count - 1));
                    }

                }
            }
            foreach (Alliance a in newAlls) alliances.Add(a);
        }
        private void Merge()
        {
            UI.CleanSelections();
            foreach (Tribe t in tribes) t.members.Clear();
            Color color = Color.FromArgb(Misc.color1, Misc.color2, Misc.color3);
            string name = names.tribeNames[Misc.rng.Next(0, names.tribeNames.Count)];
            names.tribeNames.Remove(name);
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            Console.Write("Drop your buffs, we are merged!");
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 1);
            Console.Write("You are now members of " + tribes.Last() + " tribe");
            Console.ReadKey(true);
            UI.CleanSelections();
            tribes.Add(new Tribe(name, color, new Island(tribes.Count)));
            islands.Add(tribes[tribes.Count - 1].island);
            foreach (Player p in PlayersStillInGame)
            {
                tribes[tribes.Count - 1].members.Add(p);
                p.tribe = tribes[tribes.Count - 1];
                p.tribeId = p.tribe.id;
            }
        }
        private void RoundEvents()
        {
            //actionsThisRound = 0;
            List<int> pIDs = new List<int> { };
            foreach (Player p in PlayersStillInGame) pIDs.Add(p.id);
            while (pIDs.Count > 0)
            {
                //actions++;
                //actionsThisRound++;
                int randID = pIDs[Misc.rng.Next(0, pIDs.Count)];
                pIDs.Remove(randID);
                Player p = (Player)players[randID];
                if (tribal && !p.tribe.immune) continue;
                if (randID != playerCharacter.id) p.TakeAction(this);
                else
                {
                    actionDone = false;
                    PlacePlayers();
                    Island playersIsland = playerCharacter.tribe.island;
                    scene = new SceneMap(playersIsland, this);
                    Render();
                    UpdateTimeAndIsland();
                }
                if (p.starving) p.Hunger -= Misc.rng.Next(1, 6);
                else
                {
                    if (p.hoursSinceEating != 0 && Misc.rng.Next(p.hoursSinceEating, 300) < 24 * p.endurance)
                    {
                        p.starving = true;
                        if (p == playerCharacter)
                        {
                            UI.CleanSelections();
                            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                            Console.Write("You begin to starve");
                            Console.ReadKey(true);
                            UI.CleanSelections();
                        }
                        p.Hunger -= Misc.rng.Next(1, 6);
                    }
                }
                if (p.Hunger == 0)
                {
                    p.Health -= Misc.rng.Next(1, 6);
                }
                p.hoursSinceEating++;
                UI.CleanSelections();
            }
            time++;
            foreach (Island i in islands)
            {
                SectorCamp camp = (SectorCamp)i.sectorList[i.campSectorID];
                if (camp.CampfireLit)
                {
                    camp.CampfireHealth -= Misc.rng.Next(1, 21);
                    if (camp.CampfireHealth == 0 && i.id == playerCharacter.tribe.island.id)
                    {
                        UI.CleanSelections();
                        Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                        Console.Write("The campfire went out!");
                        Console.ReadKey(true);
                        UI.CleanSelections();
                    }
                }
            }
            if (tribal)
            {
                foreach (Tribe t in tribes)
                {
                    if (!t.immune && t.members.Count > 0)
                    {
                        if (t.members.Count > 4) TribalCouncil(t);
                        else if (t.members.Count == 4)
                        {
                            FireMakingTribalCouncil(t);
                            day++;
                            time = 23;
                            AfterTribalReset();
                            FinalTribalCouncil(t);
                        }
                    }
                }
            }
        }
        private void FinalTribalCouncil(Tribe tribe)
        {
            UI.CleanSelections();
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            Console.Write("Welcome to the final tribal council! The jury will now vote for the winner of Survior.");
            Console.ReadKey(true);
            Dictionary<Player, int> votes = new Dictionary<Player, int> { };

            List<Player> pl = new List<Player> { };
            foreach (Player p in tribe.members) pl.Add(p);

            foreach (Player p in PlayersInJury)
            {
                if (p == playerCharacter)
                {
                    actionDone = false;
                    scene = new SceneSelectPlayerToVote(this, null, tribe, null);
                    Render();
                }
                else
                {
                    p.SetVote(this, pl);
                }
                if (votes.ContainsKey(p.vote)) votes[p.vote]++;
                else votes.Add(p.vote, 1);
            }
            int top = 0;
            int top2 = 0;
            foreach (KeyValuePair<Player, int> kvp in votes)
            {
                if (kvp.Value > top)
                {
                    top2 = top;
                    top = kvp.Value;

                }
                else if (kvp.Value > top2)
                {
                    top2 = kvp.Value;
                }
            }
            List<Player> votesToRead = new List<Player> { };
            Player votedOut = null;
            int votesLeft = top - top2;
            int votesEnough;
            if (votesLeft == 0) votesEnough = 0;
            else votesEnough = (int)(Math.Floor(votesLeft * 0.5) + 1);
            foreach (KeyValuePair<Player, int> kvp in votes)
            {
                int max;
                if (kvp.Value > top2)
                {
                    max = top2;
                    votedOut = kvp.Key;
                }
                else max = kvp.Value;
                for (int i = 0; i < max; i++) votesToRead.Add(kvp.Key);
            }
            UI.CleanSelections();
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            Console.Write("Okay, I'll read the votes.");
            Console.ReadKey(true);
            int count = 1;
            string suff;
            while (votesEnough >= 1 || votesToRead.Count > 0)
            {
                if (count == 1) suff = "st";
                else if (count == 2) suff = "nd";
                else if (count == 3) suff = "rd";
                else suff = "th";
                Player vote;
                if (votesToRead.Count > 0)
                {
                    int index = Misc.rng.Next(0, votesToRead.Count);
                    vote = votesToRead[index];
                    votesToRead.RemoveAt(index);
                }
                else
                {
                    vote = votedOut;
                    votesEnough--;
                }

                if (votesEnough > 0 || votesToRead.Count > 0 || votedOut == null)
                {
                    UI.CleanSelections();
                    Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                    Console.Write(count + suff + " vote: ");
                    Console.ReadKey(true);
                    Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 1);
                    Console.Write(vote);
                    Console.ReadKey(true);
                }
                else
                {
                    UI.CleanSelections();
                    Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                    Console.Write("The winner of Survivor: ");
                    Console.ReadKey(true);
                    Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop+1);
                    Console.Write(vote);
                    Console.ReadKey(true);
                    endgame = true;
                    return;
                }

                count++;
            }
            List<Player> tied = new List<Player> { };
            Player notTied = null;
            foreach (KeyValuePair<Player, int> kvp in votes)
            {
                if (kvp.Value == top) tied.Add(kvp.Key);
            }
            foreach (Player p in PlayersStillInGame) if (!tied.Contains(p)) notTied = p;
            UI.CleanSelections();
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            Console.Write("We have a tie between: ");
            for (int i = 1; i <= tied.Count; i++)
            {
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + i);
                Console.Write(tied[i - 1]);
            }
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            Console.Write("In that case, " + notTied + " will cast the deciding vote.");
            Console.ReadKey(true);
            if (notTied == playerCharacter)
            {
                actionDone = false;
                scene = new SceneSelectPlayerToVote(this, null, tribe, tied);
                Render();
            }
            else
            {
                notTied.SetVote(this, tied);
            }

            UI.CleanSelections();
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            Console.Write("The winner of Survivor: ");
            Console.ReadKey(true);
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop+1);
            Console.Write(notTied.vote);
            Console.ReadKey(true);
            endgame = true;
            return;

        }
        private void FireMakingTribalCouncil(Tribe t)
        {
            Player immune = null;
            List<Selectable> inDanger = new List<Selectable> { };
            foreach (Player p in t.members)
            {
                if (p.immune) immune = p;
                else inDanger.Add(p);
            }
            UI.CleanSelections();
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            Console.Write("Welcome to the tribal council! We will soon begin the fire making challenge.");
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 1);
            Console.Write("But first, " + immune + " since you are immune, you have to choose one person to take to the final three.");
            Console.ReadKey(true);
            if (immune == playerCharacter)
            {
                actionDone = false;
                scene = new SceneSelectFinalist(this, null, inDanger);
                Render();
            }
            else
            {
                immune.ChooseFinalist(players, inDanger);
            }
            UI.CleanSelections();
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            Console.Write("A player that " + immune + " chose to take to the final three is:");
            Console.ReadKey(true);
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 1);
            Console.Write(immune.vote);
            Console.ReadKey(true);
            inDanger.Remove(immune.vote);
            UI.CleanSelections();
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
            Console.Write("That means, " + inDanger[0] + " and " + inDanger[1] + " will face off in fire making challenge");
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 1);
            Console.Write("I hope you already learned how to make fire");
            Console.ReadKey(true);
            UI.CleanSelections();
            int turn = Misc.rng.Next(0, 2);
            Player eliminated = null;
            while (inDanger.Count > 1)
            {
                Player p = (Player)inDanger[turn];
                if (Misc.rng.Next(0, 30) < p.mental + p.firesMade)
                {
                    Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                    Console.Write(p + " managed to make fire first!");
                    inDanger.Remove(p);
                    eliminated = (Player)inDanger[0];
                    Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 1);
                    Console.Write("That means, " + eliminated + " is eliminated and becomes the final member of the jury");
                    Console.ReadKey(true);
                }
            }
            UI.CleanSelections();
            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 1);
            Console.Write(eliminated + ", the fire has spoken...");
            Console.ReadKey(true);
            eliminated.name += " (VOTED OUT)";
            if (!jury)
            {
                PlayersInPreJury.Add(eliminated);
                eliminated.name += " (VOTED OUT)";
            }
            else
            {
                PlayersInJury.Add(eliminated);
                eliminated.name += " (JURY MEMBER)";
            }
            PlayersStillInGame.Remove(eliminated);
            eliminated.tribe.members.Remove(eliminated);
            playersEliminated++;
            return;
        }
        private void TribalCouncil(Tribe tribe)
        {
            if (tribe == playerCharacter.tribe || !PlayersStillInGame.Contains(playerCharacter))
            {
                UI.CleanSelections();
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                Console.Write("Welcome to the tribal council! It is time to vote.");
                Console.ReadKey(true);
            }
            Dictionary<Player, int> votes = new Dictionary<Player, int> { };

            foreach (Player p in tribe.members)
            {
                if (p == playerCharacter)
                {
                    actionDone = false;
                    scene = new SceneSelectPlayerToVote(this, null, tribe, null);
                    Render();
                }
                else
                {
                    p.SetVote(this, null);
                }
                if (votes.ContainsKey(p.vote)) votes[p.vote]++;
                else votes.Add(p.vote, 1);
            }
            int top = 0;
            int top2 = 0;
            foreach (KeyValuePair<Player, int> kvp in votes)
            {
                if (kvp.Value > top)
                {
                    top2 = top;
                    top = kvp.Value;

                }
                else if (kvp.Value > top2)
                {
                    top2 = kvp.Value;
                }
            }
            List<Player> votesToRead = new List<Player> { };
            Player votedOut = null;
            int votesLeft = top - top2;
            int votesEnough;
            if (votesLeft == 0) votesEnough = 0;
            else votesEnough = (int)(Math.Floor(votesLeft * 0.5) + 1);
            foreach (KeyValuePair<Player, int> kvp in votes)
            {
                int max;
                if (kvp.Value > top2)
                {
                    max = top2;
                    votedOut = kvp.Key;
                }
                else max = kvp.Value;
                for (int i = 0; i < max; i++) votesToRead.Add(kvp.Key);
            }
            if(tribe == playerCharacter.tribe || !PlayersStillInGame.Contains(playerCharacter))
            {
                UI.CleanSelections();
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                Console.Write("Once the votes are read the decision is final.");
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 1);
                Console.Write("The person voted out will be asked to leave the tribal council are immediately.");
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 2);
                Console.Write("I'll read the votes.");
                Console.ReadKey(true);
            }
            int count = 1;
            string suff;
            while (votesEnough >= 1 || votesToRead.Count > 0)
            {
                if (count == 1) suff = "st";
                else if (count == 2) suff = "nd";
                else if (count == 3) suff = "rd";
                else suff = "th";
                Player vote;
                if (votesToRead.Count > 0)
                {
                    int index = Misc.rng.Next(0, votesToRead.Count);
                    vote = votesToRead[index];
                    votesToRead.RemoveAt(index);
                }
                else
                {
                    vote = votedOut;
                    votesEnough--;
                }

                if (votesEnough > 0 || votesToRead.Count > 0 || votedOut == null)
                {
                    if (tribe == playerCharacter.tribe || !PlayersStillInGame.Contains(playerCharacter))
                    {
                        UI.CleanSelections();
                        Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                        Console.Write(count + suff + " vote: ");
                        Console.ReadKey(true);
                        Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 1);
                        Console.Write(vote);
                        Console.ReadKey(true);
                    }
                }
                else
                {
                    count = playersEliminated + 1;
                    if (count == 1) suff = "st";
                    else if (count == 2) suff = "nd";
                    else if (count == 3) suff = "rd";
                    else suff = "th";
                    int count2 = PlayersInJury.Count + 1;
                    string suff2;
                    if (count2 == 1) suff2 = "st";
                    else if (count2 == 2) suff2 = "nd";
                    else if (count2 == 3) suff2 = "rd";
                    else suff2 = "th";
                    if (tribe == playerCharacter.tribe || !PlayersStillInGame.Contains(playerCharacter))
                    {
                        UI.CleanSelections();
                        Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                        if (!jury) Console.Write(count + suff + " person voted out: ");
                        else Console.Write(count + suff + " person voted out, and the " + count2 + suff2 + " member of the jury: ");
                        Console.ReadKey(true);
                        Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 1);
                        Console.Write(vote);
                        Console.ReadKey(true);
                        UI.CleanSelections();
                        Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                        Console.Write(vote + ", the tribe has spoken...");
                        Console.ReadKey(true);
                    }
                    if (!jury)
                    {
                        PlayersInPreJury.Add(vote);
                        vote.name += " (VOTED OUT)";
                    }
                    else
                    {
                        PlayersInJury.Add(vote);
                        vote.name += " (JURY MEMBER)";
                    }
                    PlayersStillInGame.Remove(vote);
                    vote.tribe.members.Remove(vote);
                    playersEliminated++;
                    foreach (Player p in PlayersStillInGame) if (p.vote == vote) p.correctVotes++;
                    return;
                }
                count++;
            }
            // jak tu dojdzie to znaczy ze remis

            List<Player> tied = new List<Player> { };
            foreach (KeyValuePair<Player, int> kvp in votes)
            {
                if (kvp.Value == top) tied.Add(kvp.Key);
            }
            if (tribe == playerCharacter.tribe || !PlayersStillInGame.Contains(playerCharacter))
            {
                UI.CleanSelections();
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                Console.Write("We have a tie between: ");
                for (int i = 1; i <= tied.Count; i++)
                {
                    Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + i);
                    Console.Write(tied[i - 1]);
                }
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                Console.Write("Time for a revote, these players will not vote.");
                Console.ReadKey(true);
            }
            votes = new Dictionary<Player, int> { };
            foreach (Player p in tribe.members)
            {
                p.vote = null;
                if (tied.Contains(p)) continue;
                if (p == playerCharacter)
                {
                    actionDone = false;
                    scene = new SceneSelectPlayerToVote(this, null, tribe, tied);
                    Render();
                }
                else
                {
                    p.SetVote(this, tied);
                }
                if (votes.ContainsKey(p.vote)) votes[p.vote]++;
                else votes.Add(p.vote, 1);
            }
            top = 0;
            top2 = 0;
            foreach (KeyValuePair<Player, int> kvp in votes)
            {
                if (kvp.Value > top)
                {
                    top2 = top;
                    top = kvp.Value;

                }
                else if (kvp.Value > top2)
                {
                    top2 = kvp.Value;
                }
            }
            votesToRead = new List<Player> { };
            votedOut = null;
            votesLeft = top - top2;
            if (votesLeft == 0) votesEnough = 0;
            else votesEnough = (int)(Math.Floor(votesLeft * 0.5) + 1);
            foreach (KeyValuePair<Player, int> kvp in votes)
            {
                int max;
                if (kvp.Value > top2)
                {
                    max = top2;
                    votedOut = kvp.Key;
                }
                else max = kvp.Value;
                for (int i = 0; i < max; i++) votesToRead.Add(kvp.Key);
            }
            if (tribe == playerCharacter.tribe || !PlayersStillInGame.Contains(playerCharacter))
            {
                UI.CleanSelections();
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                Console.Write("Once the votes are read the decision is final.");
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 1);
                Console.Write("The person voted out will be asked to leave the tribal council are immediately.");
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 2);
                Console.Write("I'll read the votes.");
                Console.ReadKey(true);
            }
            count = 1;
            while (votesEnough >= 1 || votesToRead.Count > 0)
            {
                if (count == 1) suff = "st";
                else if (count == 2) suff = "nd";
                else if (count == 3) suff = "rd";
                else suff = "th";
                Player vote;
                if (votesToRead.Count > 0)
                {
                    int index = Misc.rng.Next(0, votesToRead.Count);
                    vote = votesToRead[index];
                    votesToRead.RemoveAt(index);
                }
                else
                {
                    vote = votedOut;
                    votesEnough--;
                }

                if (votesEnough > 0 || votesToRead.Count > 0 || votedOut == null)
                {
                    if (tribe == playerCharacter.tribe || !PlayersStillInGame.Contains(playerCharacter))
                    {
                        UI.CleanSelections();
                        Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                        Console.Write(count + suff + " vote: ");
                        Console.ReadKey(true);
                        Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 1);
                        Console.Write(vote);
                        Console.ReadKey(true);
                    }
                }
                else
                {
                    count = playersEliminated + 1;
                    if (count == 1) suff = "st";
                    else if (count == 2) suff = "nd";
                    else if (count == 3) suff = "rd";
                    else suff = "th";
                    int count2 = PlayersInJury.Count + 1;
                    string suff2;
                    if (count2 == 1) suff2 = "st";
                    else if (count2 == 2) suff2 = "nd";
                    else if (count2 == 3) suff2 = "rd";
                    else suff2 = "th"; if (tribe == playerCharacter.tribe || !PlayersStillInGame.Contains(playerCharacter))
                    {
                        UI.CleanSelections();
                        Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                        if (!jury) Console.Write(count + suff + " person voted out: ");
                        else Console.Write(count + suff + " person voted out, and the " + count2 + suff2 + " member of the jury: ");
                        Console.ReadKey(true);
                        Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 1);
                        Console.Write(vote);
                        Console.ReadKey(true);
                        UI.CleanSelections();
                        Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                        Console.Write(vote + ", the tribe has spoken...");
                        Console.ReadKey(true);
                    }
                    if (!jury)
                    {
                        PlayersInPreJury.Add(vote);
                        vote.name += " (VOTED OUT)";
                    }
                    else
                    {
                        PlayersInJury.Add(vote);
                        vote.name += " (JURY MEMBER)";
                    }
                    PlayersStillInGame.Remove(vote);
                    vote.tribe.members.Remove(vote);
                    playersEliminated++;
                    foreach (Player p in PlayersStillInGame) if (p.vote == vote) p.correctVotes++;
                    return;
                }
                count++;
            }
            //kamiory
            tied = new List<Player> { };
            foreach (KeyValuePair<Player, int> kvp in votes)
            {
                if (kvp.Value == top) tied.Add(kvp.Key);
            }
            if (tribe == playerCharacter.tribe || !PlayersStillInGame.Contains(playerCharacter))
            {
                UI.CleanSelections();
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                Console.Write("We have a deadlock vote.");
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + 1);
                Console.Write("Following players are now immune:"); 
            }
            for (int i = 1; i <= tied.Count; i++)
            {
                if (tribe == playerCharacter.tribe || !PlayersStillInGame.Contains(playerCharacter))
                {
                    Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + (i + 1));
                    Console.Write(tied[i - 1]);
                }
                tied[i - 1].immune = true;
            }
            if (tribe == playerCharacter.tribe || !PlayersStillInGame.Contains(playerCharacter))
            {
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                Console.Write("The rest will draw rocks.");
                Console.ReadKey(true);
                UI.CleanSelections();
            }
            while (votedOut == null || votedOut.immune) votedOut = (Player)tribe.members[Misc.rng.Next(0, tribe.members.Count)];

            count = playersEliminated + 1;
            if (count == 1) suff = "st";
            else if (count == 2) suff = "nd";
            else if (count == 3) suff = "rd";
            else suff = "th"; if (tribe == playerCharacter.tribe || !PlayersStillInGame.Contains(playerCharacter))
            {
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                Console.Write(votedOut + " has drawn the purple rock and is the " + count + suff + " person out.");
                Console.ReadKey(true);
                UI.CleanSelections();
                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                Console.Write(votedOut + ", the rocks have spoken...");
                Console.ReadKey(true);
            }
            if (!jury)
            {
                PlayersInPreJury.Add(votedOut);
                votedOut.name += " (VOTED OUT)";
            }
            else
            {
                PlayersInJury.Add(votedOut);
                votedOut.name += " (JURY MEMBER)";
            }
            PlayersStillInGame.Remove(votedOut);
            votedOut.tribe.members.Remove(votedOut);
            playersEliminated++;

        }
        private void AfterTribalReset()
        {
            foreach (Tribe t in tribes)
            {
                t.immune = false;
            }
            foreach (Player p in PlayersStillInGame)
            {
                p.immune = false;
                p.vote = null;
                foreach (PlayerKnowledge pk in p.knowledge)
                {
                    if (!PlayersStillInGame.Contains(pk.biggestTarget)) { pk.biggestTarget = null; pk.biggestTargetId = -1; }
                }
            }
            foreach (Alliance a in alliances)
            {
                if (!PlayersStillInGame.Contains(a.target)) { a.target = null; a.targetId = -1; }
                foreach (Player p in players) if (a.members.Contains(p) && !PlayersStillInGame.Contains(p)) a.members.Remove(p);
            }
        }
        private void PlacePlayers()
        {
            foreach (Island isl in islands)
            {
                foreach (Sector s in isl.sectorList)
                {
                    s.plrsOnSector.Clear();
                    s.ResetActions();
                }
                List<Selectable> plrs = tribes[isl.id].members;
                //if (plrs.Contains(gR.playerCharacter)) plrs.Remove(gR.playerCharacter);
                List<int> idsToPlace = new List<int> { };
                foreach (Player p in plrs) if (p != playerCharacter) idsToPlace.Add(p.id);
                //idsToPlace.RemoveAt(0);
                int idsCount = idsToPlace.Count;
                List<int> notEmpty = new List<int> { };
                for (int i = 0; i < idsCount; i++)
                {
                    int pID = idsToPlace[Misc.rng.Next(0, idsToPlace.Count)]; //get random playerID
                    idsToPlace.Remove(pID); //remove that player from pool
                    Player plr = (Player)players[pID];
                    int sector;
                    if (Misc.rng.Next(0, 10) < 6 && notEmpty.Count > 0) sector = notEmpty[Misc.rng.Next(0, notEmpty.Count)]; //join somebody
                    else sector = Misc.rng.Next(0, GlobSettings.islandSize * GlobSettings.islandSize); //go random place
                    if (!notEmpty.Contains(sector)) notEmpty.Add(sector);

                    plr.sectorId = sector;
                    isl.sectorList[sector].plrsOnSector.Add(plr);

                }
                foreach (Sector s in isl.sectorList)
                {
                    s.ResetActions();
                    if (s.plrsOnSector.Count > 0)
                    {
                        s.possibleActions.Add(new ActionTalkPrivately());
                        if (s.plrsOnSector.Count > 2) s.possibleActions.Add(new ActionTalkToGroup());
                        if (s.plrsOnSector.Count > 1) s.possibleActions.Add(new ActionTalkToAll());
                    }
                }
                //plrs.Add(gR.playerCharacter);
            }
            /*
            foreach (Sector s in gR.island.sectorList)
            {
                s.plrsOnSector.Clear();
                s.ResetActions();
            }
            List<Selectable> plrs = gR.players;
            List<int> idsToPlace = new List<int> { };
            foreach (Player p in plrs) idsToPlace.Add(p.id);
            idsToPlace.RemoveAt(0);
            List<int> notEmpty = new List<int> { };
            for (int i = 1; i < plrs.Count; i++)
            {
                int pID = idsToPlace[Misc.rng.Next(0,idsToPlace.Count)]; //get random playerID
                idsToPlace.Remove(pID); //remove that player from pool
                Player plr = (Player)plrs[pID];
                int sector;
                if (Misc.rng.Next(0, 10) < 6 && notEmpty.Count>0) sector = notEmpty[Misc.rng.Next(0, notEmpty.Count)]; //join somebody
                else sector = Misc.rng.Next(0, GlobSettings.islandSize * GlobSettings.islandSize); //go random place
                if (!notEmpty.Contains(sector)) notEmpty.Add(sector);

                plr.sectorId = sector;
                gR.island.sectorList[sector].plrsOnSector.Add(plr);

            }
            foreach (Sector s in gR.island.sectorList)
            {
                s.ResetActions();
                if (s.plrsOnSector.Count > 0)
                {
                    s.possibleActions.Add(new ActionTalkPrivately());
                    if (s.plrsOnSector.Count > 2) s.possibleActions.Add(new ActionTalkToGroup());
                    if (s.plrsOnSector.Count > 1) s.possibleActions.Add(new ActionTalkToAll());
                }
            }
            return plrs;
            */
        }
        public void Render()
        {
            while (!actionDone)
            {
                scene.Display();
                Console.CursorVisible = false;
                UI.KeyPressHandling(Console.ReadKey(true), scene);
            }
        }
    }
}
