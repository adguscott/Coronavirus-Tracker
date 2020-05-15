using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace Coronavirus_Tracker.Models
{
    public class DetailedCountryModel : CountryModel
    {
        public long Population { get; set; }
        public long Cases { get; set; }
        public long Deaths { get; set; }

        public int LatestCases { get; set; }
        public int LatestDeaths { get; set; }
        public string CountryCode { get; set; }
        public string FlagUri { get; set; }
        public string CFR { get; set; }
        public string Infected { get; set; }
        public Dictionary<DateTime, int> TimelineCases { get; set; }
        public Dictionary<DateTime, int> TimelineDeaths { get; set; }

        public DateTime LastUpdated { get; set; }

        public DetailedCountryModel(int id, string name, string countryCode, long population, long cases,
            long deaths, DateTime lastUpdated, int latestCases, int latestDeaths )
        {
            this.Id = id;
            this.Name = name;
            this.CountryCode = countryCode;
            this.Population = population;
            this.Cases = cases;
            this.Deaths = deaths;
            this.LastUpdated = lastUpdated;
            this.FlagUri = $"/flags/{countryCode.ToLower()}.png";
            LatestCases = latestCases;
            LatestDeaths = latestDeaths;
            SetPercentages();
        }

        public void SetPercentages()
        {
            var cfr = ((float)Deaths / (float)Cases) * 100;
            var inf = ((float)Cases / (float)Population) * 100;
            CFR = $"{cfr:n}%";
            Infected = $"{inf:n}%";
        }

    }
}
