using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RamenCo.Data;
using RamenCo.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RamenCo.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        Product product;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            product = new Product();
            _db = db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var products = _db.Products.Where(a => a.IsHome).ToList();
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //Sepete urun ekleyen kullanıcılar
            if (claim!=null)
            {
                var count = _db.ShoppingCarts.Where(a => a.LoginUserID == claim.Value).ToList().Count();
                HttpContext.Session.SetInt32(AddRole.SassionShoppingCart, count);
            }
            return View(products);
        }
        //Urun detayi tıklandıgında diğer sayfaya ıd göndermek
        public IActionResult Details(int id)
        {
            var product = _db.Products.FirstOrDefault(a => a.ID == id);
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                Product=product,
                ProductID=product.ID,
            };
            return View(shoppingCart);
        }
        //Urunlerin sepete eklenmesi
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Kullanıcı girişi yapılmadan septe eklenmemesi için
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            shoppingCart.ID = 0;
            if (ModelState.IsValid )
            {
                var claimIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
                shoppingCart.LoginUserID = claim.Value;
                //kart boş ise db e sepete ekliyorum
                ShoppingCart cart = _db.ShoppingCarts.FirstOrDefault(a=>a.LoginUserID==shoppingCart.LoginUserID && a.ProductID==shoppingCart.ProductID);
                if (cart==null)
                {
                    _db.ShoppingCarts.Add(shoppingCart);
                }
                else
                {
                    //sepette herhangi bir urun varsa yeni bir sepet açmak yerine o sepete ekleme yapmak için
                    cart.Count += shoppingCart.Count;
                }
                _db.SaveChanges();
                //Sipariş verenlerin syısnı getirmek için
                var count = _db.ShoppingCarts.Where(a => a.LoginUserID == shoppingCart.LoginUserID).ToList().Count();
                HttpContext.Session.SetInt32(AddRole.SassionShoppingCart, count);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                var product = _db.Products.FirstOrDefault(a => a.ID ==shoppingCart.ID);
                ShoppingCart cart = new ShoppingCart()
                {
                    Product = product,
                    ProductID = product.ID,
                };
            }
          
            return View(shoppingCart);
        }
        public IActionResult Search(string search)
        {
            if (!String.IsNullOrEmpty(search))
            {
                var aranan = _db.Products.Where(i => i.Title.Contains(search) || i.Description.Contains(search));
                return View(aranan);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult CategoryDetails(int? id)
        {
            var product = _db.Products.Where(i => i.CategoryID == id).ToList();
            ViewBag.KategoriId = id;
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
