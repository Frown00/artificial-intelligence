﻿using System;
using System.Collections.Generic;
using System.Text;

namespace travelling_thief_problem
{
    class Population
    {
        public int ThiefCount;
        List<Thief> thieves;

        public Population()
        {
            thieves = new List<Thief>();
        }

        public void AddThief(Thief thief)
        {
            thieves.Add(thief);
            ThiefCount += 1;
        }

        public void Display()
        {
            int i = 1;
            foreach(Thief thief in thieves)
            {
                //thief.DisplayPaths();
                Console.WriteLine($"{i}. Fitness={thief.Fitness}");
                i++;
            }
        }

        public List<Thief> GetThieves()
        {
            return thieves;
        }
    }
}
