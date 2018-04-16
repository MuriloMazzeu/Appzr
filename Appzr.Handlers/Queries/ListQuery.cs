using Appzr.Handlers.Infra;
using Appzr.ViewModels.Attributes;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;

namespace Appzr.Handlers.Queries
{
    /// <summary>
    /// Query class to list data
    /// </summary>
    /// <typeparam name="VM">Database table representation class</typeparam>
    internal partial class ListQuery<VM> where VM : new()
    {
        private readonly string tableName;
        private readonly string[] tableColumns;
        private readonly uint? first;
        private readonly PropertyInfo[] tableProps;

        /// <summary>
        /// Create the query
        /// </summary>
        /// <param name="first">number of firsts rows to return</param>
        public ListQuery(uint? first = null)
        {
            this.first = first;
            var type = typeof(VM);
            tableProps = type.GetProperties();
            var tableInfo = Attribute.GetCustomAttribute(type, typeof(TableAttribute)) as TableAttribute;

            tableName = tableInfo.TableName;
            tableColumns = tableProps.Select(p => p
                .GetCustomAttributes(typeof(ColumnAttribute), false)
                .OfType<ColumnAttribute>()
                .FirstOrDefault()
                .ColumnName
            ).ToArray();
        }

        /// <summary>
        /// Converts plain data to object's data
        /// </summary>
        /// <param name="reader">the plain data provider</param>
        /// <returns>Object filled</returns>
        private VM DoProjection(DbDataReader reader)
        {
            var vm = new VM();                        

            foreach (var col in tableColumns)
            {
                var ordinal = reader.GetOrdinal(col);
                var data = reader.IsDBNull(ordinal) ? null : reader.GetValue(ordinal);
                
                var prop = tableProps.Where(p => p
                    .GetCustomAttributes(typeof(ColumnAttribute), false)
                    .OfType<ColumnAttribute>()
                    .FirstOrDefault()
                    ?.ColumnName == col
                ).FirstOrDefault();

                prop.SetValue(vm, data);
            }

            return vm;
        }

        /// <summary>
        /// Give all rows from database as a generator
        /// </summary>
        /// <returns>generator for iterations</returns>
        internal IEnumerable<VM> All()
        {
            var cols = String.Join(", ", tableColumns);
            var sqlCommand = $"SELECT {cols} FROM {tableName}";

            if (first.HasValue)
            {
                sqlCommand += $" LIMIT {first.Value}";
            }

            using (var connection = new SQLiteConnection($"Data Source={DataUtil.Database};Version=3;"))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sqlCommand;
                    var reader = command.ExecuteReaderAsync().GetAwaiter().GetResult();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            yield return DoProjection(reader);
                        }
                    }
                }
                connection.Close();
            }
        }
    }
}
