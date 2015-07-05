using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ecom.presentation.website.Models;

namespace ecom.presentation.website.Controllers
{
	[Authorize]
    public class DoctorSchedulesController : Controller
    {
        private DoctorEntities db = new DoctorEntities();

        // GET: DoctorSchedules
        public ActionResult Index()
        {
            var doctorSchedules = db.DoctorSchedules.Include(d => d.Doctor);
            return View(doctorSchedules.ToList());
        }

        // GET: DoctorSchedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorSchedule doctorSchedule = db.DoctorSchedules.Find(id);
            if (doctorSchedule == null)
            {
                return HttpNotFound();
            }
            return View(doctorSchedule);
        }

        // GET: DoctorSchedules/Create
        public ActionResult Create()
        {
            ViewBag.DoctorID = new SelectList(db.Doctors, "ID", "NameEN");
            return View();
        }

        // POST: DoctorSchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DoctorID,Day,Duration,MorningStartTime,MorningSlots,MorningEndTime,AfternoonStartTime,AfternoonSlots,AfternoonEndTime,EveningStartTime,EveningSlots,EveningEndTime,Created,Updated,IsActive")] DoctorSchedule doctorSchedule)
        {
            if (ModelState.IsValid)
            {
                db.DoctorSchedules.Add(doctorSchedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DoctorID = new SelectList(db.Doctors, "ID", "NameEN", doctorSchedule.DoctorID);
            return View(doctorSchedule);
        }

        // GET: DoctorSchedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorSchedule doctorSchedule = db.DoctorSchedules.Find(id);
            if (doctorSchedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoctorID = new SelectList(db.Doctors, "ID", "NameEN", doctorSchedule.DoctorID);
            return View(doctorSchedule);
        }

        // POST: DoctorSchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DoctorID,Day,Duration,MorningStartTime,MorningSlots,MorningEndTime,AfternoonStartTime,AfternoonSlots,AfternoonEndTime,EveningStartTime,EveningSlots,EveningEndTime,Created,Updated,IsActive")] DoctorSchedule doctorSchedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doctorSchedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DoctorID = new SelectList(db.Doctors, "ID", "NameEN", doctorSchedule.DoctorID);
            return View(doctorSchedule);
        }

        // GET: DoctorSchedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorSchedule doctorSchedule = db.DoctorSchedules.Find(id);
            if (doctorSchedule == null)
            {
                return HttpNotFound();
            }
            return View(doctorSchedule);
        }

        // POST: DoctorSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DoctorSchedule doctorSchedule = db.DoctorSchedules.Find(id);
            db.DoctorSchedules.Remove(doctorSchedule);
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
