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
        List<City> visitedCities;
        double speed;
        public int MaxCapacity;

        public double TravelTime = 0;
        public double TotalProfit = 0;
        public double Fitness = 0;
        public Thief(double minSpeed, double maxSpeed, int knapsackCapacity)
        {
            this.MinSpeed = minSpeed;
            this.MaxSpeed = maxSpeed;
            speed = maxSpeed;
            MaxCapacity = knapsackCapacity;
            Paths = new List<Path>();
            visitedCities = new List<City>();
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
            visitedCities.Add(path.getFrom());
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
                AddToTotalProfit(bestItem.Profit);
                SetSpeed();
            }
        }

        public void DisplayItems()
        {
            Console.WriteLine(knapsack.Capacity);
            knapsack.DisplayItems();
        }


        public void AddToTravelTime(Path path)
        {
            TravelTime += path.GetLength() / speed;
        }

        private void AddToTotalProfit(double profit)
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
            return visitedCities;
        }

        public void MapToPaths(List<City> visisted)
        {
            Paths.Clear();
            visitedCities.Clear();
            for(int i = 0; i < visisted.Count-1; i++)
            {
                this.AddPath(new Path(visisted[i], visisted[i + 1]));
            }
            this.AddPath(new Path(visisted[visisted.Count - 1], visisted[0]));
        }
    }
}
