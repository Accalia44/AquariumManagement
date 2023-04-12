using System;
using DAL;
using DAL.Entities;
using Services.Authentications;

namespace Tests
{
	public class LoginTest
	{
		UnitOfWork uow = new UnitOfWork();
		User testUserLogin = new User();
		 
		[SetUp]
		public void SetUp()
		{
            testUserLogin = new User("testUserLogin@mail.com", "Test", "Test", "TestPW123");
		}

		[Test]
		public async Task Login()
		{
            User userFromDB = await uow.User.SignUp(testUserLogin);

            User testUserLoggedIn = await uow.User.Login("testUserLogin@mail.com", "TestPW123");

			Authentication auth = new Authentication(uow);
			AuthenticationInformation info = await auth.Authenticate(testUserLoggedIn);
			Assert.NotNull(info);

			await uow.User.DeleteByIdAsync(testUserLoggedIn.ID);

        }

	}
}

