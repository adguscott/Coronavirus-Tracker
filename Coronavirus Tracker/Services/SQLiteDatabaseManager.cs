using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Coronavirus_Tracker.Services
{
    public static class SQLiteDatabaseManager
    {
        public static async Task<DataTable> Query(string commandString, string connectionString)
        {
            var table = new DataTable();
            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                    (
                        commandString, connection
                    );

                try
                {
                    connection.Open();
                }
                catch (Exception e)
                {

                    throw e;
                }

                var adapter = new SQLiteDataAdapter(command);
                await Task.Run(() => adapter.Fill(table));

                return table;
            }
        }

        public static async Task NonQuery(string commandString, string connectionString)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                    (
                        commandString, connection
                    );

                try
                {
                    connection.Open();
                }
                catch (Exception e)
                {

                    throw e;
                }
              

                await Task.Run(() => command.ExecuteNonQuery());
            }
        }
    }
}
