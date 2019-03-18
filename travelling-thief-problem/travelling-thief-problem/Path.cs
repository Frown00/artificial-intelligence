using System;
using System.Collections.Generic;
using System.Text;

namespace travelling_thief_problem
{
    class Path
    {
        City from;
        City to;
        public Path(City from, City to)
        {
            this.from = from;
            this.to = to;
        }

        public City getFrom()
        {
            return from;
        }

        public City getTo()
        {
            return to;
        }
    }
}
