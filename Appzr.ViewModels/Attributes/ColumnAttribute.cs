using System;

namespace Appzr.ViewModels.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public string ColumnName { get; private set; }

        public bool IsRequired { get; set; }

        public ColumnAttribute(string name)
        {
            ColumnName = name;
        }
    }
}
