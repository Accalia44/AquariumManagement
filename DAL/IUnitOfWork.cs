using System;
using DAL.Entities;
using DAL.Repository;
using DAL.Repository.Implementierte;

namespace DAL
{
	public interface IUnitOfWork
	{
		DBContext Context { get; }

		IRepository<Aquarium> Aquarium { get; }

		IUserRepository User { get; }

		IAquariumItemRepository AquariumItem { get; }

		IAquariumRepository Aquarium2 { get; }

		IRepository<UserAquarium> UserAquarium { get; }

		IRepository<Picture> Picture { get; }
	}
}

