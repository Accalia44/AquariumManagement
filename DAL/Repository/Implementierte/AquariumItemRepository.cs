using System;
using System.Linq.Expressions;
using DAL.Entities;
using MongoDB.Driver;

namespace DAL.Repository.Implementierte
{
    public class AquariumItemRepository : Repository<AquariumItem>, IAquariumItemRepository
    {
        public AquariumItemRepository(DBContext context) : base(context){}

        public List<Coral> GetCorals()
        {
            return FilterBy(x => x is Coral).ToList().Cast<Coral>().ToList();
        }

        public List<Animal> GetAnimals()
        {
            return FilterBy(x => x is Animal).ToList().Cast<Animal>().ToList();
        }

       
    }
}

