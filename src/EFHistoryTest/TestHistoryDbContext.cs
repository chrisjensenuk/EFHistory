using EFHistory;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFHistoryTest
{
    public class TestHistoryDbContext : HistoryDbContext
    {
        public TestHistoryDbContext(string modifyingUser) : base(modifyingUser, DateTime.UtcNow) { }

        public DbSet<Message> Messages { get; set; }
    }
}
