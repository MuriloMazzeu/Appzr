using System;

namespace Appzr.ViewModels.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public string TableName { get; private set; }

        public TableAttribute(string name)
        {
            TableName = name;
        }
    }
}
