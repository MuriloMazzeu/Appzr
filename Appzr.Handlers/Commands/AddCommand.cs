using Appzr.Handlers.Infra;
using Appzr.ViewModels.Attributes;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace Appzr.Handlers.Commands
{
    /// <summary>
    /// Command class to save data
    /// </summary>
    /// <typeparam name="VM">Database table representation class</typeparam>
    internal partial class AddCommand<VM>
    {
        private readonly bool isValid;
        private readonly string tableName;
        private readonly Dictionary<String, String> registryData;

        /// <summary>
        /// Create the command
        /// </summary>
        /// <param name="viewModel">data to save</param>
        internal AddCommand(VM viewModel)
        {
            var type = typeof(VM);
            var tableProps = type.GetProperties();
            var tableInfo = Attribute.GetCustomAttribute(type, typeof(TableAttribute)) as TableAttribute;

            registryData = new Dictionary<string, string>();
            tableName = tableInfo.TableName;
            isValid = true;

            foreach (var prop in tableProps)
            {
                var column = prop.GetCustomAttributes(typeof(ColumnAttribute), false).OfType<ColumnAttribute>().FirstOrDefault();
                var columnValue = prop.GetValue(viewModel);

                if (columnValue == null)
                {
                    if (column.IsRequired)
                    {
                        isValid = false;
                    }
                }
                else
                {
                    registryData.Add(column.ColumnName, $@"""{columnValue.ToDBString()}""");
                }
            }
        }

        /// <summary>
        /// Confirms the command
        /// </summary>
        internal void Commit()
        {
            if (!isValid)
            {
                return;
            }

            var cols = String.Join(", ", registryData.Keys.ToArray());
            var vals = String.Join(", ", registryData.Values.ToArray());
            var sqlCommand = $"INSERT INTO {tableName} ({cols}) VALUES ({vals});";

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

        /// <summary>
        /// True if all required columns has data, false otherwise
        /// </summary>
        internal bool IsValid { get => isValid; }        
    }
}
