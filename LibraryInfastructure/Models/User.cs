using Microsoft.AspNetCore.Identity;

namespace LibraryInfrastructure.Models 
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}