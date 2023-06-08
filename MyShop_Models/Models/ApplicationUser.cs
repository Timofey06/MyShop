using System;
using MyShop_Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace MyShop_Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        [NotMapped]
        public string City { get; set; }
        [NotMapped]
        public string Street { get; set; }
        [NotMapped]
        public string House { get; set; }
        [NotMapped]
        public string Apartament { get; set; }
        [NotMapped]
        public string PostalCode { get; set; }
       
    }
}
