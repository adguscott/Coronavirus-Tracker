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
        private ChartViewModel _chartview;

        public ChartViewModel ChartView
        {
            get => _chartview;

            set
            {
                _chartview = value;
                OnPropertyChanged("ChartView");
            }
        }

        private DetailedViewModel detailedViewModel;

        public DetailedViewModel DetailedView
        {
            get => detailedViewModel;
            set
            {
                detailedViewModel = value;
                OnPropertyChanged("DetailedView");
            }
        }


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

        private ObservableCollection<DetailedCountryModel> _trackedCountries;

        public ObservableCollection<DetailedCountryModel> TrackedCountries
        {
            get => _trackedCountries;

            set
            {
                _trackedCountries = value;
                OnPropertyChanged("TrackedCountries");
            }
        }

        private DetailedCountryModel _selectedCountry;

        public DetailedCountryModel SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                _selectedCountry = value;
                OnPropertyChanged("SelectedCountry");
            }
        }

        private List<CountryModel> _countryNames;

        public List<CountryModel> CountryNames
        {
            get { return _countryNames; }
            set {
                    _countryNames = value;
                    OnPropertyChanged("CountryNames");
                }
        }

        private CountryModel _selectedCountryName;

        public CountryModel SelectedCountryName
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

        private int _newTrackedIndex;

        public int NewTrackedIndex
        {
            get => _newTrackedIndex;
            set
            {
                _newTrackedIndex = value;
                OnPropertyChanged("NewTrackedIndex");
            }
        }


        public CoronavirusTrackerViewModel()
        {
            TrackedCountries = new ObservableCollection<DetailedCountryModel>();
            DatabaseManager = new DatabaseModel();
            Data = new CoronavirusDataModel();
            Enabled = false;
            ChartView = new ChartViewModel(SelectedCountry);
            DetailedView = new DetailedViewModel(SelectedCountry);
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
            NewTrackedIndex = 0;
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

        public async Task SelectionChanged()
        {

            if (SelectedCountry != null && SelectedCountry.TimelineCases != null)
                await Data.GetTimelines(SelectedCountry);

            DetailedView = new DetailedViewModel(SelectedCountry);
            ChartView = new ChartViewModel(SelectedCountry);

        }

        public void SetWorldStats()
        {
            WorldCases = Data.WorldCases();
            WorldDeaths = Data.WorldDeaths();
        }

        public async void TrackCountry()
        {
            Enabled = false;
            var country = SelectedCountryName;
            if (country != null)
            {
                var alreadyTracked = TrackedCountries.Where(c => c.Name.Equals(country.Name));
                if (!alreadyTracked.Any())
                {
                    var stat = await Data.GetCountryStats(country.Id);
                    await Data.GetTimelines(stat);
                    TrackedCountries.Add(stat);
                    NewTrackedIndex = (TrackedCountries.Count - 1);
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
            NewTrackedIndex = 0;
        }
        public void RemoveTrackedCountry()
        {
            var country = SelectedCountry;
            if(country != null)
            {
                TrackedCountries.Remove(country);
                DatabaseManager.Delete(country);
                NewTrackedIndex = 0;
            }
        }

        public void RemoveAllTrackedCountries()
        {
            TrackedCountries.Clear();
            DatabaseManager.DeleteAll();
        }

        
    }
}
