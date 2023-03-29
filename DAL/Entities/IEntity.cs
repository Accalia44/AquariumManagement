using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Entities
{

    public interface IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        string ID {get; set;}
        string Generator();

        //Generator ist GenerateID


    }
}