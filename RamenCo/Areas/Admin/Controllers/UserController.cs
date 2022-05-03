using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RamenCo.Data;
using RamenCo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RamenCo.Areas.Admin.Controllers
{
    [Area("Admin")]
    //Giriş yapmadan bu sayfalara erişme engeli(ancak normal giriş yapsa dahi erişebilir (Roles=admin verilirse sadece admin erişebilir))
    [Authorize(Roles = AddRole.RoleAdmin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            //Kullanıcılar getirmek için
            var users = _context.LoginUsers.ToList();
            var role = _context.Roles.ToList();
            var userRole = _context.UserRoles.ToList();
            foreach (var item in users)
            {
                var roleID = userRole.FirstOrDefault(i => i.UserId == item.Id).RoleId;
                item.Role = role.FirstOrDefault(u => u.Id == roleID).Name;
            }
            return View(users);
        }
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.LoginUsers
                .FirstOrDefaultAsync(m => m.Id == id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.LoginUsers.FindAsync(id);
            _context.LoginUsers.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
