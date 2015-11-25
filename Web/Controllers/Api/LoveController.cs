using Interactive.HateBin.Data;
using Interactive.HateBin.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Interactive.HateBin.Controllers.Api
{
    public class LoveController : ApiController
    {
        private LoveRepository loveRepository => new LoveRepository();
        private UserRepository userRepository => new UserRepository();

        public HttpResponseMessage Post(IncomingLove item)
        {
            var user = userRepository.GetByToken(item.Token);
            if (user != null)
            {
                var loveitem = loveRepository.Save(new Love { Email = user.Email, Reason = item.Reason });
                return Request.CreateResponse(HttpStatusCode.Created, loveitem);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "No user with supplied token.");
        }
    }
}