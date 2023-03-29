using System;
using DAL;
using DAL.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Tests.DBTests
{
	public class AquariumTests : BaseUnitTests
	{
        Aquarium aquariumTest = new Aquarium();
        Aquarium aquariumForFind = new Aquarium();
        Aquarium aquariumTestInsertOrUpdate = new Aquarium();
        UnitOfWork uow = new UnitOfWork();

        [SetUp]
        public void SetUp()
        {
            aquariumTest = new Aquarium("VikisTestAquarium", 65, 150, 150, 500, WaterType.Saltwater);
            aquariumForFind = new Aquarium("VikisAquariumForFinding", 65, 150, 150, 600, WaterType.Saltwater);
            aquariumTestInsertOrUpdate = new Aquarium("VikisTestAquariumForInsertOrUpdate", 65, 150, 150, 400, WaterType.Saltwater);

        }

        [Test]
		public async Task CreateAquarium()
		{
            Aquarium aquarium = new Aquarium("VikisFische", 65, 150, 150, 500, WaterType.Saltwater);

			Aquarium fromdb = await uow.Aquarium.InsertOneAsync(aquarium);

            Assert.NotNull(fromdb);
			Assert.NotNull(fromdb.ID);

            await uow.Aquarium.DeleteByIdAsync(aquarium.ID);

        }
        [Test]
        public async Task GetByName()
        {
            Aquarium insertedForFinding = await uow.Aquarium.InsertOneAsync(aquariumForFind);
            Aquarium firstFoundAquarium = await uow.Aquarium2.GetByName("VikisAquariumForFinding");
            Assert.IsTrue(firstFoundAquarium.Name == insertedForFinding.Name);

            await uow.Aquarium.DeleteByIdAsync(insertedForFinding.ID);
        }

        [Test]
		public async Task FindByIdAquarium()
		{
            Aquarium insertedForFinding = await uow.Aquarium.InsertOneAsync(aquariumForFind);

            string aquariumID = insertedForFinding.ID;
            Aquarium findFromDb = await uow.Aquarium.FindByIdAsync(aquariumID);
			Assert.IsTrue(findFromDb.ID == aquariumID);

            await uow.Aquarium.DeleteByIdAsync(insertedForFinding.ID);

        }

        [Test]
        public async Task FindOneAquarium()
        {
            Aquarium insertedForFinding = await uow.Aquarium.InsertOneAsync(aquariumForFind);

            Aquarium firstFoundAquarium = await uow.Aquarium.FindOneAsync(a => a.Name == "VikisAquariumForFinding");
            Assert.IsTrue(firstFoundAquarium.ID == insertedForFinding.ID);

            await uow.Aquarium.DeleteByIdAsync(insertedForFinding.ID);
        }

        [Test]
		public async Task FilterByAquarium()
		{
            Aquarium aquariumTestInserted = await uow.Aquarium.InsertOneAsync(aquariumTest);
            Aquarium aquariumTestInserted2 = await uow.Aquarium.InsertOneAsync(aquariumTest);

            IEnumerable<Aquarium> vikisFische = Enumerable.Empty<Aquarium>();
            vikisFische = uow.Aquarium.FilterBy(a => a.Name == "VikisTestAquarium");
            Assert.IsTrue(vikisFische.First().Name == "VikisTestAquarium");

            await uow.Aquarium.DeleteByIdAsync(aquariumTestInserted.ID);
            await uow.Aquarium.DeleteByIdAsync(aquariumTestInserted2.ID);

        }

        [Test]
        public async Task UpdateAquarium()
        {
            Aquarium aquariumUpdate = await uow.Aquarium.InsertOneAsync(aquariumTest);

            aquariumUpdate.Liters = 1000;
			Aquarium updatedTestDb = await uow.Aquarium.UpdateOneAsync(aquariumUpdate);

			Assert.IsTrue(updatedTestDb.Liters == 1000);
            await uow.Aquarium.DeleteByIdAsync(aquariumUpdate.ID);


        }
        [Test]
        public async Task InsertOrUpdateAquarium()
        {            

            Aquarium testInserted = await uow.Aquarium.InsertOrUpdateOneAsync(aquariumTestInsertOrUpdate);

            Aquarium testUpdatedInitial = await uow.Aquarium.InsertOneAsync(aquariumTestInsertOrUpdate);

            testUpdatedInitial.Liters = 600;

            Aquarium testUpdated = await uow.Aquarium.InsertOrUpdateOneAsync(testUpdatedInitial);

            Assert.IsTrue(testUpdated.Name == "VikisTestAquariumForInsertOrUpdate" && testInserted.Name == "VikisTestAquariumForInsertOrUpdate");

            await uow.Aquarium.DeleteByIdAsync(testInserted.ID);
            await uow.Aquarium.DeleteByIdAsync(testUpdated.ID);

        }

        [Test]
        public async Task DeleteManyAquarium()
        {
            Aquarium aquariumDeleting1 = await uow.Aquarium.InsertOneAsync(aquariumTest);
            Aquarium aquariumDeleting2 = await uow.Aquarium.InsertOneAsync(aquariumTest);

            await uow.Aquarium.DeleteManyAsync(doc => doc.Name == "VikisTestAquarium");
            Aquarium deletedAquarium = await uow.Aquarium.FindOneAsync(a => a.Name == "VikisTestAquarium");
            Assert.IsNull(deletedAquarium);
        }

        [Test]
		public async Task DelteByIdAquarium()
		{
            Aquarium aquariumTestDeleting = await uow.Aquarium.InsertOneAsync(aquariumTest);

            await uow.Aquarium.DeleteByIdAsync(aquariumTestDeleting.ID);

			Aquarium deletedAquarium = await uow.Aquarium.FindByIdAsync(aquariumTestDeleting.ID);

            Assert.IsNull(deletedAquarium);
        }

	}
}

