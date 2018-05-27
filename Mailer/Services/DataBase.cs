using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mailer.Helpers;
using Mailer.Model;
using NLog.Fluent;

namespace Mailer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SQLite;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Mailer.Database
    {
        public class DataBase
        {
            public SQLiteConnection Connection;
            public const string DbName = "MailerSettings.db";

            public DataBase()
            {
                Connection = new SQLiteConnection($"Data Source = {DbName}");
                if (File.Exists(DbName)) return;
                SQLiteConnection.CreateFile(DbName);
            }


            public void OpenConnection()
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
            }

            public void CloseConnection()
            {
                if (Connection.State != ConnectionState.Closed)
                    Connection.Close();
            }

            public async Task AddCache()
            {
                await DropTable(@"Cache");
                await CreateTable(@"Cache");
                OpenConnection();
                try
                {
                    if (!Directory.Exists(@"Cache")) return;
                    List<string> filesnames = Directory.GetFiles(@"Cache").ToList<string>();
                    foreach (var item in filesnames)
                    {
                        using (var sr = new StreamReader(item))
                        {
                            string line = sr.ReadToEnd();
                            var lineBytes = ByteConverters.ObjectToByteArray(line);
                            var sql = "INSERT INTO Cache ('filename', 'file') VALUES (@filename, @file)";
                            var command = new SQLiteCommand(sql, Connection);
                            command.Parameters.AddWithValue("@filename", item);
                            command.Parameters.Add("@file", DbType.Binary, lineBytes.Length);
                            command.Parameters["@file"].Value = lineBytes;
                            await command.ExecuteNonQueryAsync();
                        }
                    }

                }
                catch (Exception e)
                {
                    LoggingService.Log(e);
                }
                CloseConnection();
            }

            public async Task LoadCache()
            {
                OpenConnection();
                try
                {
                    if (!Directory.Exists(@"Cache"))
                        Directory.CreateDirectory(@"Cache");
                    var sql = $"SELECT * FROM Cache";
                    var command = new SQLiteCommand(sql, Connection);
                    var reader = command.ExecuteReaderAsync();
                    while (reader.Result.Read())
                    {
                        string filename = (string)reader.Result["filename"];
                        string filecontent = (string)ByteConverters.ByteArrayToObject((byte[])reader.Result["file"]);
                        using (StreamWriter sw = new StreamWriter(filename))
                        {
                            await sw.WriteAsync(filecontent);
                        }
                    }
                }
                catch (Exception e)
                {
                    LoggingService.Log(e);
                }
                CloseConnection();
            }

            public List<Contact> LoadUsers(string user)
            {
                OpenConnection();
                var result = new List<Contact>();
                try
                {
                    var sql = $"SELECT * FROM UserContacts WHERE user = '{user}'";
                    var command = new SQLiteCommand(sql, Connection);
                    var reader = command.ExecuteReaderAsync();
                    while (reader.Result.Read())
                    {
                        var tmp = (Contact)ByteConverters.ByteArrayToObject((byte[])reader.Result["contact"]);
                        result.Add(tmp);
                    }
                    CloseConnection();
                }
                catch (Exception e)
                {
                    LoggingService.Log(e);
                }
                return result;
            }

            public async Task CreateTable(string table)
            {
                OpenConnection();
                try
                {
                    const string sql = @" CREATE TABLE Cache (filename text PRIMARY KEY, file blob);";
                    SQLiteCommand command = new SQLiteCommand(sql, Connection);
                    await command.ExecuteNonQueryAsync();

                }
                catch (Exception e)
                {
                    LoggingService.Log(e);
                }
                CloseConnection();
            }

            public async Task DropTable(string table)
            {
                OpenConnection();
                string sql = @"DROP TABLE IF EXISTS "+table+";";
                var command = new SQLiteCommand(sql, Connection);
                await command.ExecuteNonQueryAsync();
                CloseConnection();
            }

        }
    }

}
