using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace DAL.Entities
{
    public class Animal : AquariumItem
    {
        public DateTime DeathDate  { get; set; } = DateTime.MinValue;

        public Boolean IsAlive { get; set; }

        public Animal(){}

        public Animal(string aquarium, string name, string species,
            int amount, string description, bool isAlive) :base( aquarium, name, species, amount, description)
        {
            IsAlive = isAlive;
        }

        public Animal(string aquarium, string name, string species,
            int amount, string description, bool isAlive, DateTime deathDate) : base(name, aquarium, species, amount, description)
        {
            IsAlive = isAlive;
            DeathDate = deathDate;
        }


    }
}