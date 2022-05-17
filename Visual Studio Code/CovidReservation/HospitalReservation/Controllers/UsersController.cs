using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataModel;
using HospitalReservation.Controllers;
using HospitalReservation.Classes;

namespace HospitalReservation
{
    public class UsersController : Controller
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");
            if (!this.GetUser().IsAdmin)
                return StatusCode(403);

            ViewBag.UserObject = this.GetUser();

            var dataContext = _context.Users.Where(r => this.GetUser().IsAdmin || (this.GetUser().Name.Equals(r.Name))).Include(u => u.Hospital);
            return View(await dataContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");
            if (!this.GetUser().IsAdmin)
                return StatusCode(403);

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Hospital)
                .FirstOrDefaultAsync(m => m.Name == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");
            if (!this.GetUser().IsAdmin)
                return StatusCode(403);

            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Name", "Name");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Address,Mobile,DateOfBirth,Password,IsAdmin,HospitalId")] User user)
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");
            if (!this.GetUser().IsAdmin)
                return StatusCode(403);

            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Name", "Name", user.HospitalId);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");
            if (!this.GetUser().IsAdmin)
                return StatusCode(403);

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Name", "Name", user.HospitalId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name,Address,Mobile,DateOfBirth,Password,IsAdmin,HospitalId")] User user)
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");
            if (!this.GetUser().IsAdmin)
                return StatusCode(403);

            if (id != user.Name)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(_context.Users.Where(x=>x.Name.Equals(user.Name)).FirstOrDefault()).State = EntityState.Detached;   
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Name))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Name", "Name", user.HospitalId);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");
            if (!this.GetUser().IsAdmin)
                return StatusCode(403);

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Hospital)
                .FirstOrDefaultAsync(m => m.Name == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");
            if (!this.GetUser().IsAdmin)
                return StatusCode(403);

            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            if (this.GetUser() == null)
                RedirectToAction("Login", "Home");
            if (!this.GetUser().IsAdmin)
                RedirectToAction("Login", "Home");

            return _context.Users.Any(e => e.Name == id);
        }
    }
}
