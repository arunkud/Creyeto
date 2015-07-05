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
    public class DoctorInsurancesController : Controller
    {
        private DoctorEntities db = new DoctorEntities();

        // GET: DoctorInsurances
        public ActionResult Index()
        {
            var doctorInsurances = db.DoctorInsurances.Include(d => d.Doctor).Include(d => d.Insurance);
            return View(doctorInsurances.ToList());
        }

        // GET: DoctorInsurances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorInsurance doctorInsurance = db.DoctorInsurances.Find(id);
            if (doctorInsurance == null)
            {
                return HttpNotFound();
            }
            return View(doctorInsurance);
        }

        // GET: DoctorInsurances/Create
        public ActionResult Create()
        {
            ViewBag.DoctorID = new SelectList(db.Doctors, "ID", "NameEN");
            ViewBag.InsuranceID = new SelectList(db.Insurances, "ID", "Name");
            return View();
        }

        // POST: DoctorInsurances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,InsuranceID,DoctorID,IsActive,Created,Updated")] DoctorInsurance doctorInsurance)
        {
            if (ModelState.IsValid)
            {
                db.DoctorInsurances.Add(doctorInsurance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DoctorID = new SelectList(db.Doctors, "ID", "NameEN", doctorInsurance.DoctorID);
            ViewBag.InsuranceID = new SelectList(db.Insurances, "ID", "Name", doctorInsurance.InsuranceID);
            return View(doctorInsurance);
        }

        // GET: DoctorInsurances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorInsurance doctorInsurance = db.DoctorInsurances.Find(id);
            if (doctorInsurance == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoctorID = new SelectList(db.Doctors, "ID", "NameEN", doctorInsurance.DoctorID);
            ViewBag.InsuranceID = new SelectList(db.Insurances, "ID", "Name", doctorInsurance.InsuranceID);
            return View(doctorInsurance);
        }

        // POST: DoctorInsurances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,InsuranceID,DoctorID,IsActive,Created,Updated")] DoctorInsurance doctorInsurance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doctorInsurance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DoctorID = new SelectList(db.Doctors, "ID", "NameEN", doctorInsurance.DoctorID);
            ViewBag.InsuranceID = new SelectList(db.Insurances, "ID", "Name", doctorInsurance.InsuranceID);
            return View(doctorInsurance);
        }

        // GET: DoctorInsurances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorInsurance doctorInsurance = db.DoctorInsurances.Find(id);
            if (doctorInsurance == null)
            {
                return HttpNotFound();
            }
            return View(doctorInsurance);
        }

        // POST: DoctorInsurances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DoctorInsurance doctorInsurance = db.DoctorInsurances.Find(id);
            db.DoctorInsurances.Remove(doctorInsurance);
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
