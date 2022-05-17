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
    public class ReservationRequestsController : Controller
    {
        private readonly DataContext _context;

        public ReservationRequestsController(DataContext context)
        {
            _context = context;
        }

        // GET: ReservationRequests
        public async Task<IActionResult> Index()
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            ViewBag.UserObject = this.GetUser();

            var dataContext = _context.ReservationRequests.Where(r=>this.GetUser().IsAdmin || (this.GetUser().HospitalId!=null && this.GetUser().HospitalId.Equals(r.HospitalId))).Include(r => r.Hospital).Include(r => r.Reservation).Include(r => r.User);
            return View(await dataContext.ToListAsync());
        }

        // GET: ReservationRequests/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            if (id == null)
            {
                return NotFound();
            }

            var reservationRequest = await _context.ReservationRequests
                .Include(r => r.Hospital)
                .Include(r => r.Reservation)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservationRequest == null)
            {
                return NotFound();
            }

            return View(reservationRequest);
        }

        // GET: ReservationRequests/Create
        public IActionResult Create()
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Name", "Name");
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Name", "Name");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Accept(long? id)
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            var rr = _context.ReservationRequests.Where(r => r.Id == id).FirstOrDefault();
            if (rr != null)
            {
                var b = _context.Beds.Where(b => !b.IsActive && b.Room.HospitalId.Equals(rr.HospitalId)).FirstOrDefault();
                Reservation r = new Reservation()
                {
                    ReservationDate = rr.RequestDate,
                    Bed = b,
                    BedId = b.Number,
                    HasEnded = false,
                    ReservationRequest = rr,
                    ReservationRequestId = rr.Id,
                    User = rr.User,
                    UserId = rr.UserId,
                };

                _context.Reservations.Add(r);
                rr.Reservation = r;
                rr.ReservationId = r.Id;
                rr.Status = ReservationRequestStatus.Accepted;
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Reject(long? id)
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            var rr = _context.ReservationRequests.Where(r => r.Id == id).FirstOrDefault();
            if (rr != null)
            {
                rr.Status = ReservationRequestStatus.Rejected;
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: ReservationRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Status,RequestDate,UserId,HospitalId,ReservationId")] ReservationRequest reservationRequest)
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            if (ModelState.IsValid)
            {
                _context.Add(reservationRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Name", "Name", reservationRequest.HospitalId);
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "Id", "Id", reservationRequest.ReservationId);
            ViewData["UserId"] = new SelectList(_context.Users, "Name", "Name", reservationRequest.UserId);
            return View(reservationRequest);
        }

        // GET: ReservationRequests/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            if (id == null)
            {
                return NotFound();
            }

            var reservationRequest = await _context.ReservationRequests.FindAsync(id);
            if (reservationRequest == null)
            {
                return NotFound();
            }
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Name", "Name", reservationRequest.HospitalId);
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "Id", "Id", reservationRequest.ReservationId);
            ViewData["UserId"] = new SelectList(_context.Users, "Name", "Name", reservationRequest.UserId);
            return View(reservationRequest);
        }



        // POST: ReservationRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Status,RequestDate,UserId,HospitalId,ReservationId")] ReservationRequest reservationRequest)
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            if (id != reservationRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(_context.Reservations.Where(x => x.Id.Equals(reservationRequest.Id)).FirstOrDefault()).State = EntityState.Detached;
                    _context.Update(reservationRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationRequestExists(reservationRequest.Id))
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
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Name", "Name", reservationRequest.HospitalId);
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "Id", "Id", reservationRequest.ReservationId);
            ViewData["UserId"] = new SelectList(_context.Users, "Name", "Name", reservationRequest.UserId);
            return View(reservationRequest);
        }

        // GET: ReservationRequests/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            if (id == null)
            {
                return NotFound();
            }

            var reservationRequest = await _context.ReservationRequests
                .Include(r => r.Hospital)
                .Include(r => r.Reservation)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservationRequest == null)
            {
                return NotFound();
            }

            return View(reservationRequest);
        }

        // POST: ReservationRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (this.GetUser() == null)
                return RedirectToAction("Login", "Home");

            var reservationRequest = await _context.ReservationRequests.FindAsync(id);
            _context.ReservationRequests.Remove(reservationRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationRequestExists(long id)
        {
            return _context.ReservationRequests.Any(e => e.Id == id);
        }
    }
}
