using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RamenCo.Models
{
    public class Category:BaseEntity
    {
        [Required]
        public string Name { get; set; }
    }
}
