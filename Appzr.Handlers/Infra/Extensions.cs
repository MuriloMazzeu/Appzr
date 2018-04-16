using System;

namespace Appzr.Handlers.Infra
{
    internal static class Extensions
    {
        /// <summary>
        /// Give the correct string to represent it on SQL query
        /// </summary>
        /// <param name="self">value to be saved</param>
        /// <returns></returns>
        internal static string ToDBString(this object self)
        {
            if(self is bool)
            {
                return (bool) self ? "1" : "";
            }
            else if(self is DateTime)
            {
                return ((DateTime)self).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else return self.ToString();
        }
    }
}
