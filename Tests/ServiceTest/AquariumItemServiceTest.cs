using System;
using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Services;
using Services.ImplementedServices;
using Services.Models.Response;

namespace Tests.ServiceTest
{
	public class AquariumItemServiceTest
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

            testAnimal1 = new Animal("Vikis Fische", "DoriTest", "Docktorfisch", 1, "My Little Dori", true);
            testAnimal2 = new Animal("Vikis Fische", "NemoTest", "Anemonenfisch", 20, "My Little Nemo", true);
            animalForFinding = new Animal("Vikis Fische", "DoriForFinding", "Docktorfisch", 1, "My Little Dori", true);

            testCoral = new Coral("Vikis Fische", "TestCorale", "StabCoral", 10, "This is a Stab Coral", CoralType.Hardcoral);
            testCoral1 = new Coral("Vikis Fische", "TestCorale", "CactusCoral", 10, "This is a Cactus Coral", CoralType.Hardcoral);
            testCoral2 = new Coral("Vikis Fische", "TestCorale", "TigerCoral", 20, "This is a Tiger Coral", CoralType.Softcoral);
            Aquarium aquarium = new Aquarium("VikisFische", 65, 150, 150, 500, WaterType.Saltwater);
        }

        [Test]
        public async Task TestInsert()
        {
            AquariumItemService aquariumItemService = new AquariumItemService(uow, uow.AquariumItem, null);

            var modelState = new Mock<ModelStateDictionary>();
            await aquariumItemService.SetModelState(modelState.Object);

            ItemResponseModel<AquariumItem> fromservice = await aquariumItemService.CreateHandler(testAnimal1);
            Assert.NotNull(fromservice);

            await uow.AquariumItem.DeleteByIdAsync(fromservice.Data.ID);

        }

        [Test]
        public async Task TestDelete()
        {
            AquariumItemService aquariumItemService = new AquariumItemService(uow, uow.AquariumItem, null);

            var modelState = new Mock<ModelStateDictionary>();
            await aquariumItemService.SetModelState(modelState.Object);

            ItemResponseModel<AquariumItem> fromservice = await aquariumItemService.CreateHandler(testCoral);
            ActionResponseModel fromserviceDeleted = await aquariumItemService.Delete(fromservice.Data.ID);

            Assert.IsTrue(fromserviceDeleted.Success);

        }

        [Test]
        public async Task TestUpdate()
        {
            AquariumItemService aquariumItemService = new AquariumItemService(uow, uow.AquariumItem, null);

            var modelState = new Mock<ModelStateDictionary>();
            await aquariumItemService.SetModelState(modelState.Object);

            ItemResponseModel<AquariumItem> fromservice = await aquariumItemService.CreateHandler(testAnimal1);

            fromservice.Data.Name = "updatedName";

            ItemResponseModel<AquariumItem> fromServiceUpdate = await aquariumItemService.UpdateHandler(fromservice.Data.ID, fromservice.Data);

            Assert.That(fromServiceUpdate.Data.Name, Is.EqualTo("updatedName"));
            await uow.User.DeleteByIdAsync(fromservice.Data.ID);

        }

    }
}

