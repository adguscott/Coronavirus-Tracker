using Coronavirus_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.SQLite;
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
        }

        public async void Create(DisplayCountry country)
        {
            await Task.Run(() =>
            {
                try
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
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
            });

        }

        public ObservableCollection<DisplayCountry> Read()
        {
            var trackedCountries = new ObservableCollection<DisplayCountry>();
            try
            {
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
            }
            catch (SQLiteException e)
            {
                throw e;
            }

            return trackedCountries;
        }

        public async Task Update(DisplayCountry country)
        {
            await Task.Run(() =>
            {
                try
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
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
            });
        }

        public void Delete(DisplayCountry country)
        {
            try
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
            catch (SQLiteException e)
            {
                throw e;
            }
        }

        public void DeleteAll()
        {
            try
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
            catch (SQLiteException e)
            {
                throw e;
            }
        }
    }



}

