using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Logbuch.Classes
{
    class LogbuchDataAccess
    {

        private const string _filename = "LogbuchSQLite.db";

        public async static void InitializeDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync(_filename, CreationCollisionOption.OpenIfExists);
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, _filename);
            Console.WriteLine($"SQLite dbpath: {dbpath}");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS Log (" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "title VARCHAR(30) NULL, " +
                        "content VARCHAR(200) NULL, " +
                        "datetime VARCHAR(19) NULL " +
                    ")";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }

        public static void AddLog(string title, string content, string datetime)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, _filename);
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO Log(title, content, datetime) VALUES " + 
                    "(@title, @content, @datetime);";

                insertCommand.Parameters.AddWithValue("@title", title);
                insertCommand.Parameters.AddWithValue("@content", content);
                insertCommand.Parameters.AddWithValue("@datetime", datetime);

                insertCommand.ExecuteReader();

                db.Close();
            }

        }

        public static void AddLog(Log log)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, _filename);
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO Log(title, content, datetime) VALUES " +
                    "(@title, @content, @datetime);";

                insertCommand.Parameters.AddWithValue("@title", log.Title);
                insertCommand.Parameters.AddWithValue("@content", log.Content);
                insertCommand.Parameters.AddWithValue("@datetime", log.DateTime);

                insertCommand.ExecuteReader();

                db.Close();
            }

        }

        public static List<Log> GetLogs(int limit = 100, int offset = 0)
        {
            List<Log> entries = new List<Log>();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, _filename);
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                Console.WriteLine($"SQLite dbpath: {dbpath}");
                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT id, title, content, datetime FROM Log " +
                     "ORDER BY id DESC " +
                     "LIMIT @limit OFFSET @offset ; ", db);

                selectCommand.Parameters.AddWithValue("@limit", limit.ToString());
                selectCommand.Parameters.AddWithValue("@offset", offset.ToString());

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(new Log
                    {
                        Title = query.GetString(1),
                        Content = query.GetString(2),
                        DateTime = query.GetString(3)
                    });
                }

                db.Close();
            }

            return entries;
        }

    }
}
