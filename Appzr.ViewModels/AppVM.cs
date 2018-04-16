using Appzr.ViewModels.Attributes;
using System;

namespace Appzr.ViewModels
{
    [Table("my_apps")]
    public class AppVM
    {
        [Id]
        [Column("id", IsRequired = false)]
        public long? Id { get; set; }

        [Column("name", IsRequired = true)]
        public string Name { get; set; }

        [Column("url", IsRequired = true)]
        public string Url { get; set; }

        [Column("description", IsRequired = false)]
        public string Description { get; set; }

        [Column("registered_at", IsRequired = false)]
        public DateTime? RegisteredAt { get; set; }

        [Column("inactived_at", IsRequired = false)]
        public DateTime? InactiveAt { get; set; }
    }
}
