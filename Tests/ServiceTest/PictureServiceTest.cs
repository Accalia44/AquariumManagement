using System;
using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Services;
using Services.ImplementedServices;
using Services.Models.Request;
using Services.Models.Response;

namespace Tests.ServiceTest
{
	public class PictureServiceTest
	{

        UnitOfWork uow = new UnitOfWork();
        Coral testCoral = new Coral();

        Animal testAnimal = new Animal();

        Aquarium aquarium = new Aquarium();
        Aquarium aquarium1 = new Aquarium();

        User testUserService = new User();
        UserAquarium userAquarium = new UserAquarium();

        [SetUp]
        public async Task SetUp()
        {
            testUserService = new User("testUserService@mail.com", "TestService", "TestService", "TestPW123");
            testAnimal = new Animal("Vikis Fische", "DoriTest", "Docktorfisch", 1, "My Little Dori", true);
            testCoral = new Coral("Vikis Fische", "TestCorale", "StabCoral", 10, "This is a Stab Coral", CoralType.Hardcoral);
            aquarium = new Aquarium("VikisFische", 65, 150, 150, 500, WaterType.Saltwater);
            aquarium1 = new Aquarium("Vikis Andere Fische", 65, 150, 150, 500, WaterType.Saltwater);

            await uow.User.InsertOneAsync(testUserService);
            await uow.AquariumItem.InsertOneAsync(testAnimal);
            await uow.AquariumItem.InsertOneAsync(testCoral);
            await uow.Aquarium.InsertOneAsync(aquarium);
            await uow.Aquarium.InsertOneAsync(aquarium1);

            userAquarium = new UserAquarium(testUserService.ID, aquarium.ID);

            await uow.UserAquarium.InsertOneAsync(userAquarium);

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

        }

        [Test]
        public async Task UploadPicture()
        {
            PictureService pictureService = new PictureService(uow, uow.Picture, null);

            PictureRequest request = new PictureRequest();
            request.Description = "die kleine Dori";

            byte[] bytes = System.IO.File.ReadAllBytes(@"/Users/viki/Documents/FH/ADV-SWE/AquariumManagement/Pictures/dori.jpg");

            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "image.jpg");
            request.FormFile = file;

            List<Picture> pictures = uow.Picture.FilterBy(x => true).ToList();
            int old = pictures.Count;

            await pictureService.AddPicture("VikisFische", request);

            pictures = uow.Picture.FilterBy(x => true).ToList();
            Assert.AreEqual(pictures.Count, old + 1);
        }

        [Test]
        public async Task UploadPictureWithValidation()
        {
            PictureService pictureService = new PictureService(uow, uow.Picture, null);

            PictureRequest request = new PictureRequest();
            request.Description = "die kleine Dori";

            byte[] bytes = System.IO.File.ReadAllBytes(@"/Users/viki/Documents/FH/ADV-SWE/AquariumManagement/Pictures/dori.jpg");

            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "image.jpg");
            request.FormFile = file;

            List<Picture> pictures = uow.Picture.FilterBy(x => true).ToList();
            int old = pictures.Count;

            ItemResponseModel<PictureResponse> pics = await pictureService.AddPicture("Vikis Fishe", request); 

            pictures = uow.Picture.FilterBy(x => true).ToList();
            Assert.IsFalse(pics.HasError);
        }
        //Get Picture
        //Get Picture For Aquarium
        //Delete Picture
        //Tests schreiben
    }
}

