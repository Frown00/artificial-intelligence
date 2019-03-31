using CsvHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;

namespace travelling_thief_problem
{
    class TravellingThiefProblem
    {
        //// PARAMETERS ////
        int popSize = 100;
        int gen = 100;
        double crossingProb = 0.7;
        double mutationProb = 0.1;
        int tour = 5;

        int testNum = 10;


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
        List<PopulationStats> populationsStats;
        List<List<PopulationStats>> tests;

       
        public TravellingThiefProblem(List<string> problemSetup)
        {
            items = new List<Item>();
            cities = new List<City>();
            populationsStats = new List<PopulationStats>();
            MapToObject(problemSetup);
            AddItemsToCities();
            // . -> ,
            CultureInfo ci = new CultureInfo("pl-PL");
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            tests = new List<List<PopulationStats>>();
            for (int i = 0; i < testNum; i++)
            {
                populationsStats.Clear();
                populationsStats = new List<PopulationStats>();
                Run();
                Console.WriteLine($"Test {i + 1} has finished");
                
                tests.Add(new List<PopulationStats>(populationsStats));
            }
            
            ExtractDataToCSV();

            Console.WriteLine("Done!");
            Console.WriteLine("Data was extracted to csv file");
            Process.Start("explorer.exe", System.IO.Directory.GetCurrentDirectory());
        }

        public void Run()
        {
            Console.WriteLine("Evaluating...");
            int popNum = 1;

            Population currentPopulation = GeneratePopulation();
            currentPopulation.Number = popNum;

            PopulationStats popStats = new PopulationStats();
            SaveStats(currentPopulation);

            Population newPopulation = new Population();

            for (int i = 1; i < gen; i++)
            {
                Console.Write("\r" + "Gen: " + i + "\r");
                popNum++;
                newPopulation = TournamentSelection(currentPopulation);

                if (IsPerform(crossingProb))
                {
                    Crossing(newPopulation);
                }
                Mutation(newPopulation);

                Evaluate(newPopulation);

                newPopulation.Number = popNum;
                SaveStats(newPopulation);
                currentPopulation.Copy(newPopulation);
                newPopulation = null;
            }
          

        }

        private void SaveStats(Population pop)
        {
            PopulationStats popStats = new PopulationStats();
            popStats.PopulationNumber = pop.Number;
            popStats.Best = pop.GetStats().Best;
            popStats.Average = pop.GetStats().Average;
            popStats.Worst = pop.GetStats().Worst;

            populationsStats.Add(popStats);
        }

        private void ExtractDataToCSV()
        {

            //// Preparing records //// 
            List<PopulationStats> records = new List<PopulationStats>();
            double testDeviations = 0;


            for (int i = 0; i < gen; i++)
            {
                double bestSum = 0;
                double averageSum = 0;
                double worstSum = 0;
                

                for (int j = 0; j < tests.Count; j++)
                {
                    bestSum += tests[j][i].Best;
                    averageSum += tests[j][i].Average;
                    worstSum += tests[j][i].Worst;
                }
                double bestAvg = bestSum / tests.Count;
                double averageAvg = averageSum / tests.Count;
                double worstAvg = worstSum / tests.Count;


                PopulationStats populationStatsAvg = new PopulationStats();
                populationStatsAvg.PopulationNumber = i + 1;
                populationStatsAvg.Best = bestAvg;
                populationStatsAvg.Average = averageAvg;
                populationStatsAvg.Worst = worstAvg;

                

                records.Add(populationStatsAvg);
            }

            
            double testAverageAvg = 0;

            for (int t = 0; t < tests.Count; t++)
            {
                double avg = 0;
                double sum = 0;

                for (int p = 0; p < gen; p++)
                {
                    sum += tests[t][p].Average;
                }
                avg = sum / popSize;
                testAverageAvg += avg;
            }
            testAverageAvg = testAverageAvg / tests.Count;



            for (int t = 0; t < tests.Count; t++)
            {
                double avg = 0;
                double sum = 0;

                for (int p = 0; p < gen; p++)
                {
                    sum += tests[t][p].Average;
                }
                avg = sum / popSize;
                testDeviations += Math.Pow((avg - testAverageAvg), 2);
            }
            testDeviations = Math.Sqrt(testDeviations / tests.Count);

           


            // Wrting data to csv
            using (var writer = new StreamWriter("ttp-stats.csv"))
            {
                writer.WriteLine("sep=;");
                writer.WriteLine($"Population size: {popSize}");
                writer.WriteLine($"Generation amount: {gen}");
                writer.WriteLine($"Crossing probability: {crossingProb}");
                writer.WriteLine($"Mutation probability: {mutationProb}");
                writer.WriteLine($"Tournament parameter: {tour}");
                writer.WriteLine("\n");
                writer.WriteLine($"Deviation : {testDeviations}");
                using (var csv = new CsvWriter(writer))
                {
                    csv.Configuration.Delimiter = ";";
                    csv.WriteRecords(records);
                }
            }
                
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

                while (cityIndeces.Count > 0)
                {
                    int citiesNum = cityIndeces.Count;
                    int randomId = random.Next(citiesNum);
                    toId = cityIndeces[randomId];
                    Path path = new Path(cities[fromId], cities[toId]);
                    thief.AddPath(path);
                    thief.AddToTravelTime(path);
                    thief.PutBestItemIntoKnapsack(cities[toId]);
                    thief.CountFitness();

                    fromId = toId;
                    cityIndeces.RemoveAt(randomId);
                }
                Path returnPath = new Path(thief.Paths[thief.Paths.Count - 1].getTo(), cities[0]);
                thief.AddPath(returnPath);
                thief.AddToTravelTime(returnPath);
                thief.CountFitness();

                population.AddThief(thief);
                
            }
            

