using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RamenCo.Models
{
    public class OrderDetails:BaseEntity
    {
        [Required]
        public int OrderID { get; set; }
        [ForeignKey("OrderID")]
        public OrderHeader OrderHeader { get; set; }
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
    }
}
