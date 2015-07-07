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
	[Authorize(Roles = "Admin")]
    public class DoctorReviewsController : Controller
    {
        private DoctorEntities db = new DoctorEntities();

        // GET: DoctorReviews
        public ActionResult Index()
        {
            var doctorReviews = db.DoctorReviews.Include(d => d.Doctor);
            return View(doctorReviews.ToList());
        }

        // GET: DoctorReviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorReview doctorReview = db.DoctorReviews.Find(id);
            if (doctorReview == null)
            {
                return HttpNotFound();
            }
            return View(doctorReview);
        }

        // GET: DoctorReviews/Create
        public ActionResult Create()
        {
            ViewBag.DoctorID = new SelectList(db.Doctors, "ID", "NameEN");
            return View();
        }

        // POST: DoctorReviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserID,DoctorID,Review,Rating,IsActive")] DoctorReview doctorReview)
        {
            if (ModelState.IsValid)
            {
                db.DoctorReviews.Add(doctorReview);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DoctorID = new SelectList(db.Doctors, "ID", "NameEN", doctorReview.DoctorID);
            return View(doctorReview);
        }

        // GET: DoctorReviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorReview doctorReview = db.DoctorReviews.Find(id);
            if (doctorReview == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoctorID = new SelectList(db.Doctors, "ID", "NameEN", doctorReview.DoctorID);
            return View(doctorReview);
        }

        // POST: DoctorReviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserID,DoctorID,Review,Rating,IsActive")] DoctorReview doctorReview)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doctorReview).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DoctorID = new SelectList(db.Doctors, "ID", "NameEN", doctorReview.DoctorID);
            return View(doctorReview);
        }

        // GET: DoctorReviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorReview doctorReview = db.DoctorReviews.Find(id);
            if (doctorReview == null)
            {
                return HttpNotFound();
            }
            return View(doctorReview);
        }

        // POST: DoctorReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DoctorReview doctorReview = db.DoctorReviews.Find(id);
            db.DoctorReviews.Remove(doctorReview);
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
