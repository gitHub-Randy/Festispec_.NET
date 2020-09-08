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
    public class ScheduleController : Controller
    {
        private FestiSpecContext db = new FestiSpecContext();

        // GET: Schedule
        public ActionResult Index()
        {
            int userId = Convert.ToInt16(Session["UserID"]); // Get employeeID from sessiondata
            var inspections = db.Inspections.Include(i => i.Employee).Include(i => i.QuestionList.Festival)
                .Where(e => e.EmployeeId == userId)
                .Where(i => i.StartDate >= DateTime.Now)
                .OrderBy(i => i.StartDate);
            
            return View(inspections.ToList());
        }

        // GET: Schedule/Details/5
        public ActionResult Details(int? id)
        {
            int userId = Convert.ToInt16(Session["UserID"]); // Get employeeID from sessiondata
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inspection inspection = db.Inspections.Where(f => f.QuestionList.Festival.Id == id)
                .Where(e => e.EmployeeId == userId)
                .FirstOrDefault();
            if (inspection == null)
            {
                return HttpNotFound();
            }
            return View(inspection);
        }

        // GET: Schedule/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "Name");
            ViewBag.FestivalId = new SelectList(db.Festivals, "Id", "Name");
            return View();
        }

        // POST: Schedule/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeId,FestivalId,StartDate,EndDate,Present,ReasonsAbsent")] Inspection inspection)
        {
            if (ModelState.IsValid)
            {
                db.Inspections.Add(inspection);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "Name", inspection.EmployeeId);
            ViewBag.FestivalId = new SelectList(db.Festivals, "Id", "Name", inspection.QuestionList.Festival.Id);
            return View(inspection);
        }

        // GET: Schedule/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inspection inspection = db.Inspections.Find(id);
            if (inspection == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "Name", inspection.EmployeeId);
            ViewBag.FestivalId = new SelectList(db.Festivals, "Id", "Name", inspection.QuestionList.Festival.Id);
            return View(inspection);
        }

        // POST: Schedule/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeId,FestivalId,StartDate,EndDate,Present,ReasonsAbsent")] Inspection inspection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inspection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "Name", inspection.EmployeeId);
            ViewBag.FestivalId = new SelectList(db.Festivals, "Id", "Name", inspection.QuestionList.Festival.Id);
            return View(inspection);
        }

        // GET: Schedule/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inspection inspection = db.Inspections.Find(id);
            if (inspection == null)
            {
                return HttpNotFound();
            }
            return View(inspection);
        }

        // POST: Schedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inspection inspection = db.Inspections.Find(id);
            db.Inspections.Remove(inspection);
            db.SaveChanges();
            return RedirectToAction("Index");
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
