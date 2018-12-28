using Common.Model;
using Common.MongoDB.Helpers;
using Common.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.MongoDB
{
    public class Repository<TEntity> : IRepository<TEntity, ObjectId> where TEntity : IEntity<ObjectId>
    {
        protected IMongoDatabase Database { get; set; }
        protected IMongoCollection<TEntity> DataCollection { get; set; }

        public Repository(IMongoDatabase database, bool createIfNotExists = false)
        {
            if (database == null)
                throw new ArgumentNullException("database");
            Database = database;

            string collName = TypeHelper.GetCollectionNameFromType<TEntity>();
            if (!CollectionExists(database, collName))
            {
                if (!createIfNotExists)
                    throw new ArgumentException($"Collection {collName} does not exists \n set 'createIfNotExists' to true if you wont create it automatically");
                else
                    Database.CreateCollection(collName);
            }
            DataCollection = Database.GetCollection<TEntity>(TypeHelper.GetCollectionNameFromType<TEntity>());
        }

        protected bool CollectionExists(IMongoDatabase database, string collectionName)
        {
            return database.ListCollectionNames().ToEnumerable().Any(c=>c==collectionName);
        }

        public TEntity Add(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            DataCollection.InsertOne(entity);
            return entity;
        }

        public IQueryable<TEntity> GetAll()
        {
            return DataCollection.AsQueryable();
        }

        public IQueryable<TEntity> GetEntitiesByExpression(Expression<Func<TEntity, bool>> expression)
        {
            return DataCollection.AsQueryable().Where(expression);
        }

        public IEnumerable<TEntity> GetEntitiesByFilter(Func<TEntity, bool> predicate)
        {
            return DataCollection.Find(e=>true).ToList().Where(predicate);
        }

        public TEntity GetEntity(ObjectId id)
        {
            return DataCollection.Find(e => e.Id == id).FirstOrDefault();
        }

        public async Task<TEntity> GetEntityAsync(ObjectId id)
        {
            return await (await DataCollection.FindAsync(e => e.Id == id)).FirstOrDefaultAsync();
        }

        public void Remove(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            DataCollection.FindOneAndDelete(e => e.Id == entity.Id);
        }

        public void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            DataCollection.ReplaceOne(
                e => e.Id == entity.Id,
                entity,
                new UpdateOptions { IsUpsert = true });
        }
    }
}
