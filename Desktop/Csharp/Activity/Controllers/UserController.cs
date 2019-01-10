using System;
using System.Collections.Generic;
using System.Linq;
using Activity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Activity.Controllers
{
    public class UserController : Controller
    {
        private readonly MyContext _context;
        public UserController(MyContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(User user)
        {
            if(ModelState.IsValid)
            {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                _context.Users.Add(user);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("User", user.FirstName);
                return RedirectToAction("Home");
            }
            else
            {
                return View("Index");
            }
        }
        
        [HttpPost]
        [Route("login")]
        public IActionResult Login(string Email, string Password)
        {
            var user = _context.Users.SingleOrDefault(p => p.Email == Email);
            if (user != null && Password != null)
            {
                var Hasher = new PasswordHasher<User>();
                var result = Hasher.VerifyHashedPassword(user, user.Password, Password);
                if(result != 0)
                {
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("User", user.FirstName);
                    return RedirectToAction("Home");
                }
                else {
                    ViewBag.errors = "Please provide a valid email and password.";
                    return View("Index");
                }
            }
            else {
                ViewBag.errors = "Email not registered";
                return View("Index");
            }
        }

        [HttpGet]
        [Route("Home")]
        public IActionResult Home()
        {
            ViewBag.SessionId = HttpContext.Session.GetInt32("UserId");
            List<Actv> AllActivities = _context.Activities.Include(p => p.Participants).Include(u => u.User).OrderByDescending(x => x.ActvId).ToList();
            ViewBag.AllActivities = AllActivities;
            ViewBag.User = HttpContext.Session.GetString("User");
            return View();
        }

        [HttpGet]
        [Route("new")]
        public IActionResult New()
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        [Route("new")]
        public IActionResult Add(Actv newActivity)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            else if(newActivity.Date < DateTime.Now)
            {
                ViewBag.dateproblem = "Please choose a date and time in the future.";
                return View("new");
            }
            else if (ModelState.IsValid)
            {
                Actv NewActivity = new Actv {
                    Title = newActivity.Title,
                    Time = newActivity.Time,
                    Date = newActivity.Date,
                    Duration = newActivity.Duration,
                    DurationType = newActivity.DurationType,
                    Description = newActivity.Description,
                    User = _context.Users.SingleOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"))
                };
                _context.Activities.Add(NewActivity);
                _context.SaveChanges();
                return RedirectToAction("home");
            }
            else
            {
                return View("New");
            }
        }

        [HttpGet]
        [Route("activity/{id}")]
        public IActionResult Display(int id)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            Actv ThisActivity = _context.Activities.Where(a => a.ActvId == id).Include(u => u.User).Include(p => p.Participants).SingleOrDefault();
            ViewBag.Participant = _context.Participants.Where(a => a.ActvId == id).Include(u => u.User).Distinct().ToList();
            ViewBag.ThisActivity = ThisActivity;
            return View();
        }

        [HttpGet]
        [Route("join/{id}")]
        public IActionResult Join(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null ){
                return RedirectToAction("Index");
            } 
            User sheep = _context.Users.SingleOrDefault(s => s.UserId == HttpContext.Session.GetInt32("UserId"));
            Actv ThisActivity = _context.Activities.SingleOrDefault(a => a.ActvId == id);
            Participant NewParticipant = new Participant
            {
                ActvId = id,
                Actv = ThisActivity,
                UserId = sheep.UserId,
                User = sheep
            };
            _context.Participants.Add(NewParticipant);
            _context.SaveChanges();
            return RedirectToAction("home");
        }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            Actv DeleteActivity = _context.Activities.SingleOrDefault(x => x.ActvId == id);
            _context.Activities.Remove(DeleteActivity);
            _context.SaveChanges();
            return RedirectToAction("home");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
