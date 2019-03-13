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

        public Thief(double minSpeed, double maxSpeed)
        {
            this.minSpeed = minSpeed;
            this.maxSpeed = maxSpeed;
        }
    }
}
