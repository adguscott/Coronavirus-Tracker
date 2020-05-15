using Coronavirus_Tracker.Models;
using Coronavirus_Tracker.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Coronavirus_Tracker.Data
{
    class DatabaseModel : IDatabaseModel
    {
        private string ConnectionString;

        public DatabaseModel()
        {
            ConnectionString = Properties.Settings.Default.SQLiteConnectionString;
            if(!File.Exists("Countries.sqlite"))
            {
                Task.Run(() => CreateDatabase());
            }
        }

        public async Task CreateDatabase()
        {
            SQLiteConnection.CreateFile("Countries.sqlite");
            var createCountries = "create table 'Countries' (" +
                            "'Id' integer not null primary key unique, " +
                            "'Name' text unique, " +
                            "'CountryCode' text unique, " +
                            "'Population' integer, " +
                            "'LatestCases' integer, " +
                            "'LatestDeaths' integer, " +
                            "'Cases' integer, " +
                            "'Deaths' integer, " +
                            "'LastUpdated' text)";

            await SQLiteDatabaseManager.NonQuery(createCountries, ConnectionString);
        }

        public async Task Create(DetailedCountryModel country)
        {
            var sqlCommand = $"insert into Countries (Id, Name, CountryCode, Population, LatestCases, LatestDeaths, Cases, Deaths, LastUpdated) values ({country.Id}, '{country.Name}', '{country.CountryCode}', {country.Population}, {country.LatestCases}, {country.LatestDeaths}, {country.Cases}, {country.Deaths}, '{country.LastUpdated:s}')";
            await SQLiteDatabaseManager.NonQuery(sqlCommand, ConnectionString);
        }

        public async Task<List<DetailedCountryModel>> Read()
        {
            var trackedCountries = new List<DetailedCountryModel>();
            var sqlCommand = "select * from Countries";
            var table = await SQLiteDatabaseManager.Query(sqlCommand, ConnectionString);
            foreach(DataRow row in table.Rows)
            {
                var id = Convert.ToInt32(row["Id"]);
                var name = row["Name"].ToString();
                var countryCode = row["CountryCode"].ToString();
                var population = Convert.ToInt64(row["Population"]);
                var latestCases = Convert.ToInt32(row["LatestCases"]);
                var latestDeaths = Convert.ToInt32(row["LatestDeaths"]);
                var totalCases = Convert.ToInt32(row["Cases"]);
                var totalDeaths = Convert.ToInt32(row["Deaths"]);
                DateTime lastUpdated = DateTime.Parse(row["LastUpdated"].ToString());
                trackedCountries.Add(new DetailedCountryModel(id, name, countryCode, population, totalCases, totalDeaths, lastUpdated, latestCases, latestDeaths));
            }

            return trackedCountries;
        }

        public async Task Update(DetailedCountryModel country)
        {
            var sqlCommand = $"update Countries set LatestCases = {country.LatestCases}, LatestDeaths = {country.LatestDeaths}, Cases = {country.Cases}, Deaths = {country.Deaths}, LastUpdated = '{country.LastUpdated:s}' where Name = '{country.Name}'";
            await SQLiteDatabaseManager.NonQuery(sqlCommand, ConnectionString);
        }

        public async Task DeleteCountryTimeline(int countryId, string tableName)
        {
            var sqlCommand = $"delete from {tableName} where countryId = {countryId}";
            await SQLiteDatabaseManager.NonQuery(sqlCommand, ConnectionString);
        }
        public async Task Delete(DetailedCountryModel country)
        {
            var sqlCommand = $"delete from Countries where Id = {country.Id}";
            await SQLiteDatabaseManager.NonQuery(sqlCommand, ConnectionString);
        }

        public async Task DeleteAll()
        {
            var deleteCountries = "delete from Countries";
            await SQLiteDatabaseManager.NonQuery(deleteCountries, ConnectionString);
        }

    }



}

