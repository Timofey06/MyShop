using Microsoft.AspNetCore.Identity;

namespace MyShop.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
