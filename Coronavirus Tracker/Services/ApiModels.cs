using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coronavirus_Tracker.Services
{
    public class ApiModels
    {
        public class CoronavirusTrackerApiModel
        {
            public StatsModel Latest { get; set; }
            public List<BasicLocationModel> Locations { get; set; }
        }

        public class CountryOverviewModel
        {
            public SingleLocationModel Location;
        }
        public class BasicLocationModel
        {
            public int Id { get; set; }
            public string Country { get; set; }
            public string Province { get; set; }

        }

        public class Australia
        {
            public StatsModel Latest;
            public List<SingleLocationModel> Locations;
        }

        public class SingleLocationModel
        {
            public int Id { get; set; }
            public string Country { get; set; }
            public string Country_Code { get; set; }
            public string Province { get; set; }
            public long Country_Population { get; set; }
            public DateTime Last_Updated { get; set; }

            public StatsModel Latest;
            public TimelineOuterModel Timelines;
        }

        public class StatsModel
        {
            public long Confirmed { get; set; }
            public long Deaths { get; set; }
            public long Recovered { get; set; }
        }

        public class TimelineOuterModel
        {
            public TimelineInnerModel Confirmed;
            public TimelineInnerModel Deaths;
            public TimelineInnerModel Recoveries;
        }

        public class TimelineInnerModel
        {
            public int Latest;
            public Dictionary<DateTime, int> Timeline;
        }
    }
}
