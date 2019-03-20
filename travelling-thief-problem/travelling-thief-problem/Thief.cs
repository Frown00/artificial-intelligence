using System;
using System.Collections.Generic;
using System.Text;

namespace travelling_thief_problem
{
    class Thief
    {
        Knapsack knapsack;
        public double MinSpeed;
        public double MaxSpeed;
        public List<Path> Paths;
        double speed;
        public int MaxCapacity;

        public double TravelTime = 0;
        public double TotalProfit = 0;
        public double Fitness;
        public Thief(double minSpeed, double maxSpeed, int knapsackCapacity)
        {
            this.MinSpeed = minSpeed;
            this.MaxSpeed = maxSpeed;
            speed = maxSpeed;
            MaxCapacity = knapsackCapacity;
            Paths = new List<Path>();
            knapsack = new Knapsack(knapsackCapacity);
        }

        public void ResetTravel()
        {
            TravelTime = 0;
            TotalProfit = 0;
            knapsack.RemoveAll();
            speed = MaxSpeed;
            
        }
        public void AddPath(Path path)
        {
            Paths.Add(path);
        }

        public void DisplayPaths()
        {
            Console.WriteLine("Thief paths");
            foreach(Path path in Paths)
            {
                Console.WriteLine("From: ");
                path.getFrom().DisplayCoord();
                Console.WriteLine("To: ");
                path.getTo().DisplayCoord();
                Console.WriteLine();
            }
        }

        // Take the best item ( profit / weight ) from city
        public void PutBestItemIntoKnapsack(City city)
        {
            double profitability = 0;
            int bestItemId = -1;
            int i = 0;
            foreach (Item item in city.GetItems())
            {
                double prof = item.Profit / item.Weight;
                if(prof > profitability && knapsack.Capacity >= item.Weight)
                {
                    profitability = prof;
                    bestItemId = i;
                }
                i++;
            }

            if(bestItemId != -1)
            {
                Item bestItem = city.GetItems()[bestItemId];
                knapsack.AddItem(bestItem);
                CountTotalProfit(bestItem.Profit);
                SetSpeed();
            }
        }

        public void DisplayItems()
        {
            Console.WriteLine(knapsack.Capacity);
            knapsack.DisplayItems();
        }


        public void CountTravelTime(Path path)
        {
            TravelTime += path.GetLength() / speed;
        }

        private void CountTotalProfit(double profit)
        {
            TotalProfit += profit;
        }

        public void CountFitness()
        {
            Fitness = TotalProfit - TravelTime;
        }

        private void SetSpeed()
        {
            speed = MaxSpeed - ((MaxCapacity - knapsack.Capacity) * ((MaxSpeed - MinSpeed) / MaxCapacity));
        }

        public List<City> VisitedCities()
        {
            List<City> visited = new List<City>();
            foreach(Path path in Paths)
            {
                visited.Add(path.getFrom());
            }

            return visited;
        }

        public void MapToPaths(List<City> visistedCities)
        {
            for(int i = 0; i < visistedCities.Count-1; i++)
            {
                this.AddPath(new Path(visistedCities[i], visistedCities[i + 1]));
            }
            this.AddPath(new Path(visistedCities[visistedCities.Count - 1], visistedCities[0]));
        }
    }
}
