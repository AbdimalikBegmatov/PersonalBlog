using Microsoft.AspNetCore.Identity;
using System;

namespace PersonalBlog.Models
{
    public class User:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DayOfBirth { get; set; }
        public DateTime CreatedDate { get; set; }=DateTime.Now;
    }
}
