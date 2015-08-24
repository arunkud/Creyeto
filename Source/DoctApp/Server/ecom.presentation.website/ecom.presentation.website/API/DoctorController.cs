using ecom.presentation.website.helper;
using ecom.presentation.website.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace ecom.presentation.website.API
{
    public class DoctorController : ApiController
    {
        public static int _checkSumSalt = Convert.ToInt32(ConfigurationManager.AppSettings["salt"].ToString());

        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            try
            {
                NameValueCollection nvc = HttpContext.Current.Request.QueryString;

                int? cityId = 0;
                int? cityLocationId = null;
                int? specialityId = null;
                int? checkSumVal = null;
                double? latitude = null;
                double? longitude = null;
                int? hospitalId = null;

                string strCityID = nvc["cid"] != null ? nvc["cid"].ToString() : null;
                string strCityLocationId = nvc["clid"] != null ? nvc["cid"].ToString() : null;
                string strSpecialityId = nvc["sid"] != null ? nvc["sid"].ToString() : null;
                string strCheckSum = nvc["cs"] != null ? nvc["cs"].ToString() : null;
                string strLatitude = nvc["lt"] != null ? nvc["lt"].ToString() : null;
                string strLongitude = nvc["lg"] != null ? nvc["lg"].ToString() : null;
                string strHospital = nvc["hid"] != null ? nvc["hid"].ToString() : null;

                if (!string.IsNullOrEmpty(strCityID))
                    cityId = int.Parse(strCityID);

                if (!string.IsNullOrEmpty(strCityLocationId))
                    cityLocationId = int.Parse(strCityLocationId);

                if (!string.IsNullOrEmpty(strSpecialityId))
                    specialityId = int.Parse(strSpecialityId);

                if (!string.IsNullOrEmpty(strCheckSum))
                    checkSumVal = int.Parse(strCheckSum);

                if (!string.IsNullOrEmpty(strLatitude))
                    latitude = double.Parse(strLatitude);

                if (!string.IsNullOrEmpty(strLongitude))
                    longitude = double.Parse(strLongitude);

                if (!string.IsNullOrEmpty(strHospital))
                    hospitalId = int.Parse(strHospital);

                //if (IsValidCheckSum(cityId, specialityId, checkSumVal))
                {
                    Entities dbContext = new Entities();
                    if (latitude.HasValue && longitude.HasValue && specialityId != 0)
                    {
                        var doctors = from d in dbContext.GetAllDoctors(specialityId, latitude, longitude) select d;
                        return Request.CreateResponse(HttpStatusCode.OK, Json(doctors.ToList()));
                    }
                    else if (specialityId != 0 && cityId.HasValue && cityId.Value != 0)
                    {
                        var doctors = from d in dbContext.GetAllDoctorsByCity(specialityId, cityId) select d;
                        return Request.CreateResponse(HttpStatusCode.OK, Json(doctors.ToList()));
                    }
                    else if (hospitalId.HasValue)
                    {
                        var doctors = from d in dbContext.GetAllDoctorsByHospital(hospitalId, specialityId) select 
                                      new
                                      {
                                          d.NameEN                                       
                                      };
                        var items = doctors.ToList();
                        return Request.CreateResponse(HttpStatusCode.OK, Json(items));
                    }
                }
            }
            catch
            {
                return null;
            }

            return null;

        }

        // GET api/<controller>/5
        public HttpResponseMessage Get(string sid)
        {
            return null;
        }

        // POST api/<controller>
        public void Post(string value)
        {
            return;
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        private bool IsValidCheckSum(int? cityid, int? specialityId, int? checkSum)
        {
            return (((cityid.HasValue ? cityid.Value : 0) + (specialityId.HasValue ? specialityId.Value : 0) + (checkSum.HasValue ? checkSum.Value : 0)) == _checkSumSalt);
        }
    }
}