using Coronavirus_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Coronavirus_Tracker.Data
{
    class SQLiteDatabaseManager : IDatabaseManager
    {
        private string ConnectionString;

        public SQLiteDatabaseManager()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["Covid19Tracker.Properties.Settings.SQLiteConnectionString"].ConnectionString;
            if(!File.Exists("Countries.sqlite"))
            {
                CreateDatabase();
            }
        }

        public void CreateDatabase()
        {
            SQLiteConnection.CreateFile("Countries.sqlite");
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                var command = new SQLiteCommand
                    (
                        "create table 'Countries' (" +
                        "'Id' integer not null primary key autoincrement unique, " +
                        "'Name' text unique, " +
                        "'Cases' integer, " +
                        "'Deaths' integer)", connection
                    );

                connection.Open();
                command.ExecuteNonQuery();
                
            }

        }

        public async Task Create(DisplayCountry country)
        {
            await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    var command = new SQLiteCommand
                        (
                            $"insert into Countries (Name, Cases, Deaths) "
                            + $"values ('{country.Name}', {country.Cases}, {country.Deaths})", connection
                        );

                    connection.Open();
                    command.ExecuteNonQuery();
                }
                
            });

        }

        public IEnumerable<DisplayCountry> Read()
        {
            var trackedCountries = new ObservableCollection<DisplayCountry>();
            using (var connection = new SQLiteConnection(ConnectionString))
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
            }

            return trackedCountries;
        }

        public async Task Update(DisplayCountry country)
        {
            await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    var command = new SQLiteCommand
                        (
                            $"update Countries set Cases = {country.Cases}, Deaths = {country.Deaths} where Name = '{country.Name}'", connection
                        );

                    connection.Open();
                    command.ExecuteNonQuery();
                }
                
            });
        }

        public void Delete(DisplayCountry country)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                var command = new SQLiteCommand
                    (
                        $"delete from Countries "
                        + $"where Name = '{country.Name}'", connection
                    );

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                var command = new SQLiteCommand
                    (
                        $"delete from Countries", connection
                    );

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

    }



}

