using ecom.presentation.website.helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ecom.presentation.website.API
{
    public class SpecialityController : ApiController
    {
        // GET api/<controller>
        public string Get()
        {
            return JsonConvert.SerializeObject(
                SpecialityHelper.GetAll().Select(item => new { id = item.ID, name = item.NameEN }),
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return SpecialityHelper.GetByID(id)?.NameEN;
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}