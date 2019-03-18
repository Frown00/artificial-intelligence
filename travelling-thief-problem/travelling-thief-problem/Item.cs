namespace travelling_thief_problem
{
    public class Item
    {
        int index;
        public int Profit;
        public int Weight;
        int assignedCityId;

        public Item(int index, int profit, int weight, int assignedCityId)
        {
            this.index = index;
            this.Profit = profit;
            this.Weight = weight;
            this.assignedCityId = assignedCityId;
        }

        public int GetAssignedCityId()
        {
            return assignedCityId;
        }

        public override string ToString()
        {
            return $"Id: {index}\tProfit: {Profit}\tWeigth: {Weight}\t Assign: {assignedCityId}";
        }
    }
}