            return population;
        }

        private Population TournamentSelection(Population pop)
        {
            Random random = new Random();
            Population newPopulation = new Population();
            Population currentPopulation = pop;

            List<int> thiefIndeces = new List<int>();

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

                int bestThiefId = FindBestThief(competitors, currentPopulation);
                Thief bestThief = currentPopulation.GetThieves()[bestThiefId];
                Thief newBestThief = new Thief(minSpeed, maxSpeed, knapsackCapacity);
                newBestThief.MapToPaths(bestThief.VisitedCities());
                newPopulation.AddThief(newBestThief);
            }

            return newPopulation;

        }

        private int FindBestThief(List<int> competitors, Population population)
        {
            int bestThiefId = competitors[0];

            Thief bestThief = population.GetThieves()[bestThiefId];

            int i = 0;
            foreach (int competitorId in competitors)
            {
                Thief competitor = population.GetThieves()[competitorId];
                bestThief = population.GetThieves()[bestThiefId];

                if ( competitor.Fitness > bestThief.Fitness )
                {
                    bestThiefId = competitorId;
                }
                i++;
            }

            return bestThiefId;
        }

        //// CROSSING ////
        private void Crossing(Population pop)
        {

            Population currentPopulation = pop;
            Population afterCrossing = new Population();
            Random random = new Random();

            for (int k = 0; k < currentPopulation.ThiefCount; k++)
            {
                // choose random parents
                int firstParentId = random.Next(currentPopulation.ThiefCount);
                int secondParentId = firstParentId;


                while (secondParentId == firstParentId)
                {
                    secondParentId = random.Next(currentPopulation.ThiefCount);
                }
                Thief parent1 = currentPopulation.GetThieves()[firstParentId];
                Thief parent2 = currentPopulation.GetThieves()[secondParentId];


                int allelesLength = parent1.VisitedCities().Count / 2;
                int startAlleles = random.Next(parent1.VisitedCities().Count / 2);
                Thief child1 = new Thief(parent1.MinSpeed, parent1.MaxSpeed, parent1.MaxCapacity);
                Thief child2 = new Thief(parent2.MinSpeed, parent2.MaxSpeed, parent2.MaxCapacity);

                List<City> childVisitedCities1 = new List<City>();
                List<City> childVisitedCities2 = new List<City>();

                List<City> tempParentCities1 = new List<City>(parent1.VisitedCities());
                List<City> tempParentCities2 = new List<City>(parent2.VisitedCities());

                // initate childs
                for (int i = 0; i < parent1.VisitedCities().Count; i++)
                {
                    childVisitedCities1.Add(new City(-1, -1, -1));
                    childVisitedCities2.Add(new City(-1, -1, -1));
                }

                // choose alleles
                for (int i = startAlleles; i < startAlleles + allelesLength; i++)
                {
                    childVisitedCities1[i] = parent1.VisitedCities()[i];
                    int idToRemove1 = FindCityToRemove(tempParentCities2, childVisitedCities1[i].Index);
                    tempParentCities2.RemoveAt(idToRemove1);
                    childVisitedCities2[i] = parent2.VisitedCities()[i];
                    int idToRemove2 = FindCityToRemove(tempParentCities1, childVisitedCities2[i].Index);
                    tempParentCities1.RemoveAt(idToRemove2);

                }

                // Assign alleles from second parent
                for (int i = 0; i < childVisitedCities1.Count; i++)
                {
                    if (childVisitedCities1[i].Index == -1)
                    {
                        childVisitedCities1[i] = tempParentCities2[0];
                        tempParentCities2.RemoveAt(0);
                    }

                    if (childVisitedCities2[i].Index == -1)
                    {
                        childVisitedCities2[i] = tempParentCities1[0];
                        tempParentCities1.RemoveAt(0);
                    }
                }

                child1.MapToPaths(childVisitedCities1);
                child2.MapToPaths(childVisitedCities2);

                afterCrossing.AddThief(child1);
                afterCrossing.AddThief(child2);
            }
               
            currentPopulation.Copy(afterCrossing);


        }

        private int FindCityToRemove(List<City> cities, int cityId)
        {
            int toRemove = -1;
            for(int i = 0; i < cities.Count; i++)
            {
                if(cities[i].Index == cityId)
                {
                    toRemove = i;
                    i = cities.Count;
                }
            }
            return toRemove;
        }


        //// MUTATION ////
        private void Mutation(Population currentPopulation)
        {
            Random random = new Random();
            foreach(Thief thief in currentPopulation.GetThieves())
            {
                if(IsPerform(mutationProb))
                {
                    int firstCity = random.Next(1, thief.VisitedCities().Count);
                    int secondCity = firstCity;

                    while (secondCity == firstCity)
                    {
                        secondCity = random.Next(1, thief.VisitedCities().Count);
                    }

                    List<City> cities = thief.VisitedCities();
                    List<City> citiesAfterMutation = new List<City>((List<City>)Swap(cities, firstCity, secondCity));

                   

                    thief.MapToPaths(citiesAfterMutation);

                }

            }

        }


        public static IList<T> Swap<T>(IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
            return list;
        }

        private bool IsPerform(double probability)
        {
            Random random = new Random();
            return random.Next(1) < probability;
        }

        //// EVALUATE ////
        private void Evaluate(Population pop)
        {
            foreach(Thief thief in pop.GetThieves())
            {
                thief.ResetTravel();
                for(int i = 0; i < thief.Paths.Count - 1; i++)
                {
                    thief.AddToTravelTime(thief.Paths[i]);
                    thief.PutBestItemIntoKnapsack(thief.Paths[i].getTo());
                }
                thief.CountFitness();
            }
        }

        

        //// LOADER FUNCTIONS ////
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
