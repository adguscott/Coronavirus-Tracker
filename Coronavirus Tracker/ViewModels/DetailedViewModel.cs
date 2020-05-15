using Coronavirus_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coronavirus_Tracker.ViewModels
{
    public class DetailedViewModel : ViewModel
    {
        private bool _visibleUC;

        public bool VisibleUC
        {
            get { return _visibleUC; }
            set
            {
                _visibleUC = value;
                OnPropertyChanged("VisibleUC");
            }
        }

        public long Population { get; set; }
        public int LatestCases { get; set; }
        public int LatestDeaths { get; set; }
        public long Cases { get; set; }
        public long Deaths { get; set; }
        public string FlagUri { get; set; }
        public string CFR { get; set; }
        public string Infected { get; set; }
        public DateTime LastUpdated { get; set; }

        public DetailedViewModel(DetailedCountryModel country)
        {
            
            if (country != null)
            {
                this.VisibleUC = true;
                this.Population = country.Population;
                this.LatestCases = country.LatestCases;
                this.LatestDeaths = country.LatestDeaths;
                this.Cases = country.Cases;
                this.Deaths = country.Deaths;
                this.FlagUri = country.FlagUri;
                this.CFR = country.CFR;
                this.Infected = country.Infected;
                this.LastUpdated = country.LastUpdated;
            }
            else
            {
                VisibleUC = false;
            }
        }
    }
}
