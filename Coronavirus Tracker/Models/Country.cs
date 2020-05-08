using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coronavirus_Tracker.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long Population { get; set; }
        public long Cases { get; set; }
        public long Deaths { get; set; }
        public int CFR { get; set; }

        public Country(int id, string name, long population, long cases, long deaths)
        {
            this.Id = id;
            this.Name = name;
            this.Population = population;
            this.Cases = cases;
            this.Deaths = deaths;
            SetStats();
        }

        public void SetStats()
        {
            long cfrPercentage = (100 / (Cases / Deaths));
            CFR = Convert.ToInt32(cfrPercentage);
        }

    }
}
