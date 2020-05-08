using Coronavirus_Tracker.Services;
using Coronavirus_Tracker.ViewModels;
using CoronavirusTrackerAPI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public List<ComboboxCountryModel> GetCountryNames()
        {
            var countries = ApiModel.Locations.Where(c => c.Province.Equals("")).Select(c => c);
            var result = new List<ComboboxCountryModel>();
            foreach (LocationModel country in countries)
            {
                result.Add(new ComboboxCountryModel() { Id = country.Id, Name = country.Country });
            }
            result = result.OrderBy(c => c.Name).ToList();

            return result;

        }

        public async Task<Country> GetCountryStats(int countryId)
        {
            await GetCountryById(countryId);
            var name = CountryOverview.Location.Country;
            var deaths = CountryOverview.Location.Latest.Deaths;
            var cases = CountryOverview.Location.Latest.Confirmed;
            var pop = CountryOverview.Location.Country_Population;
            return new Country(countryId, name, pop, cases, deaths);
        }

        public long WorldCases()
        {
            return ApiModel.Latest.Confirmed;
        }

        public long WorldDeaths()
        {
            return ApiModel.Latest.Deaths;
        }
    }
}
