using System;
using System.Collections.Generic;
using System.Text;

namespace travelling_thief_problem
{
    class Thief
    {
        Knapsack knapsack;
        double minSpeed;
        double maxSpeed;
        List<Path> paths;
        double fitness;
        double speed;
        public Thief(double minSpeed, double maxSpeed)
        {
            this.minSpeed = minSpeed;
            this.maxSpeed = maxSpeed;
            speed = maxSpeed;
            paths = new List<Path>();
        }

        public void AddPath(Path path)
        {
            paths.Add(path);
        }

        public void DisplayPaths()
        {
            Console.WriteLine("Thief paths");
            foreach(Path path in paths)
            {
                Console.WriteLine("From: ");
                path.getFrom().DisplayCoord();
                Console.WriteLine("To: ");
                path.getTo().DisplayCoord();
                Console.WriteLine();
            }
        }
    }
}
