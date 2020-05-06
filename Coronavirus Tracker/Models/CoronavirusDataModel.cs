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
	class CoronavirusDataModel : CoronavirusData
	{
		public List<string> GetCountryNames()
		{
			return GetCountryList().Split('\n').ToList<string>();
		}

		public async Task<Country> GetCountryStats(string countryName)
		{
			var cases = GetConfirmed(countryName);
			var deaths = GetDeaths(countryName);

			return new Country()
			{
				Name = countryName,
				Cases = await cases,
				Deaths = await deaths,
			};
		}

		private async Task<int> GetConfirmed(string countryName)
		{
			var confirmed = Task.Run(() => FromCountryNameConfirmed(countryName));

			return Convert.ToInt32(await confirmed);
		}

		private async Task<int> GetDeaths(string countryName)
		{
			var deaths = Task.Run(() => FromCountryNameDeaths(countryName));

			return Convert.ToInt32(await deaths);
		}


	}
}
