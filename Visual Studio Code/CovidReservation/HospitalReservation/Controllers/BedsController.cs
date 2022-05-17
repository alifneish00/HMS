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
    public class BedsController : Controller
    {
        private readonly DataContext _context;

        public BedsController(DataContext context)
        {
            _context = context;
        }

        // GET: Beds
        public async Task<IActionResult> Index()
        {

            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            ViewBag.UserObject = this.GetUser();

            var dataContext = _context.Beds.Where(r => this.GetUser().IsAdmin || (this.GetUser().HospitalId != null && this.GetUser().HospitalId.Equals(r.Room.HospitalId))).Include(b => b.Room);
            return View(await dataContext.ToListAsync());
        }

        // GET: Beds/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            if (id == null)
            {
                return NotFound();
            }

            var bed = await _context.Beds
                .Include(b => b.Room)
                .FirstOrDefaultAsync(m => m.Number == id);
            if (bed == null)
            {
                return NotFound();
            }

            return View(bed);
        }

        // GET: Beds/Create
        public IActionResult Create()
        {

            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");
            
            ViewData["RoomId"] = new SelectList(_context.Rooms.Where(b => this.GetUser().IsAdmin || (b.HospitalId != null && b.HospitalId.Equals(this.GetUser().HospitalId))), "Number", "Discription");
            return View();
        }

        // POST: Beds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Number,Status,IsActive,RoomId")] Bed bed)
        {

            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            if (ModelState.IsValid)
            {
                _context.Add(bed);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms.Where(b => this.GetUser().IsAdmin || (b.HospitalId != null && b.HospitalId.Equals(this.GetUser().HospitalId))), "Number", "Discription", bed.RoomId);
            return View(bed);
        }

        // GET: Beds/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {

            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            if (id == null)
            {
                return NotFound();
            }

            var bed = await _context.Beds.FindAsync(id);
            if (bed == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms.Where(b => this.GetUser().IsAdmin || (b.HospitalId != null && b.HospitalId.Equals(this.GetUser().HospitalId))), "Number", "Discription", bed.RoomId);
            return View(bed);
        }

        // POST: Beds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Number,Status,IsActive,RoomId")] Bed bed)
        {

            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            if (id != bed.Number)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(_context.Beds.Where(x => x.Number.Equals(bed.Number)).FirstOrDefault()).State = EntityState.Detached;
                    _context.Update(bed);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BedExists(bed.Number))
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
            ViewData["RoomId"] = new SelectList(_context.Rooms.Where(b => this.GetUser().IsAdmin || (b.HospitalId != null && b.HospitalId.Equals(this.GetUser().HospitalId))), "Number", "Discription", bed.RoomId);
            return View(bed);
        }

        // GET: Beds/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bed = await _context.Beds
                .Include(b => b.Room)
                .FirstOrDefaultAsync(m => m.Number == id);
            if (bed == null)
            {
                return NotFound();
            }

            return View(bed);
        }

        // POST: Beds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {

            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            var bed = await _context.Beds.FindAsync(id);
            _context.Beds.Remove(bed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BedExists(long id)
        {

            if (this.GetUser() == null)
                RedirectToAction("Login", "Home");

            return _context.Beds.Any(e => e.Number == id);
        }
    }
}
