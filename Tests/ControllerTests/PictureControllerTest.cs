using System;
using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using Services.ImplementedServices;
using Services.Models.Request;
using Services.Models.Response;
using API.Controllers;
using Services;

namespace Tests.ControllerTests
{
    public class PictureControllerTest
    {
        UnitOfWork uow = new UnitOfWork();

        Coral testCoral = new Coral();

        Animal testAnimal = new Animal();

        Aquarium aquarium = new Aquarium();
        Aquarium aquarium1 = new Aquarium();

        User testUserService = new User();
        UserAquarium userAquarium = new UserAquarium();
        Picture testPicture = new Picture();


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
            testUserService = new User("testUser@mail.com", "TestService", "TestService", "TestPW123");
            testAnimal = new Animal("Vikis Fische", "DoriTest", "Docktorfisch", 1, "My Little Dori", true);
            testCoral = new Coral("Vikis Fische", "TestCorale", "StabCoral", 10, "This is a Stab Coral", CoralType.Hardcoral);
            aquarium = new Aquarium("Vikis Fische", 65, 150, 150, 500, WaterType.Saltwater);
            aquarium1 = new Aquarium("Vikis Andere Fische", 65, 150, 150, 500, WaterType.Saltwater);

            await uow.User.InsertOneAsync(testUserService);
            await uow.AquariumItem.InsertOneAsync(testAnimal);
            await uow.AquariumItem.InsertOneAsync(testCoral);
            await uow.Aquarium.InsertOneAsync(aquarium);
            await uow.Aquarium.InsertOneAsync(aquarium1);

            userAquarium = new UserAquarium(testUserService.ID, aquarium.ID);

            await uow.UserAquarium.InsertOneAsync(userAquarium);

            //Adding Picture
            PictureService pictureService = new PictureService(uow, uow.Picture, null);
            PictureRequest request = new PictureRequest();
            request.Description = "die kleine Dori";
            byte[] bytes = System.IO.File.ReadAllBytes(@"/Users/viki/Documents/FH/ADV-SWE/AquariumManagement/Pictures/dori.jpg");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "image.jpg");
            request.FormFile = file;
            ItemResponseModel<PictureResponse> pics = await pictureService.AddPicture("Vikis Fishe", request);
            testPicture = pics.Data.Picture;

        }

        [TearDown]
        public async Task TearDown()
        {
            await uow.Aquarium.DeleteByIdAsync(aquarium.ID);
            await uow.Aquarium.DeleteByIdAsync(aquarium1.ID);
            await uow.AquariumItem.DeleteByIdAsync(testAnimal.ID);
            await uow.AquariumItem.DeleteByIdAsync(testCoral.ID);
            await uow.User.DeleteByIdAsync(testUserService.ID);
            await uow.UserAquarium.DeleteByIdAsync(userAquarium.ID);
            await uow.Picture.DeleteByIdAsync(testPicture.ID);
        }


        [Test]
        public async Task UploadPicture()
        {
            GlobalService serviceGlobal = new GlobalService(uow);

            PictureController pictureController = new PictureController(serviceGlobal, Create(Login("testUser@mail.com")));

            PictureRequest request = new PictureRequest();
            request.Description = "die kleine Dori";

            byte[] bytes = System.IO.File.ReadAllBytes(@"/Users/viki/Documents/FH/ADV-SWE/AquariumManagement/Pictures/dori.jpg");

            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "image.jpg");
            request.FormFile = file;

            ItemResponseModel<PictureResponse> response = await pictureController.Create("VikisFische", request);

            Assert.IsTrue(response.Data.Picture.Description.Equals(request.Description));
            await uow.Picture.DeleteByIdAsync(response.Data.Picture.ID);

        }

        //Error not found
        [Test]
        public async Task GetPicture()
        {
            GlobalService serviceGlobal = new GlobalService(uow);

            PictureController pictureController = new PictureController(serviceGlobal, Create(Login("testUser@mail.com")));

            ItemResponseModel<PictureResponse> foundPicture = await pictureController.GetPicture(testPicture.ID);

            Assert.IsTrue(foundPicture.Data.Picture.ID.Equals(testPicture.ID));

        }

        //Error System.InvalidOperationException : Sequence contains no elements
        [Test]
        public async Task GetPictureForAquarium()
        {
            GlobalService serviceGlobal = new GlobalService(uow);

            PictureController pictureController = new PictureController(serviceGlobal, Create(Login("testUser@mail.com")));

            ItemResponseModel<List<PictureResponse>> foundPics = await pictureController.GetForAquarium(aquarium.Name);

            Assert.IsTrue(foundPics.Data.First().Picture.Aquarium.Equals(aquarium.Name));

        }

        [Test]
        public async Task DeletePicture()
        {
            GlobalService serviceGlobal = new GlobalService(uow);

            PictureController pictureController = new PictureController(serviceGlobal, Create(Login("testUser@mail.com")));

            ActionResponseModel deleted = await pictureController.Delete(testPicture.ID);

            Assert.IsTrue(deleted.Success);


        }
    }
}

