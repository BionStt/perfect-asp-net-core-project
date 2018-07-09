using System;
using System.Collections.Generic;
using DAL.Entities.Contracts;
using DAL.Entities.Enums;
using Microsoft.AspNetCore.Identity;

namespace DAL.Entities.Identity
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class User : IdentityUser<int>, IEntity
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }
     }
}
