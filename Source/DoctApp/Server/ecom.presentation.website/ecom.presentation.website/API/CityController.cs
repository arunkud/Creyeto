using ecom.presentation.website.helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Web.Http;

namespace ecom.presentation.website.API
{
    public class CityController : ApiController
    {
        // GET api/<controller>
        public string Get()
        {
            return JsonConvert.SerializeObject(
                LocationHelper.GetAll().Select(item => new { id = item.ID, name = item.NameEN }), 
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
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