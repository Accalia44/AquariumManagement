using System;
using DAL;
using Services.ImplementedServices;

namespace Services
{
	public class GlobalService
	{
		public UserServices UserService { get; set; }
		public AquariumService AquariumService { get; set; }
		public CoralService CoralService { get; set; }
		public AnimalService AnimalService { get; set; }
		public AquariumItemService AquariumItemService { get; set; }
		public PictureService PictureService { get; set; }

		public GlobalService(IUnitOfWork uow)
		{
			UnitOfWork uowi = (UnitOfWork)uow;

			UserService = new UserServices(uowi, uowi.User, this);
			AquariumService = new AquariumService(uowi, uowi.Aquarium, this);
            CoralService = new CoralService(uowi, uowi.AquariumItem, this);
            AnimalService = new AnimalService(uowi, uowi.AquariumItem, this);
			AquariumItemService = new AquariumItemService(uowi, uowi.AquariumItem, this);
			PictureService = new PictureService(uowi, uowi.Picture, this);

        } 
	}
}

