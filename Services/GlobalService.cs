using System;
using DAL;

namespace Services
{
	public class GlobalService
	{
		public UserServices UserService { get; set; }
		public GlobalService( IUnitOfWork uow)
		{
			UnitOfWork uowi = (UnitOfWork)uow;

			UserService = new UserServices(uowi, uowi.User, this);
		} 
	}
}

