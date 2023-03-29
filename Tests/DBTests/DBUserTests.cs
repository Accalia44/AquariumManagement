using System;
using System.Runtime.ConstrainedExecution;
using DAL;
using DAL.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Tests.DBTests
{
    public class UserTests : BaseUnitTests
    {
        //Annahme das Username ID ist da Username unique sein muss

        UnitOfWork uow = new UnitOfWork();
        User testUserUpdate = new User();
        User testUser = new User();

        [SetUp]
        public async Task SetUp()
        {
            testUserUpdate = new User("test@mail.com", "Test", "Test", "TestPW123");
            testUser = new User("test@mail.com", "Test", "Test", "TestPW123");
        }

        [Test]
        public async Task SignUpUser()
        {
            User testUser1 = new User("test@mail.com", "Viktoria", "Test", "TestPW1223");
            User userFromDB = await uow.User.SignUp(testUser1);
            Assert.NotNull(userFromDB);

            await uow.User.DeleteByIdAsync(userFromDB.ID);
        }

        [Test]
		public async Task CreateUser()
		{
            User Viktoria = new User("test@mail.com", "Viktoria", "Test", "TestPW1223");
            Viktoria.HashedPassword = PasswordHasher.HashPasword(Viktoria.Password, out var salt);
            Viktoria.Salt = salt;

            User userFromDB = await uow.User.InsertOneAsync(Viktoria);

            Assert.NotNull(userFromDB);

            await uow.User.DeleteByIdAsync(userFromDB.ID);
        }


        [Test]
		public async Task UserLoginCorrect()
		{
            testUser.HashedPassword = PasswordHasher.HashPasword(testUser.Password, out var salt);
            testUser.Salt = salt;
            User testUserDB = await uow.User.InsertOneAsync(testUser);

			User loggedInUser = await uow.User.Login(testUserDB.Email, password: testUser.Password);

			Assert.True(loggedInUser.HashedPassword == testUser.HashedPassword);

            await uow.User.DeleteByIdAsync(testUserDB.ID);
		}

		[Test]
		public async Task UserLoginIncorrectPassword()
		{

            testUser.HashedPassword = PasswordHasher.HashPasword(testUser.Password, out var salt);
            testUser.Salt = salt;
            User testUserDB = await uow.User.InsertOneAsync(testUser);

            User failedLogInUser = await uow.User.Login(testUserDB.ID, "WrongPW123");

            Assert.IsNull(failedLogInUser.ID);
            await uow.User.DeleteByIdAsync(testUserDB.ID);
        }

        [Test]
        public async Task UserLoginIncorrectUsername()
        {
            User userIdNotExisting = await uow.User.Login("SomeTestID", "WrongPW123");

            Assert.IsNull(userIdNotExisting.ID);

        }
        [Test]
        public async Task UpdateUser()
        {
            await uow.User.InsertOneAsync(testUserUpdate);

            testUserUpdate.Email = "testUpdate@gmail.com";
            User updatedTestDbUser = await uow.User.UpdateOneAsync(testUserUpdate);

            Assert.IsTrue(updatedTestDbUser.Email == "testUpdate@gmail.com");
            await uow.User.DeleteByIdAsync(updatedTestDbUser.ID);

        }

        [Test]
        public async Task DeleteUser()
        {
            User TestUser = new User("test@mail.com", "Test", "Test", "TestPW123");
            TestUser.HashedPassword = PasswordHasher.HashPasword(TestUser.Password, out var salt);
            TestUser.Salt = salt;

            User testUserDB = await uow.User.InsertOneAsync(TestUser);
            await uow.User.DeleteByIdAsync(testUserDB.ID);
            User deletedUser = await uow.User.FindByIdAsync(testUserDB.ID);
            Assert.IsNull(deletedUser);
        }
    }
}

