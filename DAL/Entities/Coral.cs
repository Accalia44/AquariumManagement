using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using static MongoDB.Driver.WriteConcern;

namespace DAL.Entities
{
    public class Coral : AquariumItem 
    {
        [BsonRepresentation(BsonType.String)]
        public CoralType CoralType {get; set;}

        public Coral() { }

        public Coral(string aquarium, string name, string species,  int amount, string description, CoralType coralType) :
            base(aquarium, name, species, amount, description)
        {
            CoralType = coralType;
        }
        public Coral(string aquarium, string name, string species, int amount, string description) : base(aquarium, name, species, amount, description)
        {
        }
    }

    public enum CoralType 
    {
        Hardcoral,
        Softcoral
    }

}