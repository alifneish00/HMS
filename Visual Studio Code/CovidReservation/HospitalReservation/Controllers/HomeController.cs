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

namespace HospitalReservation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private DataContext context_;

        public HomeController(ILogger<HomeController> logger, DataContext dataContext_)
        {
            _logger = logger;
            context_ = dataContext_;
        }

        public ActionResult Login()
        {
            this.SaveUser(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User objUser)
        {
            var obj = context_.Users.Where(a => a.Name.Equals(objUser.Name) && a.Password.Equals(objUser.Password) && (a.IsAdmin || a.Hospital!=null)).FirstOrDefault();
            if (obj != null)
            {
                this.SaveUser(obj);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login","Home");
            }
        }

        public IActionResult Index()
        {
            if (this.GetUser()==null)
                return RedirectToAction("Login", "Home");

            return View();
        }

        public IActionResult Privacy()
        {

            if (this.GetUser()==null)
                return RedirectToAction("Login", "Home");

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
