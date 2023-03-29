using System;
using System.Xml.Linq;
using DAL;
using DAL.Entities;

namespace Tests.DBTests
{
	public class AquariumItemTest : BaseUnitTests
	{
        UnitOfWork uow = new UnitOfWork();
        Coral testCoral = new Coral();
        Coral testCoral1 = new Coral();
        Coral testCoral2 = new Coral();

        Animal testAnimal1 = new Animal();
        Animal testAnimal2 = new Animal();
        Animal animalForFinding = new Animal();

        Aquarium aquarium = new Aquarium();

        [SetUp]
        public void SetUp()
        {
            testCoral.CoralType = CoralType.Hardcoral;
            testCoral.Name = "My little Coral";

            testAnimal1 = new Animal("Vikis Fische", "DoriTest", "Docktorfisch", 1, "My Little Dori", true);
            testAnimal2 = new Animal("Vikis Fische", "NemoTest", "Anemonenfisch", 20, "My Little Nemo", true);
            animalForFinding = new Animal("Vikis Fische", "DoriForFinding", "Docktorfisch", 1, "My Little Dori", true);

            testCoral1 = new Coral("Vikis Fische", "TestCorale", "CactusCoral", 10, "This is a Cactus Coral", CoralType.Hardcoral);
            testCoral2 = new Coral("Vikis Fische", "TestCorale", "TigerCoral", 20, "This is a Tiger Coral", CoralType.Softcoral);
            Aquarium aquarium = new Aquarium("VikisFische", 65, 150, 150, 500, WaterType.Saltwater);
        }

        [TearDown]
        public async Task TearDown()
        {
            await uow.Aquarium.DeleteByIdAsync(aquarium.ID);
            await uow.AquariumItem.DeleteByIdAsync(testCoral.ID);
            await uow.AquariumItem.DeleteByIdAsync(testCoral1.ID);
            await uow.AquariumItem.DeleteByIdAsync(testCoral2.ID);
            await uow.AquariumItem.DeleteByIdAsync(testAnimal1.ID);
            await uow.AquariumItem.DeleteByIdAsync(testAnimal2.ID);
            await uow.AquariumItem.DeleteByIdAsync(animalForFinding.ID);
            await uow.AquariumItem.DeleteManyAsync(doc => doc.Name == "DoriTest");

        }

        [Test]
        public async Task CreateAquariumItem()
        {
            AquariumItem coralTest = await uow.AquariumItem.InsertOneAsync(testCoral);

            Assert.NotNull(coralTest);
            Assert.NotNull(coralTest.ID);

            await TearDown();
        }

        [Test]
        public async Task FindByIdAquarium()
        {

            AquariumItem insertedForFinding = await uow.AquariumItem.InsertOneAsync(testCoral);
            AquariumItem findFromDb = await uow.AquariumItem.FindByIdAsync(insertedForFinding.ID);
            Assert.IsTrue(findFromDb.ID == insertedForFinding.ID);

            await TearDown();
        }

        [Test]
        public async Task GetCorals()
        {
            AquariumItem insertedGettingCorals = await uow.AquariumItem.InsertOneAsync(testCoral);
            List<Coral> gettingAllCorals= new List<Coral>(uow.AquariumItem.GetCorals());
            AquariumItem filterThroughList = gettingAllCorals.Find(i => i.ID == insertedGettingCorals.ID);
            Assert.IsTrue(filterThroughList.ID == insertedGettingCorals.ID);

            await TearDown();
        }

        [Test]
        public async Task GetAnimals()
        {
            AquariumItem insertedGettingAnimals = await uow.AquariumItem.InsertOneAsync(testAnimal1);
            List<Animal> gettingAllAnimals = new List<Animal>(uow.AquariumItem.GetAnimals());
            AquariumItem filterThroughList = gettingAllAnimals.Find(i => i.ID == insertedGettingAnimals.ID);
            Assert.IsTrue(filterThroughList.ID == insertedGettingAnimals.ID);

            await TearDown();

            await uow.AquariumItem.DeleteByIdAsync(insertedGettingAnimals.ID);
        }

