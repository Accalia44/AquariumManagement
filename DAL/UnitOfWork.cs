using System;
using DAL.Entities;
using DAL.Repository;
using DAL.Repository.Implementierte;



namespace DAL
{
	public class UnitOfWork : IUnitOfWork
	{
        public DBContext Context { get; private set; } = null;

        public IRepository<Aquarium> Aquarium => new Repository<Aquarium>(Context);

        public IRepository<UserAquarium> UserAquarium => new Repository<UserAquarium>(Context);

        public IRepository<Picture> Picture => new Repository<Picture>(Context);

        public IUserRepository User => new UserRepository(Context);

        public IAquariumItemRepository AquariumItem => new AquariumItemRepository(Context);

        public IAquariumRepository Aquarium2 => new AquariumRepository(Context);

        public UnitOfWork()
        {
            Context = new DBContext();
        }
    }
}

