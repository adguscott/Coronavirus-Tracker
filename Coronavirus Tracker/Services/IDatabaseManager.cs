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
        void CreateDatabase();

        Task Create(DisplayCountry country);

        IEnumerable<DisplayCountry> Read();

        Task Update(DisplayCountry country);

        void Delete(DisplayCountry country);

        void DeleteAll();



    }
}
