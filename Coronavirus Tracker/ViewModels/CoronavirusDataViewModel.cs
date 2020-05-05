using Coronavirus_Tracker.Models;
using CovidSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI;

namespace Coronavirus_Tracker.ViewModels 
{
	class CoronavirusDataViewModel : CoronavirusData
	{
		public List<string> GetCountryNames()
		{
			return GetCountryList().Split('\n').ToList<string>();
		}

		public async Task<DisplayCountry> GetCountryStats(string countryName)
		{
			var cases = GetConfirmed(countryName);
			var deaths = GetDeaths(countryName);

			return new DisplayCountry()
			{
				Name = countryName,
				Cases = await cases,
				Deaths = await deaths,
			};
		}

		private async Task<int> GetConfirmed(string countryName)
		{
			var confirmedTask = Task<string>.Run(() => FromCountryNameConfirmed(countryName));

			var confirmed = await confirmedTask;

			return Convert.ToInt32(confirmed);
		}

		private async Task<int> GetDeaths(string countryName)
		{
			var deathsTask = Task<string>.Run(() => FromCountryNameDeaths(countryName));

			var deaths = await deathsTask;

			return Convert.ToInt32(deaths);
		}


	}
}
