using FestiSpec.Entities.Dal;
using FestiSpec.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FestiSpec.Website.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(EmployeeAccount objUser)
        {
            //if (ModelState.IsValid)
            //{
            var obj = DBContext.Instance.EmployeeAccounts.Where(a => a.Username.Equals(objUser.Username) && a.Password.Equals(objUser.Password)).FirstOrDefault();
            if (obj != null) // Check if login is valid
            {
                Session["UserID"] = obj.EmployeeId.ToString();
                Session["UserName"] = obj.Username.ToString();
                return RedirectToAction("Index");
            }
            else
                ViewBag.Message = "Login is niet geldig, probeer het opnieuw.";
            //}
            return View(objUser);
        }

        public ActionResult Index()
        {
            if (Session["UserID"] != null)
                return View();
            else
                return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Login");
        }

        public ActionResult Schedule()
        {
            if (Session["UserID"] != null)
                return View();
            else
                return RedirectToAction("Login");
        }

        public ActionResult Availability()
        {
            if (Session["UserID"] != null)
                return View();
            else
                return RedirectToAction("Login");
        }

        public ActionResult Contact()
        {
            if (Session["UserID"] != null)
                return View();
            else
                return RedirectToAction("Login");
        }
    }
}