using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RamenCo.Models
{
    public class LoginUser:IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public string Strert { get; set; }
        public string PostalCode { get; set; }
        [NotMapped] //Karşılığı olmayan bir column
        public string Role { get; set; }
    }
}
