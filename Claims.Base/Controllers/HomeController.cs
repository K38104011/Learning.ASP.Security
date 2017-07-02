using System.Collections.Generic;
using System.Web.Http;

namespace Claims.Base.Controllers
{
    public class HomeController : ApiController
    {

        [Permission("MyApplication.ViewHomePage", "MyApplication.ViewIndex")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Permission("MyApplication.ViewHomePage", "MyApplication.DontExisted")]
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