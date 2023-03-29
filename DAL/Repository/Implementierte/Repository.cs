using System;
using System.Linq.Expressions;
using DAL.Entities;
using MongoDB.Driver;

namespace DAL.Repository
{
	
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected Serilog.ILogger log = Utils.Logger.ContextLog<IRepository<TEntity>>();

        private readonly IMongoCollection<TEntity> mongoCollection;

        public Repository(DBContext context)
        {
            mongoCollection = context.DataBase.GetCollection<TEntity>(typeof(TEntity).Name.ToString());
        }

        //Delete by ID funktioniert
        public async Task DeleteByIdAsync(string id)
        {
            var filter = Builders<TEntity>.Filter.Eq(doc => doc.ID, id);
            await mongoCollection.DeleteOneAsync(filter);
                
        }
        //Delete Many Funktioniert
        public async Task DeleteManyAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            await mongoCollection.DeleteManyAsync(filterExpression);
        }

        public async Task DeleteOneAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            await mongoCollection.FindOneAndDeleteAsync(filterExpression);
        }

        //Filter By Funktioniert
        public IEnumerable<TEntity> FilterBy(Expression<Func<TEntity, bool>> filterExpression)
        {
            return mongoCollection.Find(filterExpression).ToEnumerable();
        }

        public IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TProjected>> projectionExpression)
        {
            return mongoCollection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        //Find by ID funktioniert
        public async Task<TEntity> FindByIdAsync(string id)
        {
                var filter = Builders<TEntity>.Filter.Eq(doc => doc.ID, id);
                return await mongoCollection.Find(filter).SingleOrDefaultAsync();
               
         }

        //Find One Async Funktioniert
        public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            return await mongoCollection.Find(filterExpression).FirstOrDefaultAsync();
        }

        //Insert One funktioniert
        public virtual async Task<TEntity> InsertOneAsync(TEntity document)
        {
            document.ID = document.Generator();
            await mongoCollection.InsertOneAsync(document);
            return document;
        }

        //Insert Or Update funktioniert
        public virtual async Task<TEntity> InsertOrUpdateOneAsync(TEntity document)
        {
            var idFoundOrNot = await this.FindByIdAsync(document.ID);
            try
            {
                var emptyID = String.IsNullOrEmpty(idFoundOrNot.ID);
                var filter = Builders<TEntity>.Filter.Eq(doc => doc.ID, document.ID);
                mongoCollection.FindOneAndReplace(filter, document);
                return document;
                
            }
            catch (NullReferenceException e)
            {
                document.ID = document.Generator();
                await this.InsertOneAsync(document);
                return document;
            }
        }

        //Updaet funktioniert
        public async Task<TEntity> UpdateOneAsync(TEntity document)
        {
            var filter = Builders<TEntity>.Filter.Eq(doc => doc.ID, document.ID);
            mongoCollection.FindOneAndReplace(filter, document);
            return document;

        }



    }

}

