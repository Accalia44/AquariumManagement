using System;
using DAL;

namespace Services
{
	public class GlobalService
	{
		public UserServices UserService { get; set; }
		public AquariumService AquariumService { get; set; }
		public GlobalService( IUnitOfWork uow)
		{
			UnitOfWork uowi = (UnitOfWork)uow;

			UserService = new UserServices(uowi, uowi.User, this);
			AquariumService = new AquariumService(uowi, uowi.Aquarium, this);
		} 
	}
}

