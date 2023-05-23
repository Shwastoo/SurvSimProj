using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class PlayerKnowledge
    {
        public int id;

        public List<string> relationships = new List<string> { };
        public List<Item> inventory = new List<Item> { };
        [JsonIgnore]
        public List<Alliance> alliances = new List<Alliance> { };
        public List<int> alliancesId = new List<int> { };
        public double susLevel = 1;
        public int compWins = 0;
        public int correctVotes = 0;
        public int advantageSaves = 0;
        [JsonIgnore]
        public Player biggestTarget = null;
        public int biggestTargetId = -1;
        public int pointsForTargetting = 0;

        public double targetPoints = 0;

        public PlayerKnowledge(int _id)
        {
            id = _id;
            for(int i = 0; i < GlobSettings.playerCount; i++)
            {
                relationships.Add("?");
            }

        }

    }
}
