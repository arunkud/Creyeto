﻿using System;
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
    public class DoctorsController : Controller
    {
        private DoctorEntities db = new DoctorEntities();

        // GET: Doctors
        public ActionResult Index()
        {
            var doctors = db.Doctors.Include(d => d.City).Include(d => d.CityLocation).Include(d => d.Specialization);
            return View(doctors.ToList());
        }

        // GET: Doctors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // GET: Doctors/Create
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(db.Cities, "ID", "NameEN");
            ViewBag.CityLocationID = new SelectList(db.CityLocations, "ID", "NameEN");
            ViewBag.SpecialityID = new SelectList(db.Specializations, "ID", "NameEN");
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NameEN,ContactNumber,Latitude,Longitude,CityID,CityLocationID,SecondNumber,SpecialityID,IsActive,Qualification,Fee")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.Doctors.Add(doctor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityID = new SelectList(db.Cities, "ID", "NameEN", doctor.CityID);
            ViewBag.CityLocationID = new SelectList(db.CityLocations, "ID", "NameEN", doctor.CityLocationID);
            ViewBag.SpecialityID = new SelectList(db.Specializations, "ID", "NameEN", doctor.SpecialityID);
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(db.Cities, "ID", "NameEN", doctor.CityID);
            ViewBag.CityLocationID = new SelectList(db.CityLocations, "ID", "NameEN", doctor.CityLocationID);
            ViewBag.SpecialityID = new SelectList(db.Specializations, "ID", "NameEN", doctor.SpecialityID);
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NameEN,ContactNumber,Latitude,Longitude,CityID,CityLocationID,SecondNumber,SpecialityID,IsActive,Qualification,Fee")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doctor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(db.Cities, "ID", "NameEN", doctor.CityID);
            ViewBag.CityLocationID = new SelectList(db.CityLocations, "ID", "NameEN", doctor.CityLocationID);
            ViewBag.SpecialityID = new SelectList(db.Specializations, "ID", "NameEN", doctor.SpecialityID);
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            db.Doctors.Remove(doctor);
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
