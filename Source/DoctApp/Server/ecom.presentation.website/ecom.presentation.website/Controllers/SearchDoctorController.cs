using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ecom.presentation.website.Models;

namespace ecom.presentation.website.Views.Search
{
	[Authorize]
    public class SearchDoctorController : Controller
    {
		public static int _checkSumSalt = Convert.ToInt32(ConfigurationManager.AppSettings["salt"].ToString());
        // GET: SearchDoctor
		[HttpPost]
		[AllowAnonymous]
        public ActionResult Index()
        {
			try
			{
				Request.InputStream.Seek(0, SeekOrigin.Begin);
				string jsonData = new StreamReader(Request.InputStream).ReadToEnd();
				NameValueCollection nvc = HttpUtility.ParseQueryString(jsonData);

				int? cityId = 0;
				int? cityLocationId = null;
				int? specialityId = null;
				int? checkSumVal = null;

				string strCityID = nvc["cid"] != null ? nvc["cid"].ToString() : null;
				string strCityLocationId = nvc["clid"] != null ? nvc["cid"].ToString() : null;
				string strSpecialityId = nvc["sid"] != null ? nvc["sid"].ToString() : null;
				string strCheckSum = nvc["cs"] != null ? nvc["cs"].ToString() : null;


				if (!string.IsNullOrEmpty(strCityID))
					cityId = int.Parse(strCityID);

				if (!string.IsNullOrEmpty(strCityLocationId))
					cityLocationId = int.Parse(strCityLocationId);

				if (!string.IsNullOrEmpty(strSpecialityId))
					specialityId = int.Parse(strSpecialityId);

				if (!string.IsNullOrEmpty(strCheckSum))
					checkSumVal = int.Parse(strCheckSum);


				if (IsValidCheckSum(cityId, cityLocationId, specialityId, checkSumVal))
				{
					//.Where(x => (x.CityID == cityId || x.CityID == null) &&
					//(x.CityLocationID == cityLocationId || x.CityLocationID == null) &&
					//(x.SpecialityID == specialityID || x.SpecialityID == null))
					DoctorEntities dbContext = new DoctorEntities();
					var doctors = from d in dbContext.Doctors
									join c in dbContext.Cities on d.CityID equals c.ID into cd from c in cd.DefaultIfEmpty()
									where (d.CityID == cityId || d.CityLocationID == cityLocationId) &&
									d.SpecialityID == specialityId &&
									d.IsActive == true
									select new
									{
										Name = d.NameEN,
										Phone = d.ContactNumber,
										Place = !string.IsNullOrEmpty(d.CityLocation != null ? d.CityLocation.NameEN : null) 
												? d.CityLocation.NameEN : !string.IsNullOrEmpty(d.City != null ? d.City.NameEN : null) ? d.City.NameEN : null,
										Fee = d.Fee,

									};

					return Json(doctors.ToList());
				}
			}
			catch (Exception ex)
			{
				return null;
			}
			
            return null;
        }

		private bool IsValidCheckSum(int? cityid, int? cityLocationId, int? specialityId, int? checkSum)
		{
			return (((cityid.HasValue ? cityid.Value : 0) + (cityLocationId.HasValue ? cityLocationId.Value : 0) + (specialityId.HasValue ? specialityId.Value : 0) + (checkSum.HasValue ? checkSum.Value : 0)) == _checkSumSalt);
		}
    }
}