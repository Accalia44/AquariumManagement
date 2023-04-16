using System;
using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using API.Controllers;
using Services;
using Services.Models.Response;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Services.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace Tests.ControllerTests
{
	public class UserControllerTest
	{

        UnitOfWork uow = new UnitOfWork();

        User testUser = new User();


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
            testUser = new User("testUser@mail.com", "TestService", "TestService", "TestPW123");

            await uow.User.SignUp(testUser);

        }

        [TearDown]
        public async Task TearDown()
        {
            await uow.User.DeleteByIdAsync(testUser.ID);

        }

        [Test]
        public async Task Login()
        {
            GlobalService serviceGlobal = new GlobalService(uow);

            UserController userController = new UserController(serviceGlobal, Create(Login("testUser@mail.com")));

            LoginRequest loginRequest = new LoginRequest("testUser@mail.com", "TestPW123");
            
            ActionResult<ItemResponseModel<UserResponse>> loginSuccess = await userController.Login(loginRequest);

            Assert.That(testUser.Email, Is.EqualTo(loginSuccess.Value.Data.User.Email));

            await TearDown();
        }


    }
}

