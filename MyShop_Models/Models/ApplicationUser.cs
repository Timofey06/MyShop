
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace MyShop_Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
