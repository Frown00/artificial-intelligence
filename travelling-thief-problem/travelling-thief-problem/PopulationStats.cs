using System;
using System.Collections.Generic;
using System.Text;

namespace travelling_thief_problem
{
    class PopulationStats
    {
        public int PopulationNumber { get; set; }
        public double Best { get; set; }
        public double Average { get; set; }
        public double Worst { get; set; }

        

        public override string ToString()
        {
            return $"PopNum: {PopulationNumber} Best: {Best}\tAverage: {Average}\tWorst: {Worst}";
        }
    }
}
