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
			var country = Task.Run(() =>
			{
				return new DisplayCountry()
				{
					Name = countryName,
					Cases = Convert.ToInt32(FromCountryNameConfirmed(countryName)),
					Deaths = Convert.ToInt32(FromCountryNameDeaths(countryName))
				};
			});

			return await country;
		}


	}
}
