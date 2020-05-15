using Coronavirus_Tracker.Models;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Coronavirus_Tracker.ViewModels
{
    public class ChartViewModel : ViewModel
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
        public ChartViewModel(DetailedCountryModel country)
        {
            VisibleUC = false;
            if(country != null)
                if (country.TimelineCases != null)
                {
                    VisibleUC = true;
                    PopulateChart(country);
                }

        }

        public List<int> GetTimelineStat(Dictionary<DateTime, int> timeline)
        {
            var stat = timeline.Values.ToList<int>();
            var timelineResult = new List<int>();
            for (int i = 0; i < stat.Count; i++)
            {
                var figure = (i == 0) ? 0 : stat[i] - stat[i - 1];
                if (figure < 0)
                    figure = 0;
                timelineResult.Add(figure);

            }
            return timelineResult;
        }

        public void PopulateChart(DetailedCountryModel country)
        {
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Cases",
                    Values = new ChartValues<int>(GetTimelineStat(country.TimelineCases)),
                }
            };

            Labels = country.TimelineCases.Keys.Select(key => key.ToShortDateString()).ToArray();

            SeriesCollection.Add(new LineSeries
            {
                Title = "Deaths",
                Values = new ChartValues<int>(GetTimelineStat(country.TimelineDeaths)),
            });
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
    }
}

