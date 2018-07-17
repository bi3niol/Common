using Common.Model;
using Common.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.EntityFramework
{
    public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : Entity<TKey>
    {
        protected DbContext context;
        protected DbSet<TEntity> dbSet;

        public Repository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public TEntity Add(TEntity entity)
        {
            return dbSet.Add(entity);
        }

        public IQueryable<TEntity> GetAll()
        {
            return dbSet.Where(e => true);
        }

        public abstract Task<TEntity> GetEntityAsync(TKey id);
        public abstract TEntity GetEntity(TKey id);

        public IQueryable<TEntity> GetEntitiesByExpression(Expression<Func<TEntity, bool>> expression)
        {
            return dbSet.Where(expression);
        }

        public IEnumerable<TEntity> GetEntitiesByFilter(Func<TEntity, bool> predicate)
        {
            IEnumerable<TEntity> entities = dbSet.Where(predicate);
            return entities;
        }

        public void Remove(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);

            dbSet.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            entity = dbSet.Attach(entity);

            context.Entry(entity).State = EntityState.Modified;
        }
    }
}
