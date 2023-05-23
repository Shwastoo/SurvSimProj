using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Pastel;
using Newtonsoft.Json;

namespace survSimProject
{
    class Alliance:Selectable
    {
        public int id;
        public string name;
        private Color color;
        public List<Selectable> members;
        public List<double> loyalty;
        [JsonIgnore]
        public Player target = null;
        public int targetId = -1;

        public Alliance(int _id)
        {
            id = _id;
            name = "Alliance " + id;
            members = new List<Selectable> { };
            loyalty = new List<double> { };
            color = Color.FromArgb(Misc.rng.Next(20, 256), Misc.rng.Next(20, 256), Misc.rng.Next(20, 256));
        }

        public override string ToString()
        {
            return name.Pastel(color);
        }
    }
}
