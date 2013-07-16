using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EFHistory
{
    public class HistoryDbContext : DbContext
    {
        
        private string _modifyingUser;
        private DateTime _now;

        public HistoryDbContext(string modifyingUser, DateTime now)
        {
            _modifyingUser = modifyingUser;
            _now = now;
        }

        public override int SaveChanges()
        {
            //Need to ensure that updates are really saved as a new record
            var changeSet = ChangeTracker.Entries<HistoryEntity>();
            if (changeSet != null)
            {
                foreach (DbEntityEntry<HistoryEntity> dbEntityEntry in changeSet)
                {
                    dbEntityEntry.Entity.ModifiedBy = _modifyingUser;
                    dbEntityEntry.Entity.ModifiedOnUtc = _now;
                    
                    switch (dbEntityEntry.State)
                    {
                        case EntityState.Added:

                            //create a new sequential guid
                            dbEntityEntry.Entity.Id = NewId();
                            break;

                        case EntityState.Modified:

                            dbEntityEntry.State = EntityState.Added;
                            dbEntityEntry.Entity.Version = null;

                            break;
                    }
                }
            }

            return base.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [DllImport("rpcrt4.dll", SetLastError = true)]
        static extern int UuidCreateSequential(out Guid guid);

        private static Guid NewId()
        {
            const int RPC_S_OK = 0;
            Guid g;
            if (UuidCreateSequential(out g) != RPC_S_OK)
                return Guid.NewGuid();
            else
                return g;
        }
    }
}
