using System;
using System.Security.Claims;
using API.Controllers;
using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Serilog;
using Services;
using Services.ImplementedServices;
using Services.Models.Request;
using Services.Models.Response;
using Tests.DBTests;


namespace Tests.ControllerTests
{
    public class AquariumControllerTest
    {
        UnitOfWork uow = new UnitOfWork();

        Animal testAnimal1 = new Animal();
        Animal testAnimal2 = new Animal();
        Animal animalForFinding = new Animal();
        Animal deadAnimal = new Animal();

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
            animalForFinding = new Animal("Vikis Fische", "DoriForFinding", "Doktorfisch", 1, "My Little Dori", true);
            deadAnimal = new Animal("Vikis Fische", "DoriForFinding", "Doktorfisch", 1, "My Little Dori", false);

            testUser = new User("testUser@mail.com", "TestService", "TestService", "TestPW123");

            await uow.User.SignUp(testUser);

            await uow.Aquarium.InsertOneAsync(aquarium1);
            await uow.Aquarium.InsertOneAsync(aquarium);

            userAquarium = new UserAquarium(testUser.ID, aquarium.ID);

            await uow.UserAquarium.InsertOneAsync(userAquarium);
        }
        
        [TearDown]
        public async Task TearDown()
        {
            await uow.Aquarium.DeleteByIdAsync(aquarium.ID);
            await uow.Aquarium.DeleteByIdAsync(aquarium1.ID);
            await uow.AquariumItem.DeleteByIdAsync(testAnimal1.ID);
            await uow.User.DeleteByIdAsync(testUser.ID);
            await uow.UserAquarium.DeleteByIdAsync(userAquarium.ID);

        }

        [Test]
        public async Task TestAddAquarium()
        {

            GlobalService aquariumServiceGlobal = new GlobalService(uow);

            AquariumController aquariumController = new AquariumController(aquariumServiceGlobal, Create(Login("testUser@mail.com")));

            ItemResponseModel<Aquarium> insertAquarium = await aquariumController.CreateAquarium(aquarium);

            Assert.IsTrue(!String.IsNullOrEmpty(insertAquarium.Data.Name));

            await TearDown();
        }

        [Test]
        public async Task TestGetAquarium()
        {
            GlobalService aquariumServiceGlobal = new GlobalService(uow);

            AquariumController aquariumController = new AquariumController(aquariumServiceGlobal, Create(Login("testUser@mail.com")));

            ItemResponseModel<Aquarium> foundAquarium = await aquariumController.GetAquarium(aquarium.ID);

            Assert.IsTrue(aquarium.ID.Equals(foundAquarium.Data.ID));
        }

        [Test]
        public async Task TestUpdateAquarium()
        {
            GlobalService aquariumServiceGlobal = new GlobalService(uow);

            AquariumController aquariumController = new AquariumController(aquariumServiceGlobal, Create(Login("testUser@mail.com")));

            aquarium.Name = "Updated Name";

            ItemResponseModel<Aquarium> foundAquarium = await aquariumController.Edit(aquarium.ID, aquarium);

            Assert.IsTrue(aquarium.Name.Equals(foundAquarium.Data.Name));
        }

        [Test]
        public async Task TestGetForUser()
        {
            GlobalService aquariumServiceGlobal = new GlobalService(uow);

            AquariumController aquariumController = new AquariumController(aquariumServiceGlobal, Create(Login("testUser@mail.com")));

            ItemResponseModel<List<Aquarium>> foundAquariums = await aquariumController.ForUser(userName: testUser.Email);

            Console.WriteLine(foundAquariums.Data.First().Name);

            Assert.That(foundAquariums.Data.First().Name, Is.EqualTo("Vikis Fische"));
        }
    }
}

