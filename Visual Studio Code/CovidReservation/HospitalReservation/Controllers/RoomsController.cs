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
    public class RoomsController : Controller
    {
        private readonly DataContext _context;

        public RoomsController(DataContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {

            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            ViewBag.UserObject = this.GetUser();

            var dataContext = _context.Rooms.Where(r => this.GetUser().IsAdmin || (this.GetUser().HospitalId != null && this.GetUser().HospitalId.Equals(r.HospitalId))).Include(r => r.Hospital);
            return View(await dataContext.ToListAsync());
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(string id)
        {

            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.Hospital)
                .FirstOrDefaultAsync(m => m.Number == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {

            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            ViewData["HospitalId"] = new SelectList(_context.Hospitals.Where(h => this.GetUser().IsAdmin || h.Name.Equals(this.GetUser().HospitalId)), "Name", "Name");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Number,Floor,IsActive,HospitalId")] Room room)
        {

            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HospitalId"] = new SelectList(_context.Hospitals.Where(h => this.GetUser().IsAdmin || h.Name.Equals(this.GetUser().HospitalId)), "Name", "Name", room.HospitalId);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(string id)
        {

            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            ViewData["HospitalId"] = new SelectList(_context.Hospitals.Where(h => this.GetUser().IsAdmin || h.Name.Equals(this.GetUser().HospitalId)), "Name", "Name", room.HospitalId);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Number,Floor,IsActive,HospitalId")] Room room)
        {

            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            if (id != room.Number)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(_context.Rooms.Where(x => x.Number.Equals(room.Number)).FirstOrDefault()).State = EntityState.Detached; 
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Number))
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
            ViewData["HospitalId"] = new SelectList(_context.Hospitals.Where(h => this.GetUser().IsAdmin || h.Name.Equals(this.GetUser().HospitalId)), "Name", "Name", room.HospitalId);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(string id)
        {

            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.Hospital)
                .FirstOrDefaultAsync(m => m.Number == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            var room = await _context.Rooms.FindAsync(id);
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(string id)
        {

            if (this.GetUser() == null)
                RedirectToAction("Login", "Home");
            if (!this.GetUser().IsAdmin && this.GetUser().Hospital==null)
                RedirectToAction("Login", "Home");

            return _context.Rooms.Any(e => e.Number == id);
        }
    }
}
