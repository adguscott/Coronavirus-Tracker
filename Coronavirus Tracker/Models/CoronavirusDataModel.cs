using Coronavirus_Tracker.Services;
using Coronavirus_Tracker.ViewModels;
using CoronavirusTrackerAPI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using static Coronavirus_Tracker.Services.ApiModels;

namespace Coronavirus_Tracker.Models
{
    public class CoronavirusDataModel
    {
        public CoronavirusTrackerApiModel ApiModel { get; set; }
        public CountryOverviewModel CountryOverview { get; set; }

        public bool IsSuccessful { get; set; }

        private static string Url = "v2/locations?source=jhu";

        public CoronavirusDataModel()
        {
            ApiHelper.InitializeClient();
            ApiHelper.ApiClient.BaseAddress = new Uri("https://coronavirus-tracker-api.herokuapp.com/");
        }
        public async Task GetApiModel()
        {
            try
            {
                ApiModel = await ApiHelper.Read<CoronavirusTrackerApiModel>(Url);
                IsSuccessful = true;
            }
            catch (Exception)
            {
                IsSuccessful = false;
            }
        }

        public async Task GetCountryById(int id)
        {
            CountryOverview = await ApiHelper.Read<CountryOverviewModel>($"v2/locations/{id}?source=jhu&timelines=true");
        }
        public List<CountryModel> GetCountryNames()
        {
            var countries = ApiModel.Locations.Where(c => c.Province.Equals("")).Select(c => c);
            var result = new List<CountryModel>();
            foreach (BasicLocationModel country in countries)
            {
                result.Add(new CountryModel() { Id = country.Id, Name = country.Country });
            }
            //result.Add(GetAustraliaName());
            result = result.OrderBy(c => c.Name).ToList();

            return result;

        }

        public async Task GetTimelines(DetailedCountryModel country)
        {
            var id = country.Id;
            var countryApiData = await ApiHelper.Read<CountryOverviewModel>($"v2/locations/{id}?source=jhu&timelines=true");
            var timelineCases = countryApiData.Location.Timelines.Confirmed.Timeline;
            var timelineDeaths = countryApiData.Location.Timelines.Deaths.Timeline;

            country.TimelineCases = timelineCases;
            country.TimelineDeaths = timelineDeaths;
        }

        public async Task<DetailedCountryModel> GetCountryStats(int countryId)
        {
            await GetCountryById(countryId);
            var name = CountryOverview.Location.Country;
            var totaldeaths = CountryOverview.Location.Latest.Deaths;
            var totalconfirmed = CountryOverview.Location.Latest.Confirmed;
            var population = CountryOverview.Location.Country_Population;
            var lastUpdated = CountryOverview.Location.Last_Updated;
            var countryCode = CountryOverview.Location.Country_Code;
            var latestCases = SetLatest(CountryOverview.Location.Timelines.Confirmed.Timeline.Values.ToList<int>());
            var latestDeaths = SetLatest(CountryOverview.Location.Timelines.Deaths.Timeline.Values.ToList<int>());
            return new DetailedCountryModel(countryId, name, countryCode, population, totalconfirmed, totaldeaths, lastUpdated, latestCases, latestDeaths);
        }


        public long WorldCases()
        {
            return ApiModel.Latest.Confirmed;
        }

        public long WorldDeaths()
        {
            return ApiModel.Latest.Deaths;
        }

        public int SetLatest(List<int> timelineValues)
        {
            var penultimate = timelineValues.Count - 2;
            return timelineValues.Last() - timelineValues[penultimate];
        }
    }
}
