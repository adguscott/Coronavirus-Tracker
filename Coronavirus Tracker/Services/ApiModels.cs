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
            public List<LocationModel> Locations { get; set; }
        }

        public class CountryOverviewModel
        {
            public SingleLocationModel Location;
        }
        public class LocationModel
        {
            public int Id { get; set; }
            public string Country { get; set; }
            public string Country_Code { get; set; }
            public Nullable<long> Country_Population { get; set; }
            public DateTime Last_Updated { get; set; }
            public string Province { get; set; }
            public string County { get; set; }
            public StatsModel Latest;
        }

        public class SingleLocationModel
        {
            public int Id { get; set; }
            public string Country { get; set; }
            public string Country_Code { get; set; }
            public long Country_Population { get; set; }
            public DateTime Last_Updated { get; set; }

            public string County { get; set; }
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
