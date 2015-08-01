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
    [Authorize(Roles= "Admin")]
    public class HospitalAdminController : Controller
    {
        private Entities db = new Entities();

        // GET: HospitalAdmin
        public ActionResult Index()
        {
            var hospitalAdmins = db.HospitalAdmins.Include(h => h.AspNetUser).Include(h => h.Hospital);
            return View(hospitalAdmins.ToList());
        }

        // GET: HospitalAdmin/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HospitalAdmin hospitalAdmin = db.HospitalAdmins.Find(id);
            if (hospitalAdmin == null)
            {
                return HttpNotFound();
            }
            return View(hospitalAdmin);
        }

        // GET: HospitalAdmin/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.HospitalID = new SelectList(db.Hospitals, "ID", "NameEN");
            return View();
        }

        // POST: HospitalAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,HospitalID,IsActive,Created,Updated")] HospitalAdmin hospitalAdmin)
        {
            if (ModelState.IsValid)
            {
                db.HospitalAdmins.Add(hospitalAdmin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", hospitalAdmin.UserID);
            ViewBag.HospitalID = new SelectList(db.Hospitals, "ID", "NameEN", hospitalAdmin.HospitalID);
            return View(hospitalAdmin);
        }

        // GET: HospitalAdmin/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HospitalAdmin hospitalAdmin = db.HospitalAdmins.Find(id);
            if (hospitalAdmin == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", hospitalAdmin.UserID);
            ViewBag.HospitalID = new SelectList(db.Hospitals, "ID", "NameEN", hospitalAdmin.HospitalID);
            return View(hospitalAdmin);
        }

        // POST: HospitalAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,HospitalID,IsActive,Created,Updated")] HospitalAdmin hospitalAdmin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hospitalAdmin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", hospitalAdmin.UserID);
            ViewBag.HospitalID = new SelectList(db.Hospitals, "ID", "NameEN", hospitalAdmin.HospitalID);
            return View(hospitalAdmin);
        }

        // GET: HospitalAdmin/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HospitalAdmin hospitalAdmin = db.HospitalAdmins.Find(id);
            if (hospitalAdmin == null)
            {
                return HttpNotFound();
            }
            return View(hospitalAdmin);
        }

        // POST: HospitalAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HospitalAdmin hospitalAdmin = db.HospitalAdmins.Find(id);
            db.HospitalAdmins.Remove(hospitalAdmin);
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
