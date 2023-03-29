using System;
using System.Linq.Expressions;
using DAL.Entities;
using MongoDB.Driver;

namespace DAL.Repository.Implementierte
{

	public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DBContext context) : base(context) { }

        public async Task<User> Login(string username, string password)
        {
            User emptyUser = new User();            
            try
            {
                User currentUser = await FindOneAsync(x => x.Email == username);

                bool verifiedUserPassword = PasswordHasher.VerifyPassword(password, currentUser.HashedPassword, currentUser.Salt);
                if(verifiedUserPassword == true)
                {
                    Console.WriteLine("Login Successfull");
                    return currentUser;
                }
                else
                {
                    Console.WriteLine("Password not correct, please try again");
                    return emptyUser;
                }         

            }
            catch(NullReferenceException e)
            {
                Console.WriteLine(e);
                Console.WriteLine("User Not Found, please check if the Username and Password are correct");
                return emptyUser;
            }

        }
        
        public async Task<User> SignUp(User user)
        {
            user.HashedPassword = PasswordHasher.HashPasword(user.Password, out var salt);
            user.Salt = salt;

            return await InsertOneAsync(user);

        }

    }
}

