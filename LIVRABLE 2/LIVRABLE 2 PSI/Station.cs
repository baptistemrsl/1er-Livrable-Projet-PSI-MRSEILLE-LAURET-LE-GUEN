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
        public string Nom { get; set; } = string.Empty;
        public string Ligne { get; set; } = string.Empty;
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Commune { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Nom} (Ligne {Ligne})";
        }

        public override bool Equals(object? obj)
        {
            return obj is Station station &&
                   Id == station.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
