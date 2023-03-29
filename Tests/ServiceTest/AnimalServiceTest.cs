using System;
using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Services;
using Services.ImplementedServices;
using Services.Models.Request;
using Services.Models.Response;
using Tests.DBTests;

namespace Tests.ServiceTest
{
	public class AnimalServiceTest
	{

        UnitOfWork uow = new UnitOfWork();

        Animal testAnimal1 = new Animal();
        Animal testAnimal2 = new Animal();
        Animal animalForFinding = new Animal();
        Animal deadAnimal = new Animal();

        Aquarium aquariumTest = new Aquarium();
        Aquarium aquarium = new Aquarium();

        [SetUp]
        public async Task SetUp()
        {
            aquariumTest = new Aquarium("Vikis Fische", 65, 150, 150, 500, WaterType.Saltwater);
            aquarium = new Aquarium("Vikis Andere Fische", 65, 150, 150, 500, WaterType.Saltwater);

            testAnimal1 = new Animal("Vikis Fische", "DoriTest", "Doktorfisch", 1, "My Little Dori", true);
            testAnimal2 = new Animal("Vikis Empty Fishe", "NemoTest", "Anemonenfisch", 20, "My Little Nemo", true);
            animalForFinding = new Animal("Vikis Fische", "DoriForFinding", "Doktorfisch", 1, "My Little Dori", true);
            deadAnimal = new Animal("Vikis Fische", "DoriForFinding", "Doktorfisch", 1, "My Little Dori", false);

            await uow.Aquarium.InsertOneAsync(aquariumTest);
            await uow.Aquarium.InsertOneAsync(aquarium);
        }

        [TearDown]
        public async Task TearDown()
        {
            await uow.Aquarium.DeleteByIdAsync(aquarium.ID);
            await uow.Aquarium.DeleteByIdAsync(aquariumTest.ID);

            await uow.AquariumItem.DeleteByIdAsync(testAnimal1.ID);
            await uow.AquariumItem.DeleteByIdAsync(testAnimal2.ID);
            await uow.AquariumItem.DeleteByIdAsync(animalForFinding.ID);
            await uow.AquariumItem.DeleteManyAsync(doc => doc.Name == "DoriTest");
        }

        [Test]
        public async Task TestAddAnimal()
        {
            AnimalService animalService = new AnimalService(uow, uow.AquariumItem , null);

            var modelState = new Mock<ModelStateDictionary>();
            await animalService.SetModelState(modelState.Object);

            ItemResponseModel<Animal> fromservice = await animalService.AddAnimal(testAnimal1);
            Assert.NotNull(fromservice);

            await TearDown();

        }

        [Test]
        public async Task TestAddAnimalWithoutAquarium()
        {
            AnimalService animalService = new AnimalService(uow, uow.AquariumItem, null);

            var modelState = new Mock<ModelStateDictionary>();
            await animalService.SetModelState(modelState.Object);

            ItemResponseModel<Animal> fromservice = await animalService.AddAnimal(testAnimal2);

            Assert.IsTrue(fromservice.HasError);

            await TearDown();
        }

        [Test]
        public async Task TestAddDeadAnimal()
        {
            AnimalService animalService = new AnimalService(uow, uow.AquariumItem, null);

            var modelState = new Mock<ModelStateDictionary>();
            await animalService.SetModelState(modelState.Object);

            ItemResponseModel<Animal> fromservice = await animalService.AddAnimal(deadAnimal);

            Assert.IsTrue(fromservice.HasError);

            await TearDown();
        }

        [Test]
        public async Task GetAnimals()
        {
            AnimalService animalService = new AnimalService(uow, uow.AquariumItem, null);

            var modelState = new Mock<ModelStateDictionary>();
            await animalService.SetModelState(modelState.Object);

            AquariumItem insertedGettingCorals = await uow.AquariumItem.InsertOneAsync(testAnimal1);
            await uow.AquariumItem.InsertOneAsync(animalForFinding);

            ItemResponseModel<List<AquariumItem>> gettingAllAnimals = await animalService.GetAnimal(aquariumTest);

            Assert.IsTrue(gettingAllAnimals.Data.First().Aquarium == aquariumTest.Name);

            await TearDown();

        }

        [Test]
        public async Task TestDelete()
        {
            AnimalService animalService = new AnimalService(uow, uow.AquariumItem, null);

            var modelState = new Mock<ModelStateDictionary>();
            await animalService.SetModelState(modelState.Object);

            ItemResponseModel<Animal> fromservice = await animalService.AddAnimal(testAnimal1);
            ActionResponseModel fromserviceDeleted = await animalService.Delete(fromservice.Data.ID);

            Assert.IsTrue(fromserviceDeleted.Success);

        }


        [Test]
        public async Task TestUpdate()
        {
            AnimalService animalService = new AnimalService(uow, uow.AquariumItem, null);

            var modelState = new Mock<ModelStateDictionary>();
            await animalService.SetModelState(modelState.Object);

            ItemResponseModel<Animal> fromservice = await animalService.AddAnimal(testAnimal1);

            fromservice.Data.Name = "updatedName";

            ItemResponseModel<AquariumItem> fromServiceUpdate = await animalService.UpdateHandler(fromservice.Data.ID, fromservice.Data);

            Assert.True(fromServiceUpdate.Data.Name.Equals("updatedName"));

        }
        

    }
}

