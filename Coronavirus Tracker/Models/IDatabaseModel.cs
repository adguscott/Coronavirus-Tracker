using Coronavirus_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coronavirus_Tracker.Data
{
    interface IDatabaseModel
    {
        Task CreateDatabase();

        Task Create(DetailedCountryModel country);

        Task<List<DetailedCountryModel>> Read();

        Task Update(DetailedCountryModel country);

        Task Delete(DetailedCountryModel country);

        Task DeleteAll();



    }
}
