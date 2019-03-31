using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace travelling_thief_problem
{
    class Population
    {
        public int Number;
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

        public void Copy(Population other)
        {

            for(int i = 0; i < ThiefCount; i++)
            {
                thieves[i] = other.thieves[i];
            }
        }

        public PopulationStats GetStats()
        {
            PopulationStats stats = new PopulationStats();
            double worst = thieves[0].Fitness;
            double best = thieves[0].Fitness;
            double sum = 0;
            double average = thieves[0].Fitness;
            foreach(Thief thief in thieves)
            {
                sum += thief.Fitness;

                if(worst > thief.Fitness)
                {
                    worst = thief.Fitness;
                }
                if (best < thief.Fitness)
                {
                    best = thief.Fitness;
                }
            }

            average = sum / ThiefCount;
            stats.PopulationNumber = Number;
            stats.Best = best;
            stats.Average = average;
            stats.Worst = worst;

            return stats;
        }
    }
}
