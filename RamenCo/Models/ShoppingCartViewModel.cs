using System;
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;

namespace RamenCo.Models
{
    public class ShoppingCartViewModel
    {
        public IEnumerable<ShoppingCart> ListCart { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
