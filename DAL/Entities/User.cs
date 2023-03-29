using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks; 

namespace DAL.Entities{

    public class User : Entity
    {
        public string? Email { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string Fullname

        {
             get 
            {
                return Firstname+ " " + Lastname;
            }
        }

        [JsonIgnore]
        [BsonIgnore]
        public string? Password { get; }
        [JsonIgnore]
        public string HashedPassword {get;set;} ="";
        public byte[] Salt { get; set; } = new byte[0];



        public Boolean IsActive { get; set; }

        public User(){}
        public User(string email, string firstname, string lastname, string password)
        {
            Email = email;
            Firstname = firstname;
            Lastname = lastname;
            Password = password;
        }
    }
}