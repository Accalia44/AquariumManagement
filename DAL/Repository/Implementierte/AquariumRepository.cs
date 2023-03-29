using System;
using System.Linq.Expressions;
using DAL.Entities;
using MongoDB.Driver;

namespace DAL.Repository.Implementierte
{
    public class AquariumRepository : Repository<Aquarium>, IAquariumRepository
    {
        public AquariumRepository(DBContext context) : base(context) { }

        public async Task<Aquarium> GetByName(string name)
        {
            
            return await this.FindOneAsync(doc => doc.Name.Equals(name));
        }

   
    }
}

