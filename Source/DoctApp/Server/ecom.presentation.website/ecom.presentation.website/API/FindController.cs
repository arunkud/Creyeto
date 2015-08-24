using ecom.presentation.website.helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace ecom.presentation.website.API
{
    public class FindController : ApiController
    {
        // GET api/<controller>
        public string Get(string query)
        {
            if (!string.IsNullOrEmpty(query) && query.Length > 0)
            {
                return JsonConvert.SerializeObject(
                    new {
                        suggestions = SearchInputHelper.GetByName(query)
                    }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            }
            return string.Empty;
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