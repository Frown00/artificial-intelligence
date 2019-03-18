namespace travelling_thief_problem
{
    public class Item
    {
        int index;
        int profit;
        int weight;
        int assignedCityId;

        public Item(int index, int profit, int weight, int assignedCityId)
        {
            this.index = index;
            this.profit = profit;
            this.weight = weight;
            this.assignedCityId = assignedCityId;
        }

        public int GetAssignedCityId()
        {
            return assignedCityId;
        }

        public override string ToString()
        {
            return $"Id: {index}\tProfit: {profit}\tWeigth: {weight}\t Assign: {assignedCityId}";
        }
    }
}