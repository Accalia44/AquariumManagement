using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Entities
{
    public class Aquarium : Entity
    {
        public string? Name {get; set;}
        public double Depth {get; set;}
        public double Length {get; set;}
        public double Height {get; set;}
        public double Liters {get;set;}

        [BsonRepresentation(BsonType.String)]

        public WaterType WaterType {get; set;}

        public Aquarium() { }
        public Aquarium(string name, double depth, double length, double height, double liters, WaterType waterType)
        {
            Name = name;
            Depth = depth;
            Length = length;
            Height = height;
            Liters = liters;
            WaterType = waterType;
        }

    }
}