using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFHistory
{
    public interface IHistory
    {
        Guid Id { get; }
        byte[] Version { get; }
        string ModifiedBy { get; }
        DateTime ModifiedOnUtc { get; }
        bool Deleted { get; }
    }
}
