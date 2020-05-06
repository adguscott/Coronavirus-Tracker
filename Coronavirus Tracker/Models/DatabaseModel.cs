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
            var sqlcommand = "create table 'Countries' (" +
                            "'Id' integer not null primary key autoincrement unique, " +
                            "'Name' text unique, " +
                            "'Cases' integer, " +
                            "'Deaths' integer)";
            await SQLiteDatabaseManager.NonQuery(sqlcommand, ConnectionString);

        }

        public async Task Create(Country country)
        {
            var sqlCommand = $"insert into Countries (Name, Cases, Deaths) values ('{country.Name}', {country.Cases}, {country.Deaths})";
            await SQLiteDatabaseManager.NonQuery(sqlCommand, ConnectionString);
        }

        public async Task<List<Country>> Read()
        {
            var trackedCountries = new List<Country>();
            var sqlCommand = "select * from Countries";
            var table = await SQLiteDatabaseManager.Query(sqlCommand, ConnectionString);
            foreach(DataRow row in table.Rows)
            {
                trackedCountries.Add(new Country()
                {
                    Name = row["Name"].ToString(),
                    Cases = Convert.ToInt32(row["Cases"]),
                    Deaths = Convert.ToInt32(row["Deaths"])
                });
            }

            /*            using (var connection = new SQLiteConnection(ConnectionString))
                        {
                            var command = new SQLiteCommand
                                (
                                    "select * from Countries", connection
                                );

                            connection.Open();
                            var reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                trackedCountries.Add
                                    (
                                        new DisplayCountry() { Name = reader["Name"].ToString(),
                                        Cases = Convert.ToInt32(reader["Cases"]),
                                        Deaths = Convert.ToInt32(reader["Deaths"])}
                                    );                            
                            }
                        }*/

            return trackedCountries;
        }

        public async Task Update(Country country)
        {
            var sqlCommand = $"update Countries set Cases = {country.Cases}, Deaths = {country.Deaths} where Name = '{country.Name}'";
            await SQLiteDatabaseManager.NonQuery(sqlCommand, ConnectionString);
        }

        public async Task Delete(Country country)
        {
            var sqlcommand = $"delete from Countries where Name = '{country.Name}'";
            await SQLiteDatabaseManager.NonQuery(sqlcommand, ConnectionString);
        }

        public async Task DeleteAll()
        {
            var sqlcommand = "delete from Countries";
            await SQLiteDatabaseManager.NonQuery(sqlcommand, ConnectionString);
        }

    }



}

