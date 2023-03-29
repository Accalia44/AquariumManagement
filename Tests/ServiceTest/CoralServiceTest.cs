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
    public class CoralServiceTest
    {

        UnitOfWork uow = new UnitOfWork();

        Coral testCoral = new Coral();
        Coral testCoral1 = new Coral();
        Coral testCoral2 = new Coral();
        Coral emptyCoralTypeCoral = new Coral();

        Aquarium aquariumTest = new Aquarium();
        Aquarium aquarium = new Aquarium();

        [SetUp]
        public async Task SetUp()
        {
            aquariumTest = new Aquarium("Vikis Fische", 65, 150, 150, 500, WaterType.Saltwater);
            aquarium = new Aquarium("Vikis Andere Fische", 65, 150, 150, 500, WaterType.Saltwater);


            testCoral = new Coral("Vikis Fische", "TestCorale0", "TigerCoral", 20, "This is a Tiger Coral", CoralType.Softcoral);
            testCoral1 = new Coral("Vikis Fische", "TestCorale1", "CactusCoral", 10, "This is a Cactus Coral", CoralType.Hardcoral);
            testCoral2 = new Coral("Vikis Leere Fische", "TestCorale2", "TigerCoral", 20, "This is a Tiger Coral", CoralType.Softcoral);

            await uow.Aquarium.InsertOneAsync(aquariumTest);
            await uow.Aquarium.InsertOneAsync(aquarium);

        }

        [TearDown]
        public async Task TearDown()
        {
            await uow.Aquarium.DeleteByIdAsync(aquarium.ID);
            await uow.Aquarium.DeleteByIdAsync(aquariumTest.ID);

            await uow.AquariumItem.DeleteByIdAsync(testCoral.ID);
            await uow.AquariumItem.DeleteByIdAsync(testCoral1.ID);
            await uow.AquariumItem.DeleteByIdAsync(testCoral2.ID);
        }

        [Test]
        public async Task TestAddCoral()
        {
            CoralService coralService = new CoralService(uow, uow.AquariumItem, null);

            var modelState = new Mock<ModelStateDictionary>();
            await coralService.SetModelState(modelState.Object);

            ItemResponseModel<Coral> fromservice = await coralService.AddCoral(testCoral);
            Assert.NotNull(fromservice);

            await TearDown();

        }

        [Test]
        public async Task TestAddCoralWithoutAquarium()
        {
            CoralService coralService = new CoralService(uow, uow.AquariumItem, null);

            var modelState = new Mock<ModelStateDictionary>();
            await coralService.SetModelState(modelState.Object);

            ItemResponseModel<Coral> fromservice = await coralService.AddCoral(testCoral2);

            Assert.IsTrue(fromservice.HasError);

            await TearDown();
        }

        
        [Test]
        public async Task GetCorals()
        {
            CoralService coralService = new CoralService(uow, uow.AquariumItem, null);

            var modelState = new Mock<ModelStateDictionary>();
            await coralService.SetModelState(modelState.Object);

            AquariumItem insertedGettingCorals = await uow.AquariumItem.InsertOneAsync(testCoral1);
            await uow.AquariumItem.InsertOneAsync(testCoral2);

            ItemResponseModel<List<AquariumItem>> gettingAllCorals = await coralService.GetCoral(aquariumTest);

            Assert.IsTrue(gettingAllCorals.Data.First().Aquarium == aquariumTest.Name);

            await TearDown();

        }

        [Test]
        public async Task TestDelete()
        {
            CoralService coralService = new CoralService(uow, uow.AquariumItem, null);

            var modelState = new Mock<ModelStateDictionary>();
            await coralService.SetModelState(modelState.Object);

            ItemResponseModel<Coral> fromservice = await coralService.AddCoral(testCoral1);
            ActionResponseModel fromserviceDeleted = await coralService.Delete(fromservice.Data.ID);

            Assert.IsTrue(fromserviceDeleted.Success);

            await TearDown();

        }


        [Test]
        public async Task TestUpdate()
        {
            CoralService coralService = new CoralService(uow, uow.AquariumItem, null);

            var modelState = new Mock<ModelStateDictionary>();
            await coralService.SetModelState(modelState.Object);

            ItemResponseModel<Coral> fromservice = await coralService.AddCoral(testCoral1);

            fromservice.Data.Name = "updatedName";

            ItemResponseModel<AquariumItem> fromServiceUpdate = await coralService.UpdateHandler(fromservice.Data.ID, fromservice.Data);

            Assert.True(fromServiceUpdate.Data.Name.Equals("updatedName"));

            await TearDown();

        }


    }
}

