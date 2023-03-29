using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace DAL.Entities{

    public class UserAquarium : Entity{
        public string? UserID { get; set; }
        public string? AquariumID { get; set; }

        public UserAquarium() { }

        public UserAquarium (string userID, string aquariumID)
        {
            UserID = userID;
            AquariumID = aquariumID;
        }
    }
    
    public enum UserRole
    {
        User,
        Admin
    }
}