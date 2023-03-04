﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_Models.Models
{
    public class QueryDetail
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int QueryHeaderId { get; set; }
        [ForeignKey("QueryHeaderId")]
        public QueryHeader QueryHeader { get; set; }
        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
