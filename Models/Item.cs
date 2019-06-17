using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ContentLimitService.Models
{
    public class Item
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        public float Value { get; set; }
        public int CategoryId { get; set; }
        [Required]
        public string Category { get; set; }
    }

    public class ItemDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public float Value { get; set; }
    }
}