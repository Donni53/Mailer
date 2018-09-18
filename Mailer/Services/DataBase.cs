using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mailer.Helpers;

namespace Mailer.Services
{
    namespace Mailer.Database
    {
        public class DataBase
        {
            public const string DbName = "MailerSettings.db";
            public SQLiteConnection Connection;

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
                    var filesnames = Directory.GetFiles(@"Cache").ToList();
                    foreach (var item in filesnames)
                        using (var sr = new StreamReader(item))
                        {
                            var line = sr.ReadToEnd();
                            var lineBytes = ByteConverters.ObjectToByteArray(line);
                            var sql = "INSERT INTO Cache ('filename', 'file') VALUES (@filename, @file)";
                            var command = new SQLiteCommand(sql, Connection);
                            command.Parameters.AddWithValue("@filename", item);
                            command.Parameters.Add("@file", DbType.Binary, lineBytes.Length);
                            command.Parameters["@file"].Value = lineBytes;
                            await command.ExecuteNonQueryAsync();
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
                    var sql = "SELECT * FROM Cache";
                    var command = new SQLiteCommand(sql, Connection);
                    var reader = command.ExecuteReaderAsync();
                    while (reader.Result.Read())
                    {
                        var filename = (string) reader.Result["filename"];
                        var filecontent = (string) ByteConverters.ByteArrayToObject((byte[]) reader.Result["file"]);
                        using (var sw = new StreamWriter(filename))
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

            public async Task CreateTable(string table)
            {
                OpenConnection();
                try
                {
                    const string sql = @" CREATE TABLE Cache (filename text PRIMARY KEY, file blob);";
                    var command = new SQLiteCommand(sql, Connection);
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
                var sql = @"DROP TABLE IF EXISTS " + table + ";";
                var command = new SQLiteCommand(sql, Connection);
                await command.ExecuteNonQueryAsync();
                CloseConnection();
            }
        }
    }
}