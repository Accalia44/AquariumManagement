using System;
using DAL.Entities;

namespace DAL.Repository.Implementierte
{
    public interface IAquariumRepository : IRepository<Aquarium>
    {
        Task<Aquarium> GetByName(string name);
    }
}

