using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Entities
{
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(Animal), typeof(Coral))]
    //[JsonConverter(typeof(JsonInheritanceConverter), "discriminator")]
    [KnownType(typeof(Animal))]
    [KnownType(typeof(Coral))]

    public abstract class AquariumItem : Entity
    {
        public AquariumItem() { }

        public string? Aquarium { get; set; }
        public string? Name { get; set; }

        public string? Species { get; set; }
        public DateTime Inserted { get; set; } = DateTime.Now;
        public int Amount { get; set; }
        public string? Description { get; set; }

        public AquariumItem(string aquarium, string name, string species, int amount, string description)
        {
            Aquarium = aquarium;
            Name = name;
            Species = species;
            Amount = amount;
            Description = description;
        }

        public AquariumItem(string aquarium, string name, string species, DateTime inserted, int amount)
        {
            Aquarium = aquarium;
            Name = name;
            Species = species;
            Inserted = inserted;
            Amount = amount;
        }

        public AquariumItem(string aquarium, string name, string species)
        {
            Aquarium = aquarium;
            Name = name;
            Species = species;
        }

    }
}