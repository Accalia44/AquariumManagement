using System;
using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using API.Controllers;
using Services;
using Services.Models.Response;
using System.Drawing;

namespace Tests.ControllerTests
{
	public class AquariumItemControllerTest
	{
        UnitOfWork uow = new UnitOfWork();

        Animal testAnimal1 = new Animal();
        Animal testAnimal2 = new Animal();

        Coral testCoral = new Coral();
        Coral testCoral1 = new Coral();
        Coral testCoral2 = new Coral();

        Aquarium aquarium = new Aquarium();
        Aquarium aquarium1 = new Aquarium();

        User testUser = new User();

        UserAquarium userAquarium = new UserAquarium();

        public IHttpContextAccessor Create(ClaimsPrincipal c)

        {
            var mock = new Mock<IHttpContextAccessor>();
            mock.Setup(o => o.HttpContext.User).Returns(c);
            return mock.Object;

        }
        public ClaimsPrincipal Login(string username)

        {
            Claim claim = new Claim(ClaimTypes.Email, username);
            List<Claim> claims = new List<Claim>();
            claims.Add(claim);
            ClaimsIdentity id = new ClaimsIdentity(claims);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(id);
            return claimsPrincipal;
        }

        [SetUp]
        public async Task SetUp()
        {
            aquarium = new Aquarium("Vikis Fische", 65, 150, 150, 500, WaterType.Saltwater);
            aquarium1 = new Aquarium("Vikis Andere Fische", 65, 150, 150, 500, WaterType.Saltwater);

            testAnimal1 = new Animal("Vikis Fische", "DoriTest", "Doktorfisch", 1, "My Little Dori", true);
            testAnimal2 = new Animal("Vikis Empty Fishe", "NemoTest", "Anemonenfisch", 20, "My Little Nemo", true);

            testCoral = new Coral("Vikis Fische", "TestCorale0", "TigerCoral", 20, "This is a Tiger Coral", CoralType.Softcoral);
            testCoral1 = new Coral("Vikis Fische", "TestCorale1", "CactusCoral", 10, "This is a Cactus Coral", CoralType.Hardcoral);
            testCoral2 = new Coral("Vikis Fische", "TestCorale2", "TigerCoral", 20, "This is a Tiger Coral", CoralType.Softcoral);


            testUser = new User("testUser@mail.com", "TestService", "TestService", "TestPW123");

            await uow.Aquarium.InsertOneAsync(aquarium1);
            await uow.Aquarium.InsertOneAsync(aquarium);

            await uow.AquariumItem.InsertOneAsync(testCoral1);
            await uow.AquariumItem.InsertOneAsync(testCoral2);

            await uow.AquariumItem.InsertOneAsync(testAnimal1);
            await uow.AquariumItem.InsertOneAsync(testAnimal2);

            
            await uow.AquariumItem.InsertOneAsync(testCoral);

            await uow.User.SignUp(testUser);

            userAquarium = new UserAquarium(testUser.ID, aquarium.ID);

            await uow.UserAquarium.InsertOneAsync(userAquarium);
        }

        [TearDown]
        public async Task TearDown()
        {
            await uow.Aquarium.DeleteByIdAsync(aquarium.ID);
            await uow.Aquarium.DeleteByIdAsync(aquarium1.ID);

            await uow.AquariumItem.DeleteByIdAsync(testAnimal1.ID);
            await uow.AquariumItem.DeleteByIdAsync(testAnimal2.ID);

            await uow.AquariumItem.DeleteByIdAsync(testCoral.ID);
            await uow.AquariumItem.DeleteByIdAsync(testCoral1.ID);
            await uow.AquariumItem.DeleteByIdAsync(testCoral2.ID);


            await uow.User.DeleteByIdAsync(testUser.ID);

            await uow.UserAquarium.DeleteByIdAsync(userAquarium.ID);

        }
        //Error
        [Test]
        public async Task TestAddCoral()
        {
            GlobalService aquariumServiceGlobal = new GlobalService(uow);

            AquariumItemController aquariumItemController = new AquariumItemController(aquariumServiceGlobal, Create(Login("testUser@mail.com")));

            ItemResponseModel<Coral> insertedCoral = await aquariumItemController.Create(testCoral);

            Assert.IsTrue(!String.IsNullOrEmpty(insertedCoral.Data.Name));

            //await TearDown();
        }

        [Test]
        public async Task TestGetCoral()
        {
            GlobalService aquariumServiceGlobal = new GlobalService(uow);

            AquariumItemController aquariumItemController = new AquariumItemController(aquariumServiceGlobal, Create(Login("testUser@mail.com")));

            ItemResponseModel<AquariumItem> foundCoral = await aquariumItemController.Get(testCoral1.ID);

            Assert.IsTrue(testCoral.ID.Equals(foundCoral.Data.ID));

            await TearDown();
        }

        [Test]
        public async Task TestGetAllCoral()
        {
            GlobalService aquariumServiceGlobal = new GlobalService(uow);

            AquariumItemController aquariumItemController = new AquariumItemController(aquariumServiceGlobal, Create(Login("testUser@mail.com")));

            ItemResponseModel<List<Coral>> found = await aquariumItemController.GetAllCorals(aquarium);

            Assert.That(found.Data.First().Aquarium, Is.EqualTo(aquarium.Name));

            await TearDown();
        }

        [Test]
        public async Task TestGetAllAnimal()
        {
            GlobalService aquariumServiceGlobal = new GlobalService(uow);

            AquariumItemController aquariumItemController = new AquariumItemController(aquariumServiceGlobal, Create(Login("testUser@mail.com")));

            ItemResponseModel<List<Animal>> found = await aquariumItemController.GetAllAnimals(aquarium);

            Assert.That(found.Data.First().Aquarium, Is.EqualTo(aquarium.Name));

            await TearDown();
        }

        [Test]
        public async Task Edit()
        {
            GlobalService aquariumServiceGlobal = new GlobalService(uow);

            AquariumItemController aquariumItemController = new AquariumItemController(aquariumServiceGlobal, Create(Login("testUser@mail.com")));

            testAnimal1.Name = "updatedName";

            ItemResponseModel<AquariumItem> found = await aquariumItemController.Edit(testAnimal1.ID, testAnimal1);

            Assert.That(found.Data.Name, Is.EqualTo(testAnimal1.Name));

            await TearDown();
        }
    }
}

