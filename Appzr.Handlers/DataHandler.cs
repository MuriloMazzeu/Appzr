using Appzr.Handlers.Commands;
using Appzr.Handlers.Infra;
using Appzr.Handlers.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appzr.Handlers
{
    public class DataHandler
    {
        /// <summary>
        /// Create database if not extists
        /// </summary>
        public static void Initialize()
        {
            DataUtil.CreateDB();
        }

        /// <summary>
        /// Save the object into database
        /// </summary>
        /// <typeparam name="VM">Database table representation class</typeparam>
        /// <param name="item">data to save</param>
        /// <returns>False if the object is invalid to save, true if object was saved</returns>
        public static bool Add<VM>(VM item)
        {
            var command = new AddCommand<VM>(item);
            if (command.IsValid)
            {
                command.Commit();
                return true;
            }
            else return false;
        }

        /// <summary>
        /// List data from database
        /// </summary>
        /// <typeparam name="VM">Database table representation class</typeparam>
        /// <param name="where">predicate to filter objects</param>
        /// <param name="first">number of first rows to limit</param>
        /// <returns>All filtered objects</returns>
        public static VM[] List<VM>(Func<VM, bool> where, uint? first = null) where VM : new()
            => new ListQuery<VM>(first).All().Where(where).ToArray();

        /// <summary>
        /// Remove items from active listings
        /// </summary>
        /// <typeparam name="VM">Database table representation class</typeparam>
        /// <param name="items">items to be disabled</param>
        public static void Remove<VM>(VM[] items)
            => new DeleteCommand<VM>().All(items);

        /// <summary>
        /// Remove the item from active listings
        /// </summary>
        /// <typeparam name="VM">Database table representation class</typeparam>
        /// <param name="items">item to be disabled</param>
        public static void Remove<VM>(VM items)
            => new DeleteCommand<VM>().All(new VM[] { items });
    }
}
