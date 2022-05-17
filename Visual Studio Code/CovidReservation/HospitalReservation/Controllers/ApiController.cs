using DataModel;
using HospitalReservation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HospitalReservation.Classes;
using Microsoft.EntityFrameworkCore;

namespace HospitalReservation.Controllers
{
    public class ApiController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private DataContext context_;

        public ApiController(ILogger<HomeController> logger, DataContext dataContext_)
        {
            _logger = logger;
            context_ = dataContext_;
        }

        public ActionResult<String> CreateUser([FromQuery] String username, [FromQuery] String password, [FromQuery] String address, [FromQuery] String mobile, [FromQuery] String dob)
        {
            try
            {
                User u = new User()
                {
                    Address = address,
                    DateOfBirth = DateTime.Now,
                    IsAdmin = false,
                    Mobile = mobile,
                    Name = username,
                    Password = password

                };

                context_.Add(u);
                context_.SaveChanges();
                
            }
            catch (Exception ex)
            {
                return "fail: " + ex.Message;
            }

            return "User Added";
        }

        public ActionResult<String> UpdateUser([FromQuery] String username, [FromQuery] String password, [FromQuery] String address, [FromQuery] String mobile, [FromQuery] String dob)
        {
            try
            {
                var u = context_.Users.Where(a => a.Name.Equals(username)).FirstOrDefault();
                if (u == null)
                    return "User not found";

                u.Address = address;
                u.DateOfBirth = DateTime.Now;
                u.IsAdmin = false;
                u.Mobile = mobile;
                u.Name = username;
                u.Password = password;

                context_.Update(u);
                context_.SaveChanges();
            }
            catch (Exception ex)
            {
                return "fail: " + ex.Message;
            }

            return "User Updated";
        }

        public ActionResult<String> Login([FromQuery] String username, [FromQuery] String password)
        {
            var u = context_.Users.Where(a => a.Name.Equals(username) && a.Password.Equals(password)).FirstOrDefault();
            if (u == null)
                return "";
            u.Reservations = null;
            u.Hospital = null;
            return JsonConvert.SerializeObject(u, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        }

        public ActionResult<String> Users()
        {
            var u = context_.Users.ToList();
            if (u == null)
                return "{}";

            return JsonConvert.SerializeObject(u, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        }

        public ActionResult<String> Hospitals()
        {
            var u = context_.Hospitals.ToList();
            if (u == null)
                return "{}";

            return JsonConvert.SerializeObject(u, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        }

        public ActionResult<String> Rooms()
        {
            var u = context_.Rooms.ToList();
            if (u == null)
                return "{}";

            return JsonConvert.SerializeObject(u, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        }

        public ActionResult<String> Beds()
        {
            var u = context_.Beds.ToList();
            if (u == null)
                return "{}";

            return JsonConvert.SerializeObject(u, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        }

        public ActionResult<String> Reservations()
        {
            var u = context_.Reservations.ToList();
            if (u == null)
                return "{}";

            return JsonConvert.SerializeObject(u, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        }

        public ActionResult<String> ReservationRequests()
        {
            var u = context_.ReservationRequests.ToList();
            if (u == null)
                return "{}";

            return JsonConvert.SerializeObject(u, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        }

        public ActionResult<String> GetUser([FromQuery] string id)
        {
            var u = context_.Users.Find(id);
            if (u == null)
                return "";

            return JsonConvert.SerializeObject(u, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        }

        public ActionResult<String> GetHospital([FromQuery] string id)
        {
            var u = context_.Hospitals.Find(id);
            if (u == null)
                return "";

            return JsonConvert.SerializeObject(u, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        }


        public ActionResult<String> GetRoom([FromQuery] string id)
        {
            var u = context_.Rooms.Find(id);
            if (u == null)
                return "";

            return JsonConvert.SerializeObject(u, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        }


        public ActionResult<String> GetBed([FromQuery] string id)
        {
            var u = context_.Beds.Find(id);
            if (u == null)
                return "";

            return JsonConvert.SerializeObject(u, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        }

        public ActionResult<String> GetReservation([FromQuery] long id)
        {
            var u = context_.Reservations.Find(id);
            if (u == null)
                return "";

            return JsonConvert.SerializeObject(u, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        }

        public ActionResult<String> GetReservationRequest([FromQuery] long id)
        {
            var u = context_.ReservationRequests.Find(id);
            if (u == null)
                return "";

            return JsonConvert.SerializeObject(u, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        }

        public ActionResult<String> DeleteReservation([FromQuery] long id)
        {
            try
            {
                var u = context_.Reservations.Find(id);
                if (u == null)
                    return "Reservation not found";

                context_.Reservations.Remove(u);
                context_.SaveChanges();
            }
            catch (Exception e)
            {
                return "fail: " + e.Message;
            }

            return "Reservation Deleted";
        }

        public ActionResult<String> DeleteReservationRequest([FromQuery] long id)
        {
            try
            {
                var u = context_.ReservationRequests.Find(id);
                if (u == null)
                    return "Reservation Request not found";

                context_.ReservationRequests.Remove(u);
                context_.SaveChanges();
            }
            catch (Exception e)
            {
                return "fail: " + e.Message;
            }

            return "Reservation Deleted";
        }

        public ActionResult<String> AddReservationRequest([FromQuery] string username, [FromQuery] string HospitalId)
        {
            try
            {
                var hospital = context_.Hospitals.Find(HospitalId);
                if (hospital == null)
                    return "Hospital Not Found";

                var User_ = context_.Users.Find(username);
                if (User_ == null)
                    return "User Not Found";


                var u = context_.ReservationRequests.Add(new ReservationRequest
                {
                    User = User_,
                    Hospital=hospital,
                });
                context_.SaveChanges();
            }
            catch (Exception e)
            {
                return "fail: " + e.Message;
            }

            return "Reservation Added";
        }

        public ActionResult<String> AddReservation([FromQuery] string username, [FromQuery] string bedId)
        {
            try
            {
                var Bed_ = context_.Beds.Find(bedId);
                if (Bed_ == null)
                    return "Bed Not Found";

                var User_ = context_.Users.Find(username);
                if (User_ == null)
                    return "User Not Found";

                if(context_.Reservations.Where(r => r.BedId.Equals(Bed_.Number)).Count() > 0)
                {
                    return "Bed already reserved";
                }

                var u = context_.Reservations.Add(new Reservation
                {
                    User = User_,
                    Bed = Bed_
                });
                context_.SaveChanges();
            }
            catch (Exception e)
            {
                return "fail: " + e.Message;
            }

            return "Reservation Added";
        }

    }
}
