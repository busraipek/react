using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2.Models
{
    public class PredictionInput
    {
        public float Time { get; set; }
        public string Airline { get; set; }
        public string Destination { get; set; }
    }
}
