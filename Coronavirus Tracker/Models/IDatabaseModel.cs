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

        Task Create(Country country);

        Task<List<Country>> Read();

        Task Update(Country country);

        Task Delete(Country country);

        Task DeleteAll();



    }
}
