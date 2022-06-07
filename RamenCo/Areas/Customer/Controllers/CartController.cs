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
        public IActionResult Summary()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartViewModel = new ShoppingCartViewModel()
            {
                OrderHeader = new OrderHeader(),
                ListCart = _db.ShoppingCarts.Where(a => a.LoginUserID == claim.Value).Include(i => i.Product)
            };
            foreach (var item in ShoppingCartViewModel.ListCart)
            {
                item.Price = item.Product.Price;
                ShoppingCartViewModel.OrderHeader.OrderTotal += (item.Count * item.Product.Price);

            }
            return View(ShoppingCartViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Summary(ShoppingCartViewModel shoppingCart)
        {
            var claimID = (ClaimsIdentity)User.Identity;
            var claim = claimID.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartViewModel.ListCart = _db.ShoppingCarts.Where(i => i.LoginUserID == claim.Value).Include(i => i.Product);
            ShoppingCartViewModel.OrderHeader.OrderStatus = AddRole.UserWaiting;
            ShoppingCartViewModel.OrderHeader.LoginUserID = claim.Value;
            ShoppingCartViewModel.OrderHeader.OrderTime = DateTime.Now;

            _db.Add(ShoppingCartViewModel.OrderHeader);
            _db.SaveChanges();
            foreach (var item in ShoppingCartViewModel.ListCart)
            {
                item.Price = item.Product.Price;
                OrderDetails orderDetails = new OrderDetails()
                {
                    ProductID = item.ProductID,
                    OrderID = ShoppingCartViewModel.OrderHeader.ID,
                    Price = item.Price,
                    Count = item.Count
                };
                ShoppingCartViewModel.OrderHeader.OrderTotal += item.Count * item.Product.Price;
                shoppingCart.OrderHeader.OrderTotal += item.Count * item.Product.Price;
                _db.OrderDetails.Add(orderDetails);
            }
            _db.ShoppingCarts.RemoveRange(ShoppingCartViewModel.ListCart);
            _db.SaveChanges();
            HttpContext.Session.SetInt32(AddRole.SassionShoppingCart, 0);
            return RedirectToAction("OrderResult");
        }

        public IActionResult OrderResult()
        {
            return View();
        }

        public IActionResult Index(int id)
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

            double discount = _db.Products.Where(a => a.ID==id).Select(a => a.DiscountPercent).FirstOrDefault();
            foreach (var item in ShoppingCartViewModel.ListCart)
            {
                if (item.Product.DiscountPercent>0)
                {
                    if (item.Product.IsFreeShipping)
                    {
                        double total = (((item.Product.Price) * item.Product.DiscountPercent) / 100);
                        double totalDiscount = item.Count * total;
                        double withOutDiscount = (item.Count * item.Product.Price);
                        ShoppingCartViewModel.OrderHeader.OrderTotal += withOutDiscount - totalDiscount;
                    }
                    else
                    {
                        double total = (((item.Product.Price) * item.Product.DiscountPercent) / 100);
                        double totalDiscount = item.Count * total;
                        double withOutDiscount = (item.Count * item.Product.Price);
                        ShoppingCartViewModel.OrderHeader.OrderTotal += withOutDiscount - totalDiscount +item.Product.ProductShipPrice;
                    }
                }
                else
                {
                    if (item.Product.IsFreeShipping)
                    {
                        ShoppingCartViewModel.OrderHeader.OrderTotal += (item.Count * item.Product.Price);
                    }
                    else
                    {
                        ShoppingCartViewModel.OrderHeader.OrderTotal += ((item.Count * item.Product.Price) + (19));
                    }
                }
                
                
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
