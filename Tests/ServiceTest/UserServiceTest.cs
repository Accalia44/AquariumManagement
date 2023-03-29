using System;
using System.Security.Cryptography.Xml;
using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using Services;
using Services.Models.Response;
using Services.Models.Request;

namespace Tests.ServiceTest
{
	public class UserServiceTest
	{
		UnitOfWork uow = new UnitOfWork();
        User testUserService = new User();
        User testUserServiceUpdate = new User();
        User testUserExist = new User();
        User testUserService2 = new User();
        User testUserServiceLogin = new User();

        [SetUp]
        public async Task SetUp()
        {
            testUserService = new User("testUserService@mail.com", "TestService", "TestService", "TestPW123");
            testUserServiceUpdate = new User("testUserServiceUpdate1@mail.com", "TestService", "TestService", "TestPW123");
            testUserService2 = new User("testUserServiceUpdate1@mail.com", "TestService", "TestService", "TestPW123");
            testUserExist = new User("testUserServiceUpdate2@mail.com", "TestService", "TestService", "TestPW123");
            testUserServiceLogin = new User("testUserServiceLogin@mail.com", "TestService", "TestService", "TestPW123");
        }

        [TearDown]
        public async Task TearDown()
        {
            await uow.User.DeleteByIdAsync(testUserService.ID);
            await uow.User.DeleteByIdAsync(testUserServiceUpdate.ID);
            await uow.User.DeleteByIdAsync(testUserService2.ID);
            await uow.User.DeleteByIdAsync(testUserExist.ID);
            await uow.User.DeleteByIdAsync(testUserServiceLogin.ID);

        }

        [Test]
		public async Task TestInsert()
		{
			UserServices userservice = new UserServices(uow, uow.User, null);

            var modelState = new Mock<ModelStateDictionary>();
			await userservice.SetModelState(modelState.Object);

			ItemResponseModel<User> fromservice = await userservice.CreateHandler(testUserService);
			Assert.NotNull(fromservice);

            await uow.User.DeleteByIdAsync(fromservice.Data.ID);

        }

        [Test]
        public async Task TestDelete()
        {
            UserServices userservice = new UserServices(uow, uow.User, null);

            var modelState = new Mock<ModelStateDictionary>();
            await userservice.SetModelState(modelState.Object);

            ItemResponseModel<User> fromservice = await userservice.CreateHandler(testUserService);
            ActionResponseModel fromserviceDeleted = await userservice.Delete(fromservice.Data.ID);

            Assert.IsTrue(fromserviceDeleted.Success);

        }


        [Test]
		public async Task TestUpdate()
		{
            UserServices userservice = new UserServices(uow, uow.User, null);

            var modelState = new Mock<ModelStateDictionary>();
            await userservice.SetModelState(modelState.Object);

            ItemResponseModel<User> fromservice = await userservice.CreateHandler(testUserServiceUpdate);

            User createdUser = new User();
            createdUser = fromservice.Data;

            createdUser.Email = "newUpdatedEmail@mail.com";

            ItemResponseModel<User> fromServiceUpdate = await userservice.UpdateHandler(createdUser.ID, createdUser);

            Assert.True(fromServiceUpdate.Data.Email.Equals(createdUser.Email));
            await uow.User.DeleteByIdAsync(createdUser.ID);

        }


        [Test]
        public async Task TestUpdateFail()
        {
            UserServices userservice = new UserServices(uow, uow.User, null);


            var modelState = new Mock<ModelStateDictionary>();
            await userservice.SetModelState(modelState.Object);

            ItemResponseModel<User> fromservice = await userservice.CreateHandler(testUserService2);
            ItemResponseModel<User> fromserviceExist = await userservice.CreateHandler(testUserExist);

            User createdUser = new User();
            createdUser = fromservice.Data;

            createdUser.Email = "testUserServiceUpdate2@mail.com";

            try
            {
                ItemResponseModel<User> fromServiceUpdate = await userservice.UpdateHandler(createdUser.ID, createdUser);
                Assert.True(fromServiceUpdate.Data.Email.Equals("testUserServiceUpdate1@mail.com"));

            }
            catch (NullReferenceException e)
            {
                await uow.User.DeleteByIdAsync(createdUser.ID);
                await uow.User.DeleteByIdAsync(fromserviceExist.Data.ID);
                Console.WriteLine(e);
            }

        }


        [Test]
        public async Task TestLogin()
        {
            UserServices userservice = new UserServices(uow, uow.User, null);

            var modelState = new Mock<ModelStateDictionary>();
            await userservice.SetModelState(modelState.Object);
            await userservice.CreateHandler(testUserServiceLogin);

            LoginRequest loginRequest = new LoginRequest("testUserServiceLogin@mail.com", "TestPW123");

            ItemResponseModel<UserResponse> fromServiceLogin = await userservice.Login(loginRequest);

            Assert.IsNotEmpty(fromServiceLogin.Data.AuthenticationInformation.ToString());

        }
    }
}

