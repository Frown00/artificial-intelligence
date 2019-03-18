using System;
using System.Collections.Generic;

namespace travelling_thief_problem
{
    public class City
    {
        int index;
        double coordX;
        double coordY;
        List<Item> itemsInCity;

        public City(int index, double coordX, double coordY)
        {
            this.index = index;
            this.coordX = coordX;
            this.coordY = coordY;
            itemsInCity = new List<Item>();
        }

        public int GetIndex()
        {
            return index;
        }
        public void AddItem(Item item)
        {
            itemsInCity.Add(item);
        }

        public void RemoveItem(Item item)
        {
            itemsInCity.Remove(item);
        }

        public void DisplayAllItems()
        {
            Console.WriteLine("Items in city");
            foreach (Item item in itemsInCity)
            {
                Console.WriteLine(item);
            }
        }

        public void DisplayCoord()
        {
            Console.WriteLine($"City nr: {index}\t coord_x: {coordX}\t coord_y: {coordY}");
        }

        public double CalcDistanceTo(City city)
        {
            double dist = Math.Sqrt(Math.Pow(city.coordX - coordX, 2) + Math.Pow(city.coordY - coordY, 2));
            return dist;
        }

        public List<Item> GetItems()
        {
            return itemsInCity;
        }

        public override string ToString()
        {
            return $"City index: {index}\tcoord_x: {coordX}\tcoord_y: {coordY}";
        }
    }
}