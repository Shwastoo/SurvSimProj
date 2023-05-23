using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Pastel;
using Newtonsoft.Json;

namespace survSimProject
{
    class Player : Selectable
    {
        public int id;
        public string name;

        public double endurance;
        public double mental;
        public double lying;
        public double charisma;

        private double health = 100;
        private double hunger = 100;
        public double Health
        {
            get { return health; }
            set
            {
                if (value > 100) health = 100;
                else if (value < 0) health = 0;
                else health = value;
            }
        }
        public double Hunger
        {
            get { return hunger; }
            set
            {
                if (value > 100) hunger = 100;
                else if (value < 0) hunger = 0;
                else hunger = value;
            }
        }

        public bool immune = false;
        public bool starving = false;
        public int hoursSinceEating = 0;
        public int firesMade = 0;
        public int compWins = 0;
        public int correctVotes = 0;

        public List<double> relationships = new List<double> { };
        public List<Selectable> inventory = new List<Selectable> { };
        [JsonIgnore]
        public List<Alliance> alliances = new List<Alliance> { };

        public List<PlayerKnowledge> knowledge;

        public Player vote;

        [JsonIgnore]
        public Tribe tribe;
        public int tribeId;
        public int sectorId;

        public Player(double _end, double _men, double _lie, double _chr, string _name)
        {
            id = 0;
            name = "Player" + id;
            endurance = _end;
            mental = _men;
            lying = _lie;
            charisma = _chr;
            for(int i = 0; i < GlobSettings.playerCount; i++)
            {
                relationships.Add(5);
            }

        }
        public Player(int _id, string _name)
        {
            id = _id;
            name = _name;
            for (int i = 0; i < GlobSettings.maxAbilityPoints; i++)
            {
                switch (Misc.rng.Next(4))
                {
                    case 0:
                        if (endurance < 10) endurance++;
                        else i--;
                        break;
                    case 1:
                        if (mental < 10) mental++;
                        else i--;
                        break;
                    case 2:
                        if (lying < 10) lying++;
                        else i--;
                        break;
                    case 3:
                        if (charisma < 10) charisma++;
                        else i--;
                        break;
                }
            }
            for (int i = 0; i < GlobSettings.playerCount; i++)
            {
                relationships.Add(5);
            }
        }
        public void CreateKnowledge(List<Selectable> plrs)
        {
            knowledge = new List<PlayerKnowledge> { };
            for (int i = 0; i < GlobSettings.playerCount; i++)
            {
                knowledge.Add(new PlayerKnowledge(i));
            }
            for (int i = 0; i < GlobSettings.playerCount; i++)
            {
                Player p = (Player)plrs[id];
                knowledge[id].relationships[i] = p.relationships[i].ToString();
                knowledge[i].relationships[id] = p.relationships[i].ToString();
            }
        }
        public void TakeAction(Game gameRef)
        {
            SectorCamp camp = (SectorCamp)tribe.island.sectorList[tribe.island.campSectorID];
            //dice roll check if it will be a social action, camp action, or self action
            int diceRoll = Misc.rng.Next(0, 20);
            if(diceRoll < 3) //camp action
            {
                //prioritize making fire if not made
                diceRoll = Misc.rng.Next(0, 4);
                if (diceRoll != 0 && !camp.CampfireLit)
                {
                    //make fire
                    ActionMakeFire a = new ActionMakeFire(camp);
                    a.player = this;
                    a.Selected(gameRef);
                }
                else
                {
                    diceRoll = Misc.rng.Next(0, 8);
                    if (diceRoll == 0 || diceRoll == 1)
                    {
                        //gather food
                        Sector randSector = null;
                        while (randSector == null || randSector is SectorCamp || randSector is SectorShore)
                        {
                            randSector = tribe.island.sectorList[Misc.rng.Next(0, tribe.island.sectorList.Count)];
                        }
                        ActionLookForFood a = new ActionLookForFood(randSector);
                        a.player = this;
                        a.Selected(gameRef);
                    }
                    else if (diceRoll == 2)
                    {
                        //gather wood
                        Sector randSector = null;
                        while (randSector == null || randSector is SectorCamp)
                        {
                            randSector = tribe.island.sectorList[Misc.rng.Next(0, tribe.island.sectorList.Count)];
                        }
                        ActionGatherWood a = new ActionGatherWood(randSector);
                        a.player = this;
                        a.Selected(gameRef);
                    }
                    else if (diceRoll == 3 || diceRoll == 4)
                    {
                        //go fishing
                        Sector randSector = null;
                        while (randSector == null || !(randSector is SectorShore))
                        {
                            randSector = tribe.island.sectorList[Misc.rng.Next(0, tribe.island.sectorList.Count)];
                        }
                        ActionFishing a = new ActionFishing((SectorShore)randSector);
                        a.player = this;
                        a.Selected(gameRef);
                    }
                    else if (diceRoll >= 5)
                    {
                        //fire
                        if (camp.CampfireLit)
                        {
                            if (camp.CampfireHealth < 90)
                            {
                                //tend fire
                                ActionTendFire a = new ActionTendFire(camp);
                                a.player = this;
                                a.Selected(gameRef);

                            }
                            else
                            {
                                //gather wood
                                Sector randSector = null;
                                while (randSector == null || randSector is SectorCamp)
                                {
                                    randSector = tribe.island.sectorList[Misc.rng.Next(0, tribe.island.sectorList.Count)];
                                }
                                ActionGatherWood a = new ActionGatherWood(randSector);
                                a.player = this;
                                a.Selected(gameRef);
                            }
                        }
                        else
                        {
                            //make fire
                            ActionMakeFire a = new ActionMakeFire(camp);
                            a.player = this;
                            a.Selected(gameRef);
                        }
                    }
                }
                
            }
            else if (diceRoll < 15) //social action
            {
                List<Alliance> allsWOTarget = new List<Alliance> { };
                foreach(Alliance a in alliances)
                {
                    foreach (Player p in a.members) if (!tribe.members.Contains(p)) return;
                    if (a.target == null && a.members.Count != tribe.members.Count)
                    {
                        allsWOTarget.Add(a);
                    }
                }
                if (allsWOTarget.Count != 0 && Misc.rng.Next(8, 23) < gameRef.time)
                {
                    //gather alliance without target set
                    Alliance randAll = allsWOTarget[Misc.rng.Next(0, allsWOTarget.Count)];
                    if (randAll.members.Count == tribe.members.Count)
                    {
                        return;
                    }
                    TopicAllianceTarget a = new TopicAllianceTarget(randAll.members);
                    a.alliance = randAll;
                    if (a.alliance.members.Contains(gameRef.playerCharacter))
                    {
                        UI.CleanSelections();
                        gameRef.actionDone = false;
                        Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                        Console.Write(this + " gathered all members of " + a.alliance + " alliance");
                        Console.ReadKey(true);
                        gameRef.scene = new SceneSelectOtherPlayer(gameRef, null, a.alliance.members, "Who do you want to target?", a);
                        gameRef.Render();
                    }
                    else
                    {
                        a.Selected(gameRef);
                    }
                }
                else
                {
                    diceRoll = Misc.rng.Next(0, 20);
                    if (diceRoll < 6)
                    {
                        //propose alliance
                        //or
                        //general talk
                        int max = Convert.ToInt32((Math.Log2(tribe.members.Count - 1) + 3) * 100);
                        int randValue = Misc.rng.Next(0, max);
                        int playerCount = 0;
                        int x = 1;
                        while (playerCount == 0)
                        {
                            if (randValue < (Math.Log2(x) + 3) * 100) playerCount = x;
                            else x++;
                        }
                        List<Selectable> plrs = new List<Selectable> { };
                        for (int i = 0; i < playerCount; i++)
                        {
                            Player randP = (Player)tribe.members[Misc.rng.Next(0, tribe.members.Count)];
                            if (randP == this || plrs.Contains(randP)) i--;
                            else plrs.Add(randP);
                        }
                        List<Player> alignedPlayers = new List<Player> { };
                        bool exactAlliance = false;
                        foreach (Alliance a in alliances)
                        {
                            bool check = false;
                            foreach (Player p in a.members)
                            {
                                if (!alignedPlayers.Contains(p) && p != this) alignedPlayers.Add(p);
                                if (!plrs.Contains(p)) check = true;
                            }
                            if(check == false && a.members.Count == plrs.Count) exactAlliance = true;
                        }
                        if (Misc.rng.Next(0, Convert.ToInt32(Math.Pow(2, alignedPlayers.Count))) == 0 && !exactAlliance && playerCount != tribe.members.Count-1)
                        {
                            //propose alliance
                            TopicCreateAlliance a = new TopicCreateAlliance(plrs, this);
                            a.Selected(gameRef);
                        }
                        else
                        {
                            //general talk
                            if (plrs.Contains(gameRef.playerCharacter))
                            {
                                UI.CleanSelections();
                                Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                                if (plrs.Count == 1) Console.Write(this + " created a conversation with you");
                                else
                                {
                                    Console.Write(this + " created a conversation between you, them and: ");
                                    int offset = 1;
                                    foreach (Player p in plrs)
                                    {
                                        Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop + offset);
                                        if (p != gameRef.playerCharacter)
                                        {
                                            Console.Write(p);
                                            offset++;
                                        }
                                    }
                                }
                                Console.ReadKey(true);
                                UI.CleanSelections();
                            }
                            TopicGeneral a = new TopicGeneral(plrs, this);
                            a.Selected(gameRef);
                        }
                    }
                    else if (diceRoll < 12)
                    {
                        //talk about relationship
                        int loopGuard = 0;
                        Player plToTalkWith = null;
                        while (plToTalkWith == null || plToTalkWith == this)
                        {
                            plToTalkWith = (Player)tribe.members[Misc.rng.Next(0, tribe.members.Count)];
                            loopGuard++;
                            if (loopGuard > 100)
                            {
                                return;
                            }
                        }
                        Player plToTalkAbout = null;
                        while (plToTalkAbout == null || plToTalkAbout == this || plToTalkAbout == plToTalkWith)
                        {
                            plToTalkAbout = (Player)tribe.members[Misc.rng.Next(0, tribe.members.Count)];
                            loopGuard++;
                            if (loopGuard > 100)
                            {
                                return;
                            }
                        }
                        if (plToTalkWith != gameRef.playerCharacter)
                        {
                            TopicRelationInd a = new TopicRelationInd(new List<Selectable> { plToTalkWith, this });
                            a.target = plToTalkAbout;
                            a.Selected(gameRef);
                            TopicRelationInd b = new TopicRelationInd(new List<Selectable> { this, plToTalkWith });
                            b.target = plToTalkAbout;
                            b.Selected(gameRef);
                        }
                        else
                        {
                            UI.CleanSelections();
                            gameRef.actionDone = false;
                            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                            Console.Write(this + " asked you about your relationship with " + plToTalkAbout);
                            Console.ReadKey(true);
                            gameRef.scene = new SceneSelectRelationLie(gameRef, null, this, plToTalkAbout); ;
                            gameRef.Render();
                            TopicRelationInd b = new TopicRelationInd(new List<Selectable> { this, plToTalkWith });
                            b.target = plToTalkAbout;
                            b.Selected(gameRef);
                        }
                    }
                    else if (diceRoll < 17)
                    {
                        //talk about targets
                        Player plToTalkWith = null;
                        int loopGuard = 0;
                        while (plToTalkWith == null || plToTalkWith == this)
                        {
                            plToTalkWith = (Player)tribe.members[Misc.rng.Next(0, tribe.members.Count)];
                            loopGuard++;
                            if (loopGuard > 100)
                            {
                                return;
                            }
                        }
                        if (plToTalkWith != gameRef.playerCharacter)
                        {
                            TopicTargetInd a = new TopicTargetInd(new List<Selectable> { plToTalkWith, this });
                            a.Selected(gameRef);
                            TopicTargetInd b = new TopicTargetInd(new List<Selectable> { this, plToTalkWith });
                            b.Selected(gameRef);
                        }
                        else
                        {
                            UI.CleanSelections();
                            gameRef.actionDone = false;
                            Console.SetCursorPosition(WinProps.selectionsLeft, WinProps.selectionsTop);
                            Console.Write(this + " asked you about your current target");
                            Console.ReadKey(true);
                            gameRef.scene = new SceneSelectOtherPlayer(gameRef, null, new List<Selectable> {this }, "Who is your biggest target?", new TopicYourTarget(new List<Selectable> { this }));
                            gameRef.Render();
                            TopicTargetInd b = new TopicTargetInd(new List<Selectable> { this, plToTalkWith });
                            b.Selected(gameRef);
                        }
                    }
                    else if (diceRoll < 20)
                    {
                        //warn
                        diceRoll = Misc.rng.Next(0, 5);
                        if (diceRoll == 0)
                        {
                            //lie
                            Player targeter = null;
                            int loopGuard = 0;
                            while (targeter == null || targeter == this || knowledge[targeter.id].alliances.Count>0)
                            {
                                targeter = (Player)tribe.members[Misc.rng.Next(0, tribe.members.Count)];
                                loopGuard++;
                                if (loopGuard > 100)
                                {
                                    return;
                                }
                            }
                            Player theirTarget = null;
                            loopGuard = 0;
                            while (theirTarget == null || theirTarget == this || knowledge[theirTarget.id].alliances.Count>0 || theirTarget==targeter)
                            {
                                theirTarget = (Player)tribe.members[Misc.rng.Next(0, tribe.members.Count)];
                                loopGuard++;
                                if (loopGuard > 100)
                                {
                                    return;
                                }
                            }
                            TopicWarn a = new TopicWarn(new List<Selectable> { theirTarget, this });
                            a.target = targeter;
                            a.Selected(gameRef);
                        }
                        else
                        {
                            //truth
                            List<PlayerKnowledge> plrs = new List<PlayerKnowledge> { };
                            foreach(PlayerKnowledge pk in knowledge)
                            {
                                if(pk.biggestTarget != null)
                                {
                                    plrs.Add(pk);
                                }
                            }
                            if (plrs.Count > 0)
                            {
                                PlayerKnowledge randKnow = null; //player who has a known target
                                int loopGuard = 0;
                                while (randKnow == null || randKnow == knowledge[id] || randKnow.alliances.Count > 0)
                                {
                                    randKnow = plrs[Misc.rng.Next(0, plrs.Count)];
                                    loopGuard++;
                                    if (loopGuard > 100)
                                    {
                                        return;
                                    }
                                }
                                Player targeter = (Player)gameRef.players[randKnow.id];
                                Player theirTarget = randKnow.biggestTarget;
                                TopicWarn a = new TopicWarn(new List<Selectable> { theirTarget, this });
                                a.target = targeter;
                                a.Selected(gameRef);
                            }
                            else
                            {
                                //general talk
                            }
                        }
                    }
                }
            }
            else //self action
            {
                diceRoll = Misc.rng.Next(0, 4);
                if(diceRoll < 1)
                {
                    //look for adv
                    Sector randSector = tribe.island.sectorList[Misc.rng.Next(0, tribe.island.sectorList.Count)];
                    ActionLookForAdv a = new ActionLookForAdv(randSector);
                    a.player = this;
                    a.Selected(gameRef);
                }
                else
                {
                    //eat
                    if (camp.stash.OfType<Food>().Any() && starving && Hunger<90) 
                    {
                        List<Food> food = camp.stash.OfType<Food>().ToList();
                        if (camp.CampfireLit && food.Where(x=> x.cookable).Any())
                        {
                            List<Food> foodCookable = new List<Food> { };
                            foreach (Food f in food)
                            {
                                if (f.cookable) foodCookable.Add(f);
                            }
                            Food foodItem = foodCookable[Misc.rng.Next(0, foodCookable.Count)];
                            //eat cooked foodItem
                            ActionCookFood a = new ActionCookFood(camp);
                            a.player = this;
                            a.food = foodItem;
                            a.Selected(gameRef);
                        }
                        else
                        {
                            Food foodItem = food[Misc.rng.Next(0, food.Count)];
                            //eat not cooked foodItem
                            ActionEatFood a = new ActionEatFood(camp);
                            a.player = this;
                            a.food = foodItem;
                            a.Selected(gameRef);
                        }
                    }
                    else
                    {
                        //gather food
                        Sector randSector = null;
                        while (randSector == null || randSector is SectorCamp || randSector is SectorShore)
                        {
                            randSector = tribe.island.sectorList[Misc.rng.Next(0, tribe.island.sectorList.Count)];
                        }
                        ActionLookForFood a = new ActionLookForFood(randSector);
                        a.player = this;
                        a.Selected(gameRef);
                    }
                }
            }
        }
        public void SetVote(Game gameRef, List<Player> onlyEligible)
        {
            Player currentTarget = null;
            double currentTargetPoints = 0;
            List<Player> allianceTargets = new List<Player> { };
            foreach (Alliance a in alliances)
            {
                if(a.target!=null && !a.target.immune && (onlyEligible==null || onlyEligible.Contains(a.target))) allianceTargets.Add(a.target);
            }
            Dictionary<Player, int> targetCounter = new Dictionary<Player, int> { };
            foreach (Player t in allianceTargets)
            {
                //if (t == null) continue;
                if (targetCounter.ContainsKey(t))
                {
                    targetCounter[t]++;
                }
                else
                {
                    targetCounter.Add(t, 1);
                }
            }
            List<Player> tied = new List<Player> { };
            int number = 0;
            foreach (KeyValuePair<Player, int> kvp in targetCounter)
            {
                if (kvp.Value > number)
                {
                    number = kvp.Value;
                    tied.Clear();
                    tied.Add(kvp.Key);
                }
                else if (kvp.Value == number)
                {
                    tied.Add(kvp.Key);
                }
            }
            List<PlayerKnowledge> knowledgeToCheck = new List<PlayerKnowledge> { };
            if (tied.Count == 1)
            {
                vote = tied[0];
            }
            else
            {
                if (tied.Count > 1)
                {
                    if (onlyEligible == null) foreach (Player p in tied) knowledgeToCheck.Add(knowledge[p.id]);
                    else 
                    { 
                        vote = tied[Misc.rng.Next(0, tied.Count)];
                        return;
                    }
                }
                else
                {
                    knowledgeToCheck = knowledge;
                }
                foreach (PlayerKnowledge tk in knowledgeToCheck)
                {
                    Player p = (Player)gameRef.players[tk.id];
                    if (!tribe.members.Contains(p) || p.immune || p == this || (onlyEligible != null && !onlyEligible.Contains(p))) continue;
                    double targetPoints = 0;
                    foreach (string k in tk.relationships)
                    {
                        if (!k.Contains("?")) targetPoints += Convert.ToInt32(k);
                    }
                    targetPoints += p.correctVotes * 5;
                    targetPoints += p.compWins * 10;
                    targetPoints += tk.advantageSaves * 15;
                    targetPoints += tk.susLevel;
                    targetPoints += tk.pointsForTargetting;

                    tk.targetPoints = targetPoints;
                    if (targetPoints > currentTargetPoints)
                    {
                        currentTargetPoints = targetPoints;
                        currentTarget = (Player)gameRef.players[tk.id];

                    }
                }
                vote = currentTarget;
                if(vote == null)
                {
                    Console.Write("STOP");
                }
            }
        }
        public void ChooseFinalist(List<Selectable> allPlayers, List<Selectable> players)
        {
            Player currentTarget = null;
            double currentTargetPoints = 0;
            List<PlayerKnowledge> knowledgeToCheck = new List<PlayerKnowledge> { };
            foreach (Player p in players) knowledgeToCheck.Add(knowledge[p.id]);
            foreach (PlayerKnowledge tk in knowledgeToCheck)
            {
                Player p = (Player)allPlayers[tk.id];
                
                double targetPoints = 0;
                foreach (string k in tk.relationships)
                {
                    if (!k.Contains("?")) targetPoints += Convert.ToInt32(k);
                }
                targetPoints += p.correctVotes * 5;
                targetPoints += p.compWins * 10;
                targetPoints += tk.advantageSaves * 15;
                targetPoints += tk.susLevel;
                targetPoints += tk.pointsForTargetting;

                tk.targetPoints = targetPoints;
                if (currentTarget == null || targetPoints < currentTargetPoints)
                {
                    currentTargetPoints = targetPoints;
                    currentTarget = (Player)allPlayers[tk.id];
                }
            }
            vote = currentTarget;
            if (vote == null)
            {
                Console.Write("STOP");
            }
        }
        public override string ToString()
        {
            return name.Pastel(tribe.color);
        }

    }
}
