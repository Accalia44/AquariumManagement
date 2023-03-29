using System;
using DAL.Entities;

namespace DAL.Repository.Implementierte
{
	public interface IAquariumItemRepository : IRepository<AquariumItem>
	{
        List<Coral> GetCorals();
        List<Animal> GetAnimals();
    }
}

