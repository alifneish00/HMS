using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataModel;
using HospitalReservation.Classes;

namespace HospitalReservation.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly DataContext _context;

        public ReservationsController(DataContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            ViewBag.UserObject = this.GetUser();

            var dataContext = _context.Reservations.Where(r => this.GetUser().IsAdmin || (this.GetUser().HospitalId != null && this.GetUser().HospitalId.Equals(r.Bed.Room.HospitalId))).Include(r => r.Bed).Include(r => r.User).Include(r => r.ReservationRequest);
            return View(await dataContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Bed)
                .Include(r => r.User)
                .Include(r => r.ReservationRequest)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            ViewData["BedId"] = new SelectList(_context.Beds.Where(b => this.GetUser().IsAdmin || (b.Room.HospitalId != null && b.Room.HospitalId.Equals(this.GetUser().HospitalId))), "Number", "Number");
            ViewData["UserId"] = new SelectList(_context.Users, "Name", "Name");
            ViewData["ReservationRequestId"] = new SelectList(_context.ReservationRequests.Where(r=>r.Status==ReservationRequestStatus.Pending), "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReservationDate,BedId,UserId,HasEnded,ReservationRequestId")] Reservation reservation)
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            if (ModelState.IsValid)
            {
                if (_context.Reservations.Where(r => r.Id != reservation.Id && r.BedId.Equals(reservation.BedId) && r.Bed.IsActive).Count() > 0)
                {
                    throw new Exception("Bed already reserved");
                }
                reservation.Bed.IsActive = !reservation.HasEnded;
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BedId"] = new SelectList(_context.Beds.Where(b => this.GetUser().IsAdmin || (b.Room.HospitalId != null && b.Room.HospitalId.Equals(this.GetUser().HospitalId))), "Number", "Number", reservation.BedId);
            ViewData["UserId"] = new SelectList(_context.Users, "Name", "Name", reservation.UserId);
            ViewData["ReservationRequestId"] = new SelectList(_context.ReservationRequests.Where(r => r.Status == ReservationRequestStatus.Pending), "Id", "Id");
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["BedId"] = new SelectList(_context.Beds.Where(b=>this.GetUser().IsAdmin || b.Room.HospitalId.Equals(this.GetUser().HospitalId)), "Number", "Number", reservation.BedId);
            ViewData["UserId"] = new SelectList(_context.Users, "Name", "Name", reservation.UserId);
            ViewData["ReservationRequestId"] = new SelectList(_context.ReservationRequests.Where(r => r.Status == ReservationRequestStatus.Pending), "Id", "Id");
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,ReservationDate,BedId,UserId,HasEnded,ReservationRequestId")] Reservation reservation)
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (_context.Reservations.Where(r => r.Id != reservation.Id && r.BedId.Equals(reservation.BedId) && r.Bed.IsActive).Count() > 0)
                    {
                        throw new Exception("Bed already reserved");
                    }
                    var v = _context.Reservations.Where(x => x.Id.Equals(reservation.Id)).Include(r => r.Bed).Include(r => r.ReservationRequest).Include(r => r.User).FirstOrDefault();
                    reservation.Bed = v.Bed;
                    reservation.BedId = v.BedId;
                    reservation.ReservationRequest = v.ReservationRequest;
                    reservation.ReservationRequestId = v.ReservationRequestId;
                    reservation.User = v.User;  
                    reservation.UserId = v.UserId;  

                    _context.Entry(_context.Reservations.Where(x => x.Id.Equals(reservation.Id)).FirstOrDefault()).State = EntityState.Detached;
                    _context.Update(reservation);
                    reservation.Bed.IsActive = !reservation.HasEnded;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
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
            ViewData["BedId"] = new SelectList(_context.Beds.Where(b => this.GetUser().IsAdmin || (b.Room.HospitalId != null && b.Room.HospitalId.Equals(this.GetUser().HospitalId))), "Number", "Number", reservation.BedId);
            ViewData["UserId"] = new SelectList(_context.Users, "Name", "Name", reservation.UserId);
            ViewData["ReservationRequestId"] = new SelectList(_context.ReservationRequests.Where(r => r.Status == ReservationRequestStatus.Pending), "Id", "Id");
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Bed)
                .Include(r => r.User)
                .Include(r => r.ReservationRequest)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            var reservation = await _context.Reservations.FindAsync(id);
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(long id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
