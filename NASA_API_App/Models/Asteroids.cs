using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NASA_API_App.Models
{
    public class Asteroids
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double EstimatedDiameter { get; set; }
        public double Speed { get; set; }
        public bool PotentiallyHazardous { get; set; }

        public string PotentiallyHazardousTranslated => PotentiallyHazardous ? "Ano" : "Ne";
        public double TimeAroundEarth { get; set; }
    }
}
