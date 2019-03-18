using System;
using System.Collections.Generic;
using System.Text;

namespace travelling_thief_problem
{
    class Population
    {
        List<Thief> thieves;

        public Population()
        {
            thieves = new List<Thief>();
        }

        public void AddThief(Thief thief)
        {
            thieves.Add(thief);
        }

        public void Display()
        {
            foreach(Thief thief in thieves)
            {
                Console.WriteLine("\n");
                thief.DisplayPaths();
            }
        }
    }
}
