using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using ecom.presentation.website.Models;

namespace ecom.presentation.website.Controllers
{
	public class DoctorsController : Controller
    {
		private const int AvatarStoredWidth = 100;  // ToDo - Change the size of the stored avatar image
		private const int AvatarStoredHeight = 100; // ToDo - Change the size of the stored avatar image
		private const int AvatarScreenWidth = 400;  // ToDo - Change the value of the width of the image on the screen

		private const string TempFolder = "/Temp";
		private const string MapTempFolder = "~" + TempFolder;
		private const string AvatarPath = "/Images";

		private readonly string[] _imageFileExtensions = { ".jpg", ".png", ".gif", ".jpeg" };
		private Entities db = new Entities();

        // GET: Doctors
        public ActionResult Index()
        {
			var doctors = db.Doctors.Include(d => d.Hospital).Include(d => d.Specialization);
			return View(doctors.ToList());
        }

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
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
		[Authorize(Roles = "Admin")]
		public ActionResult Create()
        {
			ViewBag.HospitalID = new SelectList(db.Hospitals, "ID", "NameEN");
			ViewBag.SpecialityID = new SelectList(db.Specializations, "ID", "NameEN");
			return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ID,NameEN,ContactNumber,Latitude,Longitude,CityID,CityLocationID,SecondNumber,SpecialityID,IsActive,Qualification,Fee")] Doctor doctor)
        {
			if (ModelState.IsValid)
			{
				db.Doctors.Add(doctor);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			ViewBag.HospitalID = new SelectList(db.Hospitals, "ID", "NameEN", doctor.HospitalID);
			ViewBag.SpecialityID = new SelectList(db.Specializations, "ID", "NameEN", doctor.SpecialityID);
			return View(doctor);
        }

        // GET: Doctors/Edit/5
		[Authorize(Roles = "Admin")]
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
			ViewBag.HospitalID = new SelectList(db.Hospitals, "ID", "NameEN", doctor.HospitalID);
			ViewBag.SpecialityID = new SelectList(db.Specializations, "ID", "NameEN", doctor.SpecialityID);
			return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ID,NameEN,ContactNumber,Latitude,Longitude,CityID,CityLocationID,SecondNumber,SpecialityID,IsActive,Qualification,Fee")] Doctor doctor)
        {
			if (ModelState.IsValid)
			{
				db.Entry(doctor).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.HospitalID = new SelectList(db.Hospitals, "ID", "NameEN", doctor.HospitalID);
			ViewBag.SpecialityID = new SelectList(db.Specializations, "ID", "NameEN", doctor.SpecialityID);
			return View(doctor);
        }

        // GET: Doctors/Delete/5
		[Authorize(Roles = "Admin")]
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
		[Authorize(Roles = "Admin")]
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

		[HttpGet]
		public ActionResult Upload()
		{
			return View();
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public ActionResult _Upload()
		{
			return PartialView();
		}

		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public ActionResult _Upload(IEnumerable<HttpPostedFileBase> files)
		{
			if (files == null || !files.Any()) return Json(new { success = false, errorMessage = "No file uploaded." });
			var file = files.FirstOrDefault();  // get ONE only
			if (file == null || !IsImage(file)) return Json(new { success = false, errorMessage = "File is of wrong format." });
			if (file.ContentLength <= 0) return Json(new { success = false, errorMessage = "File cannot be zero length." });
			var webPath = GetTempSavedFilePath(file);
			return Json(new { success = true, fileName = webPath.Replace("/", "\\") }); // success
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public ActionResult Save(string t, string l, string h, string w, string fileName)
		{
			try
			{
				// Calculate dimensions
				var top = Convert.ToInt32(t.Replace("-", "").Replace("px", ""));
				var left = Convert.ToInt32(l.Replace("-", "").Replace("px", ""));
				var height = Convert.ToInt32(h.Replace("-", "").Replace("px", ""));
				var width = Convert.ToInt32(w.Replace("-", "").Replace("px", ""));

				// Get file from temporary folder
				var fn = Path.Combine(Server.MapPath(MapTempFolder), Path.GetFileName(fileName));
				// ...get image and resize it, ...
				var img = new WebImage(fn);
				img.Resize(width, height);
				// ... crop the part the user selected, ...
				img.Crop(top, left, img.Height - top - AvatarStoredHeight, img.Width - left - AvatarStoredWidth);
				// ... delete the temporary file,...
				System.IO.File.Delete(fn);
				// ... and save the new one.
				var newFileName = Path.Combine(AvatarPath, Path.GetFileName(fn));
				var newFileLocation = HttpContext.Server.MapPath(newFileName);
				if (Directory.Exists(Path.GetDirectoryName(newFileLocation)) == false)
					Directory.CreateDirectory(Path.GetDirectoryName(newFileLocation));

				img.Save(newFileLocation);
				return Json(new { success = true, avatarFileLocation = newFileName });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, errorMessage = "Unable to upload file.\nERRORINFO: " + ex.Message });
			}
		}

		private bool IsImage(HttpPostedFileBase file)
		{
			if (file == null) return false;
			return file.ContentType.Contains("image") || _imageFileExtensions.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
		}

		private string GetTempSavedFilePath(HttpPostedFileBase file)
		{
			// Define destination
			var serverPath = HttpContext.Server.MapPath(TempFolder);
			if (Directory.Exists(serverPath) == false)
			{
				Directory.CreateDirectory(serverPath);
			}

			// Generate unique file name
			var fileName = Path.GetFileName(file.FileName);
			fileName = SaveTemporaryAvatarFileImage(file, serverPath, fileName);

			// Clean up old files after every save
			CleanUpTempFolder(1);
			return Path.Combine(TempFolder, fileName);
		}

		private static string SaveTemporaryAvatarFileImage(HttpPostedFileBase file, string serverPath, string fileName)
		{
			var img = new WebImage(file.InputStream);
			var ratio = img.Height / (double)img.Width;
			img.Resize(AvatarScreenWidth, (int)(AvatarScreenWidth * ratio));

			var fullFileName = Path.Combine(serverPath, fileName);
			if (System.IO.File.Exists(fullFileName))
			{
				System.IO.File.Delete(fullFileName);
			}

			img.Save(fullFileName);
			return Path.GetFileName(img.FileName);
		}

		private void CleanUpTempFolder(int hoursOld)
		{
			try
			{
				var currentUtcNow = DateTime.UtcNow;
				var serverPath = HttpContext.Server.MapPath("/Temp");
				if (!Directory.Exists(serverPath)) return;
				var fileEntries = Directory.GetFiles(serverPath);
				foreach (var fileEntry in fileEntries)
				{
					var fileCreationTime = System.IO.File.GetCreationTimeUtc(fileEntry);
					var res = currentUtcNow - fileCreationTime;
					if (res.TotalHours > hoursOld)
						System.IO.File.Delete(fileEntry);
				}
			}
			catch
			{
				// Deliberately empty.
			}
		}

    }
}
