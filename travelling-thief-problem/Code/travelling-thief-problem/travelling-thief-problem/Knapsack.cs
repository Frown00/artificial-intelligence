using System.Collections.Generic;

namespace travelling_thief_problem
{
    public class Knapsack
    {
        int capacity;
        List<Item> itemsIn;

        public Knapsack(int capacity)
        {
            this.capacity = capacity;
            itemsIn = new List<Item>();
        }
    }
}