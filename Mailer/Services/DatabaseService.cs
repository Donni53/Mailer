using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mailer.Services
{
    public static class DatabaseService
    {
        public static bool DatabaseExists(string name)
        {
            return File.Exists(name);
        }

        public static bool CreateDataBase(string name)
        {
            try
            {
                SQLiteConnection.CreateFile(name);
                return true;
            }
            catch (Exception e)
            {
                LoggingService.Log(e);
                return false;
            }
        }

        public static bool TableExists(string database, string table)
        {
            try
            {
                var connection = new SQLiteConnection($"Data Source={database};");
                connection.Open();
                var command = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;", connection);
                var reader = command.ExecuteReader();
                foreach (DbDataRecord record in reader)
                    if (table == (string) record["name"])
                        return true;
                return false;
            }
            catch (Exception e)
            {
                LoggingService.Log(e);
                return false;
            }
        }

        public static bool AddNewTable(string database, string table)
        {
            try
            {
                var connection = new SQLiteConnection($"Data Source={database};");
                connection.Open();
                var command = new SQLiteCommand($"CREATE TABLE {table} (id INTEGER PRIMARY KEY, value TEXT);", connection);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                LoggingService.Log(e);
                return false;
            }
        }

        public static bool DeleteTable(string database, string table)
        {
            try
            {
                var connection = new SQLiteConnection($"Data Source={database};");
                connection.Open();
                var command = new SQLiteCommand($"DROP TABLE {table}", connection);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                LoggingService.Log(e);
                return false;
            }
        }

        public static bool AddNewRecord(string database, string table, string id, object value)
        {
            try
            {
                return false;
            }
            catch (Exception e)
            {
                LoggingService.Log(e);
                return false;
            }
        }

        public static bool DeleteRecord(string database, string table, string id)
        {
            try
            {
                return false;
            }
            catch (Exception e)
            {
                LoggingService.Log(e);
                return false;
            }
        }


        public static bool ExecCommand(string database, string sql)
        {
            try
            {
                var connection = new SQLiteConnection($"Data Source={database};");
                connection.Open();
                var command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception e)
            {
                LoggingService.Log(e);
                return false;
            }
        }
    }
}
