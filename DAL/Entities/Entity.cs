using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Entities{

    public class Entity : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string ID { get; set; }

        public string Generator()
        {
            return ObjectId.GenerateNewId().ToString();
        }
    }


}