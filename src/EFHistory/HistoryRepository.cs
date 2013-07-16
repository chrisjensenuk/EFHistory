using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EFHistory
{
    //resources:
    //http://stackoverflow.com/questions/1313120/retrieving-the-last-record-in-each-group

    public class HistoryRepository<T> where T : HistoryEntity
    {
        protected DbSet<T> DbSet;

        public HistoryRepository(DbContext dataContext)
        {
            DbSet = dataContext.Set<T>();
        }

        public void Insert(T entity)
        {
            DbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            entity.Deleted = true;
        }

        /// <summary>
        /// Just return the latest non-deleted version for the Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Find(Guid id)
        {
            var entity = DbSet.Where(h => h.Id == id).OrderByDescending(h => h.Version).FirstOrDefault();

            //don't return deleted items
            return (entity.Deleted) ? null : entity;
        }
        
        /// <summary>
        /// Find specific version of a record including deleted ones
        /// </summary>
        /// <param name="id"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public T Find(Guid id, byte[] version)
        {
            return DbSet.Find(new { id, version });
        }

        /// <summary>
        /// An IQueryable pre-filtered to just return the latest non-deleted version of the records
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> CurrentVersions()
        {
            return DbSet.GroupBy(h => h.Id)
                    .Select(g => g.Max(gm => gm.Version))
                        .Join(DbSet, recentVersion => recentVersion, m => m.Version, (recentVersion, h) => h)
                        .Where(m => m.Deleted == false);
        }

        public IQueryable<T> All()
        {
            return DbSet;
        }

    }
}
