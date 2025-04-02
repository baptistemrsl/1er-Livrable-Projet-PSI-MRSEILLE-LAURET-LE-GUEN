using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIVRABLE_2_PSI
{
    public class Station
    {
        public int Id { get; set; }
        public string Nom { get; set; } = "";
        public string Ligne { get; set; } = "";
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Commune { get; set; } = "";

        public override string ToString() => $"{Nom} (Ligne {Ligne})";
    }
}

