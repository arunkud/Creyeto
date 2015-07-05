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
    public class CityLocationsController : Controller
    {
        private DoctorEntities db = new DoctorEntities();

        // GET: CityLocations
        public ActionResult Index()
        {
            var cityLocations = db.CityLocations.Include(c => c.City);
            return View(cityLocations.ToList());
        }

        // GET: CityLocations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CityLocation cityLocation = db.CityLocations.Find(id);
            if (cityLocation == null)
            {
                return HttpNotFound();
            }
            return View(cityLocation);
        }

        // GET: CityLocations/Create
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(db.Cities, "ID", "NameEN");
            return View();
        }

        // POST: CityLocations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NameEN,CityID,Latitude,Longitude,IsActive")] CityLocation cityLocation)
        {
            if (ModelState.IsValid)
            {
                db.CityLocations.Add(cityLocation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityID = new SelectList(db.Cities, "ID", "NameEN", cityLocation.CityID);
            return View(cityLocation);
        }

        // GET: CityLocations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CityLocation cityLocation = db.CityLocations.Find(id);
            if (cityLocation == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(db.Cities, "ID", "NameEN", cityLocation.CityID);
            return View(cityLocation);
        }

        // POST: CityLocations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NameEN,CityID,Latitude,Longitude,IsActive")] CityLocation cityLocation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cityLocation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(db.Cities, "ID", "NameEN", cityLocation.CityID);
            return View(cityLocation);
        }

        // GET: CityLocations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CityLocation cityLocation = db.CityLocations.Find(id);
            if (cityLocation == null)
            {
                return HttpNotFound();
            }
            return View(cityLocation);
        }

        // POST: CityLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CityLocation cityLocation = db.CityLocations.Find(id);
            db.CityLocations.Remove(cityLocation);
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
