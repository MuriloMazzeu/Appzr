using System;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace Appzr.Handlers.Infra
{
    internal static class DataUtil
    {
        public static readonly string Database = @"Appzr.db";
        /// <summary>
        /// Create a SQLite db file
        /// </summary>
        /// <param name="deleteIfExits">If a previus db file exists, delete it before create a new db file</param>
        internal static void CreateDB(bool deleteIfExits = false)
        {
            if (File.Exists(Database))
            {
                if (deleteIfExits)
                {
                    File.Delete(Database);
                    CreateDB();
                }
            }
            else CreateDB();
        }

        private static void CreateDB()
        {
            SQLiteConnection.CreateFile(Database);
            using(var connection = new SQLiteConnection($"Data Source={Database};"))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"CREATE TABLE my_apps(
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name VARCHAR(100) NOT NULL,
                        url VARCHAR(4096) NOT NULL,
                        description TEXT DEFAULT NULL,
                        registered_at DATE NOT NULL DEFAULT CURRENT_TIMESTAMP,
                        inactived_at DATE DEFAULT NULL
                    )";
                    command.ExecuteNonQueryAsync().GetAwaiter().GetResult();
                }
                connection.Close();
            }
        }
    }
}
