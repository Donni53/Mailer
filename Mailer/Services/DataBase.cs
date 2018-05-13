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
            public const string DbName = "Mailer.db";

            public DataBase()
            {
                Connection = new SQLiteConnection($"Data Source = {DbName}");
                if (File.Exists(DbName)) return;
                SQLiteConnection.CreateFile(DbName);
                Connection.Open();
                const string sql = @"CREATE TABLE UserContacts (
	                                        id integer PRIMARY KEY AUTOINCREMENT,
	                                        user text,
	                                        contact blob
                                        );";
                SQLiteCommand command = new SQLiteCommand(sql, Connection);
                command.ExecuteNonQuery();
                Connection.Close();
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

            public async Task AddUser(string owner, Contact contact)
            {
                OpenConnection();
                try
                {
                    var data = ByteConverters.ObjectToByteArray(contact);
                    var sql = "INSERT INTO UserContacts ('user', 'contact') VALUES (@user, @contact)";
                    var command = new SQLiteCommand(sql, Connection);
                    command.Parameters.AddWithValue("@user", owner);
                    command.Parameters.Add("@contact", DbType.Object, data.Length);
                    command.Parameters["@contact"].Value = data;
                    await command.ExecuteNonQueryAsync();
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


            public void Drop()
            {
                OpenConnection();
                const string sql = @"DROP TABLE IF EXISTS UserContacts;";
                var command = new SQLiteCommand(sql, Connection);
                command.ExecuteNonQuery();
                CloseConnection();
            }

        }
    }

}
