using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Pastel;

namespace survSimProject
{
    class Tribe
    {
        public List<Selectable> members;
        public string name;
        public Color color;
        public Island island;
        public int id;
        public bool immune = false;

        public Tribe(string _name, Color _color, Island _island)
        {
            name = _name;
            color = _color;
            island = _island;
            members = new List<Selectable> { };
            id = island.id;
        }

        public override string ToString()
        {
            return name.Pastel(color);
        }
    }
}
