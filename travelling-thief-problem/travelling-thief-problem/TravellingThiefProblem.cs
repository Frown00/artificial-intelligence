using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace travelling_thief_problem
{
    class TravellingThiefProblem
    {
        string problemName;
        string knapsackDataType;
        int dimension;
        int itemsNum;
        int knapsackCapacity;
        double minSpeed;
        double maxSpeed;
        double rentingRatio;
        string edgeWeigthType;


        List<Item> items;
        List<City> cities;
        List<Population> populations;

        int popSize = 10;
        int gen = 100;
        double crossingProb = 0.5;
        double mutationProb = 0.5;
        int tour = 0;

        public TravellingThiefProblem(List<string> problemSetup)
        {
            items = new List<Item>();
            cities = new List<City>();
            populations = new List<Population>();
            MapToObject(problemSetup);
            AddItemsToCities();

            Population firstPopulation = GeneratePopulation();
            populations.Add(firstPopulation);
            TournamentSelection();
            //Console.WriteLine(cities[0].CalcDistanceTo(cities[1]));
            //foreach(City city in cities)
            //{
            //    Console.WriteLine(city);
            //    city.DisplayAllItems();
            //    Console.WriteLine("\n");
            //}
        }


        private Population GeneratePopulation()
        {

            Population population = new Population();

            for(int i = 0; i < popSize; i++)
            {
                Random random = new Random();

                List<int> cityIndeces = new List<int>();
                int fromId = 0;
                int toId;

                for (int j = 1; j < cities.Count; j++)
                {
                    cityIndeces.Add(j);
                }


                Thief thief = new Thief(minSpeed, maxSpeed, knapsackCapacity);

                while (cityIndeces.Count > 1)
                {
                    int citiesNum = cityIndeces.Count;
                    int randomId = random.Next(citiesNum);
                    toId = cityIndeces[randomId];
                    Path path = new Path(cities[fromId], cities[toId]);
                    thief.AddPath(path);
                    thief.CountTravelTime(path);
                    thief.PutBestItemIntoKnapsack(cities[toId]);
                    thief.CountFitness();

                    fromId = toId;
                    cityIndeces.RemoveAt(randomId);
                }
                population.AddThief(thief);
                
            }
            //population.GetThieves()[0].DisplayItems();
            //foreach (City city in population.GetThieves()[0].VisitedCities())
            //{
            //    Console.Write(city.GetIndex() + "->");
            //}
            //Console.WriteLine("\n" + population.GetThieves()[0].TravelTime);
            //Console.WriteLine(population.GetThieves()[0].TotalProfit);
            //Console.WriteLine(population.GetThieves()[0].Fitness);

            //population.Display();

            return population;
        }

        private void TournamentSelection()
        {
            Random random = new Random();
            Population newPopulation = new Population();
            Population currentPopulation = populations[populations.Count-1];

            List<int> thiefIndeces = new List<int>();
            int idx = 0;

            for (int j = 0; j < currentPopulation.GetThieves().Count; j++)
            {
                thiefIndeces.Add(j);
            }
            while (newPopulation.ThiefCount < currentPopulation.ThiefCount)
            {
                

                List<int> allCompetitors = new List<int>(thiefIndeces);
                List<int> competitors = new List<int>();
                if(allCompetitors.Count > tour && tour > 0)
                {
                    for (int i = 0; i < tour; i++)
                    {
                        int thiefNum = allCompetitors.Count;
                        int randomId = random.Next(thiefNum);
                        competitors.Add(randomId);
                        allCompetitors.RemoveAt(randomId);
                    }
                } else
                {
                    competitors = new List<int>(allCompetitors);
                }

                int bestThiefId = FindBestThief(competitors);
                Thief bestThief = currentPopulation.GetThieves()[bestThiefId];
                newPopulation.AddThief(bestThief);
            }
            Console.WriteLine("Old population");
            currentPopulation.Display();
            Console.WriteLine("New population");
            newPopulation.Display();

            populations.Add(newPopulation);

        }

        private int FindBestThief(List<int> competitors)
        {
            int bestThiefId = competitors[0];

            Population currentPopulation = populations[populations.Count - 1];
            Thief bestThief = currentPopulation.GetThieves()[bestThiefId];

            int i = 0;
            foreach (int competitorId in competitors)
            {
                Thief competitor = currentPopulation.GetThieves()[competitorId];
                bestThief = currentPopulation.GetThieves()[bestThiefId];

                if ( competitor.Fitness > bestThief.Fitness )
                {
                    bestThiefId = competitorId;
                }
                i++;
            }

            return bestThiefId;
        }

        private void AddItemsToCities()
        {
            int id;
            foreach(Item item in items)
            {
                id = item.GetAssignedCityId();
                cities[id-1].AddItem(item);
            }
        }

        private void MapToObject(List<string> problemSetup)
        {

            //this.problemName = problemSetup[0].Split(" ")[1];
            int i = 1;
            bool isNodeCoord = false;
            bool isItemsSection = false;
            const string NODE_START_TEXT = "NODE_COORD_SECTION";
            const string ITEM_START_TEXT = "ITEMS SECTION";

            foreach (string p in problemSetup)
            {
                string[] elements = p.Split("\t");

                // Basic setup
                if (i == 1)
                {
                    problemName = elements[1];
                }
                else if (i == 2)
                {
                    knapsackDataType = elements[0].Split(":")[1];
                }
                else if (i == 3)
                {
                    int number;
                    bool success = int.TryParse(elements[1], out number);
                    if (success)
                    {
                        dimension = number;
                    }
                    else
                    {
                        Console.WriteLine("Error while parsing string to int");
                    }
                }
                else if (i == 4)
                {
                    int number;
                    bool success = int.TryParse(elements[1], out number);
                    if (success)
                    {
                        itemsNum = number;
                    }
                    else
                    {
                        Console.WriteLine("Error while parsing string to int");
                    }
                }
                else if (i == 5)
                {
                    int number;
                    bool success = int.TryParse(elements[1], out number);
                    if (success)
                    {
                        knapsackCapacity = number;
                    }
                    else
                    {
                        Console.WriteLine("Error while parsing string to int");
                    }
                }
                else if (i == 6)
                {
                    double number;
                    bool success = double.TryParse(elements[1], out number);
                    if (success)
                    {
                        minSpeed = number;
                    }
                    else
                    {
                        Console.WriteLine("Error while parsing string to double");
                    }
                }
                else if (i == 7)
                {
                    double number;
                    bool success = double.TryParse(elements[1], out number);
                    if (success)
                    {
                        maxSpeed = number;
                    }
                    else
                    {
                        Console.WriteLine("Error while parsing string to double");
                    }
                }
                else if (i == 8)
                {
                    double number;
                    bool success = double.TryParse(elements[1], out number);
                    if (success)
                    {
                        rentingRatio = number;
                    }
                    else
                    {
                        Console.WriteLine("Error while parsing string to double");
                    }
                }
                else if (i == 9)
                {
                    edgeWeigthType = elements[1];
                }

                //// map cities coordinates ////
                if (isNodeCoord)
                {
                    if (elements[0] == ITEM_START_TEXT)
                    {
                        isNodeCoord = false;
                    }
                    else
                    {

                        int id;
                        double coordX;
                        double coordY;
                        bool successParseId = int.TryParse(elements[0], out id);
                        bool successParseCoordX = double.TryParse(elements[1], out coordX);
                        bool successParseCoordY = double.TryParse(elements[2], out coordY);

                        if (successParseId && successParseCoordX && successParseCoordY)
                        {
                            City city = new City(id, coordX, coordY);
                            cities.Add(city);
                        }
                        else
                        {
                            Console.WriteLine("Error while parsing data needs to city object");
                        }

                    }
                }

                if (elements[0] == NODE_START_TEXT)
                {
                    isNodeCoord = true;
                }

                //// map items properties ////
                if (isItemsSection)
                {
                    if (elements[0] == NODE_START_TEXT)
                    {
                        isItemsSection = false;
                    }
                    else
                    {
                        int id;
                        int profit;
                        int weight;
                        int assignedToCityId;

                        bool successParseId = int.TryParse(elements[0], out id);
                        bool successParseProfit = int.TryParse(elements[1], out profit);
                        bool successParseWeight = int.TryParse(elements[2], out weight);
                        bool successParseAssignedToCityId = int.TryParse(elements[3], out assignedToCityId);

                        if (successParseId && successParseProfit && successParseWeight && successParseAssignedToCityId)
                        {
                            Item item = new Item(id, profit, weight, assignedToCityId);
                            items.Add(item);
                        }
                        else
                        {
                            Console.WriteLine("Error while parsing data needs to item object");
                        }

                    }
                }

                if (elements[0] == ITEM_START_TEXT)
                {
                    isItemsSection = true;
                }

                i++;
            }

        }
    }
}
