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
    public class HospitalsController : Controller
    {
        private readonly DataContext _context;

        public HospitalsController(DataContext context)
        {
            _context = context;
        }

        // GET: Hospitals
        public async Task<IActionResult> Index()
        {

            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            ViewBag.UserObject = this.GetUser();

            return View(await _context.Hospitals.Where(r => this.GetUser().IsAdmin || (this.GetUser().HospitalId != null && this.GetUser().HospitalId.Equals(r.Name))).ToListAsync());
        }

        // GET: Hospitals/Details/5
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

            var hospital = await _context.Hospitals
                .FirstOrDefaultAsync(m => m.Name == id);
            if (hospital == null)
            {
                return NotFound();
            }

            return View(hospital);
        }

        // GET: Hospitals/Create
        public IActionResult Create()
        {

            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");
            if (!this.GetUser().IsAdmin)
                return StatusCode(403);

            return View();
        }

        // POST: Hospitals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Address,Latitude,Longitude,Phone")] Hospital hospital)
        {

            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");
            if (!this.GetUser().IsAdmin)
                return StatusCode(403);

            if (ModelState.IsValid)
            {
                _context.Add(hospital);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hospital);
        }

        // GET: Hospitals/Edit/5
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

            var hospital = await _context.Hospitals.FindAsync(id);
            if (hospital == null)
            {
                return NotFound();
            }
            return View(hospital);
        }

        // POST: Hospitals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name,Address,Latitude,Longitude,Phone")] Hospital hospital)
        {

            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");
            if (!this.GetUser().IsAdmin)
                return StatusCode(403);

            if (id != hospital.Name)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(_context.Hospitals.Where(x => x.Name.Equals(hospital.Name)).FirstOrDefault()).State = EntityState.Detached;
                    _context.Update(hospital);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HospitalExists(hospital.Name))
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
            return View(hospital);
        }

        // GET: Hospitals/Delete/5
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

            var hospital = await _context.Hospitals
                .FirstOrDefaultAsync(m => m.Name == id);
            if (hospital == null)
            {
                return NotFound();
            }

            return View(hospital);
        }

        // POST: Hospitals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");
            if (!this.GetUser().IsAdmin)
                return StatusCode(403);

            var hospital = await _context.Hospitals.FindAsync(id);
            _context.Hospitals.Remove(hospital);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HospitalExists(string id)
        {

            if (this.GetUser() == null)
                RedirectToAction("Login", "Home");
            if (!this.GetUser().IsAdmin)
                RedirectToAction("Login", "Home");

            return _context.Hospitals.Any(e => e.Name == id);
        }
    }
}
