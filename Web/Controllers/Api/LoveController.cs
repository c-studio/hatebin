using Interactive.HateBin.Data;
using Interactive.HateBin.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Interactive.HateBin.Controllers.Api
{
    public class LoveController : ApiController
    {
        private LoveRepository repository => new LoveRepository();

        public HttpResponseMessage Post(Love item)
        {
            item = repository.Save(item);
            return Request.CreateResponse(HttpStatusCode.Created, item);
        }
    }
}