using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RamenCo.Data;
using RamenCo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RamenCo.Areas.Customer.Controllers
{
    [Area("Customer")] 
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        [BindProperty]
        public ShoppingCartViewModel ShoppingCartViewModel { get; set; }
        public CartController(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartViewModel = new ShoppingCartViewModel()
            {
                OrderHeader = new OrderHeader(),
                ListCart = _db.ShoppingCarts.Where(a => a.LoginUserID == claim.Value).Include(i => i.Product)
            };
            ShoppingCartViewModel.OrderHeader.OrderTotal = 0;
            ShoppingCartViewModel.OrderHeader.LoginUser = _db.LoginUsers.FirstOrDefault(a => a.Id == claim.Value);
            foreach (var item in ShoppingCartViewModel.ListCart)
            {
                ShoppingCartViewModel.OrderHeader.OrderTotal += (item.Count * item.Product.Price);
            }
            return View(ShoppingCartViewModel);
        }
    }
}
