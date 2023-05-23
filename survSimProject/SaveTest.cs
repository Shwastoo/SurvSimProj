using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class SaveTest
    {
        public int id { get; set; }
        public string name { get; set; }

        public double endurance { get; set; }
        public double mental { get; set; }
        public double lying { get; set; }
        public double charisma { get; set; }

        public double Health { get; set; }
        public double Hunger { get; set; }

        public bool immune { get; set; }
        public bool starving { get; set; }
        public int hoursSinceEating { get; set; }
        public int firesMade { get; set; }
        public int compWins { get; set; }
        public int correctVotes { get; set; }

        public List<double> relationships { get; set; }
        public List<Selectable> inventory { get; set; }
        public List<Alliance> alliances { get; set; }

        public List<PlayerKnowledge> knowledge { get; set; }

        public Player vote { get; set; }

        public Tribe tribe { get; set; }
        public int sectorId { get; set; }
        public SaveTest(Player p)
        {
            id = p.id;
            name = p.name;
            endurance = p.endurance;
            mental = p.mental;
            lying = p.lying;
            charisma = p.charisma;
            Health = p.Health;
            Hunger = p.Hunger;
            immune = p.immune;
            starving = p.starving;
            hoursSinceEating = p.hoursSinceEating;
            firesMade = p.firesMade;
            compWins = p.compWins;
            correctVotes = p.correctVotes;
            relationships = p.relationships;
            inventory = p.inventory;
            alliances = p.alliances;
            knowledge = p.knowledge;
            vote = p.vote;
            tribe = p.tribe;
            sectorId = p.sectorId;
        }
    }
}
