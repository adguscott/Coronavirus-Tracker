using Coronavirus_Tracker.Data;
using Coronavirus_Tracker.Models;
using Coronavirus_Tracker.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;

namespace Coronavirus_Tracker.ViewModels
{
    class CoronavirusTrackerViewModel : ViewModel
    {
        private IDatabaseModel DatabaseManager;

        private CoronavirusDataModel Data;

        private bool _enabled;

        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                OnPropertyChanged("Enabled");
            }
        }

        private string _lastUpdated;

        public string LastUpdated
        {
            get =>  $"Last Updated: {_lastUpdated}";
            set
            {
                _lastUpdated = value;
                OnPropertyChanged("LastUpdated");
            }
        }

        private ICollectionView _countryView;

        public ICollectionView CountryView
        {
            get => _countryView;
            set
                {
                    _countryView = value;
                    OnPropertyChanged("CountryView");
                }
        }

        private ObservableCollection<Country> _trackedCountries;

        public ObservableCollection<Country> TrackedCountries
        {
            get => _trackedCountries;

            set
            {
                _trackedCountries = value;
                OnPropertyChanged("Tracked Countries");
            }
        }

        private Country _selectedCountry;

        public Country SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                _selectedCountry = value;
                OnPropertyChanged("SelectedCountry");
            }
        }

        private List<ComboboxCountryModel> _countryNames;

        public List<ComboboxCountryModel> CountryNames
        {
            get { return _countryNames; }
            set {
                    _countryNames = value;
                    OnPropertyChanged("CountryNames");
                }
        }

        private ComboboxCountryModel _selectedCountryName;

        public ComboboxCountryModel SelectedCountryName
        {
            get => _selectedCountryName;
            set
            {
                _selectedCountryName = value;
                OnPropertyChanged("SelectedCountryName");
            }
        }

        private long _worldCases;

        public long WorldCases
        {
            get => _worldCases;
            set
            {
                _worldCases = value;
                OnPropertyChanged("WorldCases");
            }
        }

        private long _worldDeaths;

        public long WorldDeaths
        {
            get => _worldDeaths;
            set
            {
                _worldDeaths = value;
                OnPropertyChanged("WorldDeaths");
            }
        }


        public CoronavirusTrackerViewModel()
        {
            TrackedCountries = new ObservableCollection<Country>();
            DatabaseManager = new DatabaseModel();
            Data = new CoronavirusDataModel();
            Enabled = false;
        }

        public async Task GetData()
        {
               await Task.Run(() => Data.GetApiModel());
        }

        public async Task PopulateView()
        {
            foreach(var country in await DatabaseManager.Read())
            {
                TrackedCountries.Add(country);
            }
            CountryView = CollectionViewSource.GetDefaultView(TrackedCountries);
            CountryView.SortDescriptions.Add(new SortDescription("Cases", ListSortDirection.Descending));

            await GetData();

            if (Data.IsSuccessful)
            {
                SetWorldStats();
                CountryNames = Data.GetCountryNames();
                Enabled = true;
            }
            else
            {
                MessageBox.Show("Could not pull data from API. Try again by clicking Refresh Button");
            }
        }

        public void SetWorldStats()
        {
            WorldCases = Data.WorldCases();
            WorldDeaths = Data.WorldDeaths();
        }

        public async void TrackCountry()
        {
            var countryName = SelectedCountryName;

            if (SelectedCountryName != null)
            {
                var alreadyTracked = TrackedCountries.Where(c => c.Name.Equals(countryName));
                if (!alreadyTracked.Any())
                {
                    var stat = await Data.GetCountryStats(countryName.Id);
                    TrackedCountries.Add(stat);
                    await DatabaseManager.Create(stat);
                }
            }
        }

        public async Task Refresh()
        {
            if (!Data.IsSuccessful) await PopulateView();
            for (int i = 0; i < TrackedCountries.Count; i++)
            {
                var id = TrackedCountries[i].Id;
                TrackedCountries[i] = await Data.GetCountryStats(id);
                await DatabaseManager.Update(TrackedCountries[i]);
            }
            SetLastUpdated();
        }
        public void RemoveTrackedCountry()
        {
            var country = SelectedCountry;
            if(country != null)
            {
                TrackedCountries.Remove(country);
                DatabaseManager.Delete(country);
            }
        }

        public void RemoveAllTrackedCountries()
        {
            TrackedCountries.Clear();
            DatabaseManager.DeleteAll();
        }

        public void SetLastUpdated()
        {
            if (Data != null)
            {
                LastUpdated = DateTime.Now.ToString();
                Properties.Settings.Default.LastUpdated = DateTime.Now.ToString();
                Properties.Settings.Default.Save();
            }
            else
            {
                LastUpdated = Properties.Settings.Default.LastUpdated;
            }

        }
    }
}
