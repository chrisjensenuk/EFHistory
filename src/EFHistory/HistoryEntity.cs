using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFHistory
{
    public abstract class HistoryEntity : IHistory
    {
        [Key, Column(Order = 0)]
        public Guid Id { get; internal set; }

        [Key, Column(Order = 1)]
        [Timestamp]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte[] Version { get; internal set; }

        public string ModifiedBy { get; internal set; }
        public DateTime ModifiedOnUtc { get; internal set; }

        public bool Deleted { get; internal set; }
    }
}
