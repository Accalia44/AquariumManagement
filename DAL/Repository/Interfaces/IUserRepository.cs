using System;
using DAL.Entities;

namespace DAL.Repository.Implementierte
{
	public interface IUserRepository : IRepository<User>
    {
        Task<User> Login(string username, string password);
        Task<User> SignUp(User user);

    }
}

