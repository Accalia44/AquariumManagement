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

        Aquarium aquariumTest = new Aquarium();
        Aquarium aquarium = new Aquarium();

        User testUser = new User();

        [SetUp]
        public async Task SetUp()
        {
            aquariumTest = new Aquarium("Vikis Fische", 65, 150, 150, 500, WaterType.Saltwater);
            aquarium = new Aquarium("Vikis Andere Fische", 65, 150, 150, 500, WaterType.Saltwater);

            testAnimal1 = new Animal("Vikis Fische", "DoriTest", "Doktorfisch", 1, "My Little Dori", true);
            testAnimal2 = new Animal("Vikis Empty Fishe", "NemoTest", "Anemonenfisch", 20, "My Little Nemo", true);
            animalForFinding = new Animal("Vikis Fische", "DoriForFinding", "Doktorfisch", 1, "My Little Dori", true);
            deadAnimal = new Animal("Vikis Fische", "DoriForFinding", "Doktorfisch", 1, "My Little Dori", false);
            testUser = new User("testUserService@mail.com", "TestService", "TestService", "TestPW123");

            await uow.User.SignUp(testUser);

            //await uow.Aquarium.InsertOneAsync(aquariumTest);
            //await uow.Aquarium.InsertOneAsync(aquarium);
        }


        [Test]
        public async Task TestAddAquarium()
        {
            GlobalService aquariumServiceGlobal = new GlobalService(uow);
            IHttpContextAccessor accessor = new HttpContextAccessor();

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();

            AquariumController aquariumController = new AquariumController(aquariumServiceGlobal, accessor);

            /*claimsPrincipal = accessor.HttpContext.User;

            var identity = (ClaimsIdentity)claimsPrincipal.Identity;


            Claim email = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);*/

            await aquariumController.Create(aquarium);

            Assert.IsTrue(true);
        }
    }
}