        [Test]
        public async Task FindOneAquariumItem()
        {
            AquariumItem insertedForFinding = await uow.AquariumItem.InsertOneAsync(animalForFinding);

            AquariumItem firstFoundAquariumItem = await uow.AquariumItem.FindOneAsync(a => a.Name == "DoriForFinding");
            Assert.IsTrue(firstFoundAquariumItem.ID == insertedForFinding.ID);
        }

        [Test]
        public async Task FilterByAnimal()
        {
            AquariumItem aquariumTestInserted = await uow.AquariumItem.InsertOneAsync(testAnimal1);
            AquariumItem aquariumTestInserted2 = await uow.AquariumItem.InsertOneAsync(testAnimal2);

            IEnumerable<AquariumItem> vikisFische = Enumerable.Empty<AquariumItem>();
            vikisFische = uow.AquariumItem.FilterBy(a => a.Aquarium == "Vikis Fische");
            Assert.IsTrue(vikisFische.First().Aquarium == "Vikis Fische");

            await TearDown();
        }

        [Test]
        public async Task UpdateAnimal()
        {
            AquariumItem aquariumItemUpdate = await uow.AquariumItem.InsertOneAsync(testAnimal1);

            testAnimal1.Amount = 2;
            AquariumItem updatedTestDb = await uow.AquariumItem.UpdateOneAsync(aquariumItemUpdate);
            Assert.IsTrue(updatedTestDb.Amount == 2);

            await uow.Aquarium.DeleteByIdAsync(aquariumItemUpdate.ID);
            await TearDown();
        }

        [Test]
        public async Task InsertOrUpdateAquariumItem()
        {
            AquariumItem testInserted = await uow.AquariumItem.InsertOrUpdateOneAsync(testAnimal1);
            AquariumItem testUpdatedInitial = await uow.AquariumItem.InsertOneAsync(testAnimal1);

            testUpdatedInitial.Amount = 2;

            AquariumItem testUpdated = await uow.AquariumItem.InsertOrUpdateOneAsync(testUpdatedInitial);
            Assert.IsTrue(testUpdated.Amount == 2 && testInserted.Name == "DoriTest");

            await TearDown();
            await uow.AquariumItem.DeleteByIdAsync(testInserted.ID);
            await uow.AquariumItem.DeleteByIdAsync(testUpdated.ID);

        }

        [Test]
        public async Task DeleteManyAquariumItems()
        {
            AquariumItem aquariumItemDeleting1 = await uow.AquariumItem.InsertOneAsync(testAnimal1);
            AquariumItem aquariumItemDeleting2 = await uow.AquariumItem.InsertOneAsync(testAnimal1);

            await uow.AquariumItem.DeleteManyAsync(doc => doc.Name == "DoriTest");
            AquariumItem deletedAquariumItem = await uow.AquariumItem.FindOneAsync(a => a.Name == "DoriTest");
            Assert.IsNull(deletedAquariumItem);

            await TearDown();
        }

        [Test]
        public async Task DelteByIdAquariumItem()
        {
            AquariumItem aquariumTestDeleting = await uow.AquariumItem.InsertOneAsync(testAnimal1);

            await uow.AquariumItem.DeleteByIdAsync(aquariumTestDeleting.ID);
            AquariumItem deletedAquariumItem = await uow.AquariumItem.FindByIdAsync(aquariumTestDeleting.ID);
            Assert.IsNull(deletedAquariumItem);

            await TearDown();
        }

        [Test]
        public async Task FilterByWithProjection()
        {
            Aquarium fromDB = await uow.Aquarium.InsertOneAsync(aquarium);

            await uow.AquariumItem.InsertOneAsync(testCoral1);
            await uow.AquariumItem.InsertOneAsync(testCoral2);
            await uow.AquariumItem.InsertOneAsync(testAnimal1);
            await uow.AquariumItem.InsertOneAsync(testAnimal2);

            List<String> aquariumBiggerTen = uow.AquariumItem.FilterBy(filter => filter.Amount > 10, projection => projection.Species).Cast<String>().ToList();

            Assert.NotNull(uow.Aquarium.FindByIdAsync(aquarium.ID));
            Assert.True(aquariumBiggerTen.Contains(testCoral2.Species) && aquariumBiggerTen.Contains(testAnimal2.Species));
            Assert.True(aquariumBiggerTen.Count == 2);

            await TearDown();
        }
       
    }
}

