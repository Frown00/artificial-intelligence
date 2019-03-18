using System;
using System.Collections.Generic;

namespace travelling_thief_problem
{
    public class Knapsack
    {
        public int Capacity;
        List<Item> itemsIn;
        
        public Knapsack(int capacity)
        {
            this.Capacity = capacity;
            itemsIn = new List<Item>();
        }

        public void AddItem(Item item)
        {
            itemsIn.Add(item);
            Capacity -= item.Weight;
        }

        public void DisplayItems()
        {
            foreach(Item item in itemsIn)
            {
                Console.WriteLine(item);
            }
        }
    }
}