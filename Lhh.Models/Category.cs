﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lhh.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(0, 100, ErrorMessage = "Display Order must be between 1 and 100!")]
        public int DisplayOrder { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
