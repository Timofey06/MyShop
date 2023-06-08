using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_Models.Models
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }
        public string AdminId { get; set; }
        [ForeignKey("AdminId")]
        public ApplicationUser AdminApplicationUser { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public double TotalPrice { get; set; }
        
        public string Status { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string House { get; set; }
        [Required]
        public string Apartament { get; set; }
        [Required]
        public string PostalCode { get; set; }
        public string TransactionId { get; set; }
        public DateTime DateExecution { get; set; }
    }
}
