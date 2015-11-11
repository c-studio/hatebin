using AttributeRouting;
using AttributeRouting.Web.Http;
using Interactive.HateBin.Data;
using Interactive.HateBin.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Interactive.HateBin.Controllers.Api
{
    [RoutePrefix("api/hate")]
    public class HateController : ApiController
    {
        private HateRepository repository => new HateRepository();

        [HttpGet]
        [Authorize]
        [GET("stats")]
        public HateStats Stats()
        {
            return repository.GetStats();
        }

        public HttpResponseMessage Post(Hate item)
        {
            item = repository.Save(item);
            return Request.CreateResponse(HttpStatusCode.Created, item);
        }
    }
}