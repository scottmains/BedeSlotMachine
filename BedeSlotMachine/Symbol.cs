using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedeSlotMachine
{
    class Symbol
    {
        public string Name { get; set; }
        public decimal Coefficient { get; set; }
        public double Probability { get; set; }

        public Symbol(string name, decimal coefficient, double probability)
        {
            Name = name;
            Coefficient = coefficient;
            Probability = probability;
        }
    }
}
