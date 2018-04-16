using Appzr.Handlers.Infra;
using Appzr.ViewModels.Attributes;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;

namespace Appzr.Handlers.Commands
{
    /// <summary>
    /// Command class to delete data
    /// </summary>
    /// <typeparam name="VM">Database table representation class</typeparam>
    internal partial class DeleteCommand<VM>
    {
        private readonly string tableName;
        private readonly PropertyInfo registryId;

        /// <summary>
        /// Create the command
        /// </summary>
        public DeleteCommand()
        {
            var type = typeof(VM);            
            var tableInfo = Attribute.GetCustomAttribute(type, typeof(TableAttribute)) as TableAttribute;

            tableName = tableInfo.TableName;
            registryId = type.GetProperties().FirstOrDefault(p => p
                .GetCustomAttributes(typeof(IdAttribute), false)
                .OfType<IdAttribute>()
                .Any()
            );            
        }

        /// <summary>
        /// Remove all items from active listings
        /// </summary>
        /// <param name="items">Items to be disabled</param>
        public void All(VM[] items)
        {
            var ids = new List<string>();
            foreach (var item in items)
            {
                var id = registryId.GetValue(item).ToString();
                ids.Add(id);
            }

            var sqlCommand = $"UPDATE my_apps SET inactived_at = CURRENT_TIMESTAMP WHERE id IN({String.Join(", ", ids)})";
            using (var connection = new SQLiteConnection($"Data Source={DataUtil.Database};Version=3;"))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sqlCommand;
                    command.ExecuteNonQueryAsync().GetAwaiter().GetResult();
                }
                connection.Close();
            }
        }
    }
}
