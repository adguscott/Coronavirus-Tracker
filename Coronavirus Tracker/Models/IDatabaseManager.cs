using Coronavirus_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coronavirus_Tracker.Data
{
    interface IDatabaseManager
    {
        void Create(DisplayCountry country);

        ObservableCollection<DisplayCountry> Read();

        Task Update(DisplayCountry country);

        void Delete(DisplayCountry country);

        void DeleteAll();



    }
}
