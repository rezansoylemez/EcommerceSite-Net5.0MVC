using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RamenCo.Models
{
    public class ShoppingCart:BaseEntity
    {
        public ShoppingCart()
        {
            Count = 1;
        }
        public string LoginUserID { get; set; }
        [ForeignKey("LoginUserID")]
        public LoginUser LoginUser { get; set; }
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
        public int Count { get; set; }
        [NotMapped]
        public double Price { get; set; }
    }
}
