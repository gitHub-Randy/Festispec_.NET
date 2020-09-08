using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FestiSpec.Entities.Dal;
using FestiSpec.Entity;

namespace FestiSpec.Website.Controllers
{
    public class AvailabilityController : Controller
    {
        private FestiSpecContext db = new FestiSpecContext();

        // GET: Availability
        public ActionResult Index()
        {
            var employeeAbsences = db.EmployeeAbsenceDates.Include(e => e.Employee);
            return View(employeeAbsences.ToList());
        }

        // GET: Availability/Details/5
        public ActionResult Details()
        {           
            if (Session["UserID"] == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            int id = Convert.ToInt16(Session["UserID"]); // Get employeeID from sessiondata
            EmployeeAbsence employeeAbsence = db.EmployeeAbsenceDates.Find(id);

            return View(employeeAbsence);
        }

        public ActionResult AbsenceDates()
        {
            if (Session["UserID"] == null)
                return View();

            int id = Convert.ToInt16(Session["UserID"]); // Get employeeID from sessiondata
            Employee employee = db.Employees.Find(id);

            return View(employee);
        }

        // GET: Availability/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "Name");
            return View();
        }

        // POST: Availability/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeId,Date")] EmployeeAbsence employeeAbsence, DateTime? fromDate, DateTime? toDate)
        {
            employeeAbsence.Date = fromDate ?? DateTime.MinValue; /* Set employeeAbsense date to fromDate to fix datetime picker. 
                                                                     If fromDate is not filled in set value to minValue which will be picked up in further check*/
            employeeAbsence.EmployeeId = Convert.ToInt16(Session["UserID"]); // Get employeeID from sessiondata

            if (employeeAbsence.Date == DateTime.MinValue)
            {
                ViewBag.ErrorMessage = "Vul een datum in!";
                return View("../Home/Availability");
            }
            else if (employeeAbsence.Date <= DateTime.Now) // Check on valid date
            {
                ViewBag.ErrorMessage = "Kan de datum van vandaag of datum in het verleden niet selecteren";
                return View("../Home/Availability");
            }

            var inspectionAtAbsenceDate = db.Inspections.Where(i => i.StartDate == employeeAbsence.Date && i.EmployeeId == employeeAbsence.EmployeeId).FirstOrDefault();

            if (inspectionAtAbsenceDate != null)
            {
                ViewBag.ErrorMessage = "Er is al een festival geplanned op de dag waarop u absent wilt zijn. Neem contact op met FestiSpec om dit te bespreken.";
                return View("../Home/Availability");
            }

            if (toDate == null) // If only one date is selected
            {
                if (ModelState.IsValid)
                {
                    db.EmployeeAbsenceDates.Add(employeeAbsence);
                    try
                    {
                        db.SaveChanges();
                        ViewBag.SuccesMessage = "Datum is succesvol toegevoegd!";
                    }
                    catch
                    {
                        ViewBag.ErrorMessage = "Datum is niet goed toegevoegd";
                    };

                    return View("../Home/Availability");
                }
            }
            else // When multiple dates need to be added to the database since to date is filled in
            {
                if (ModelState.IsValid)
                {
                    for (DateTime tempdate = employeeAbsence.Date; tempdate <= toDate; tempdate = tempdate.AddDays(1))
                    {
                        EmployeeAbsence ea = new EmployeeAbsence
                        {
                            EmployeeId = employeeAbsence.EmployeeId,
                            Date = tempdate,
                            Employee = employeeAbsence.Employee
                        };

                        db.EmployeeAbsenceDates.Add(ea);

                        try
                        {
                            db.SaveChanges();
                            ViewBag.SuccesMessage = "Een of meerdere datums zijn succesvol toegevoegd";
                        }
                        catch
                        {
                            ViewBag.ErrorMessage = "Er is iets fout gegaan bij het toevoegen van datum: " + ea.Date + " mogelijk datums na deze datum zijn hierdoor ook niet toegevoegd";
                            return View("../Home/Availability");
                        };
                    }

                    return View("../Home/Availability");
                }
            }



            return View(employeeAbsence);
        }

        // GET: Availability/Delete/5
        public ActionResult Delete(DateTime date)
        {
            if (date == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EmployeeAbsence employeeAbsence = db.EmployeeAbsenceDates.Where(ead => ead.Date == date).FirstOrDefault();
            if (employeeAbsence != null)
            {
                db.EmployeeAbsenceDates.Remove(employeeAbsence);
                db.SaveChanges();
            }

            return RedirectToAction("AbsenceDates");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
