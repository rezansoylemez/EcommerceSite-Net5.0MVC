using Microsoft.AspNetCore.Http;
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
        public IActionResult Success()
        {
            return View();
        }
        //Sepetteki ürünleri artırmak için.
        public IActionResult BasketAdd(int cartID)
        {
            var cart = _db.ShoppingCarts.FirstOrDefault(a => a.ID == cartID);
            cart.Count += 1;
            _db.SaveChanges();
            //Artma işlemi yapıldıktan sonra aynı sayfa kalsın.
            return RedirectToAction(nameof(Index));
        }
        public IActionResult BasketDecrease(int cartID)
        {
            var cart = _db.ShoppingCarts.FirstOrDefault(a => a.ID == cartID);
            if (cart.Count==1)
            {
                var count = _db.ShoppingCarts.Where(a => a.LoginUserID == cart.LoginUserID).ToList().Count();
                _db.ShoppingCarts.Remove(cart);
                _db.SaveChanges();
                HttpContext.Session.SetInt32(AddRole.SassionShoppingCart, count - 1);
            }
            else
            {
                cart.Count -= 1;
                _db.SaveChanges();
            }
            //Artma işlemi yapıldıktan sonra aynı sayfa kalsın.
            return RedirectToAction(nameof(Index));
        }
        public IActionResult BasketRemove(int cartID)
        {
            var cart = _db.ShoppingCarts.FirstOrDefault(a => a.ID == cartID);

            var count = _db.ShoppingCarts.Where(a => a.LoginUserID == cart.LoginUserID).ToList().Count();
            _db.ShoppingCarts.Remove(cart);
            _db.SaveChanges();
            HttpContext.Session.SetInt32(AddRole.SassionShoppingCart, count - 1);

            //Artma işlemi yapıldıktan sonra aynı sayfa kalsın.
            return RedirectToAction(nameof(Index));
        }
    }
}
