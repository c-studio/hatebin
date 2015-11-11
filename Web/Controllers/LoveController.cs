using Interactive.HateBin.Data;
using Interactive.HateBin.Models;
using Interactive.HateBin.Services;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Interactive.HateBin.Controllers
{
    [Authorize(Roles = "Approved")]
    public class LoveController : Controller
    {
        private LoveRepository repository => new LoveRepository();
        private EmailService emailService => new EmailService();

        public ActionResult Index()
        {
            var items = repository.GetAllPending();
            return View(items);
        }

        public ActionResult SendLove(int id)
        {
            var item = repository.Get(id);
            return View(new SendLoveViewModel { Love = item, SelectedItem = string.Empty, Message = string.Empty});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendLove(int id, SendLoveViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedItem == null)
                {
                    model.Love = repository.Get(id);
                    model.Message = "Du måste välja ett av meddelandena.";
                    model.SelectedItem = string.Empty;
                    return View(model);
                }
                else
                {
                    var item = repository.Get(id);
                    item.Sent++;
                    item = repository.Save(item);

                    var body = "";
                    var path = "";
                    Attachment attachment = null;

                    switch (model.SelectedItem)
                    {
                        case "Catgif":
                            path = HostingEnvironment.MapPath("~/Content/images/sleepwalker.gif") ?? "";
                            attachment = new Attachment(path, "image/png");
                            attachment.ContentDisposition.Inline = true;
                            body = $"<html><body><img src='cid:{attachment.ContentId}'></body></html>";
                            await emailService.SendMail(item.Email, body, attachment);
                            break;

                        case "Message":
                            await emailService.SendMail(item.Email, "Kämpa! Du klarar det!");
                            break;

                        case "Heart":
                            path = HostingEnvironment.MapPath("~/Content/images/heart.png") ?? "";
                            attachment = new Attachment(path, "image/png");
                            attachment.ContentDisposition.Inline = true;
                            body = $"<html><body><div><img src='cid:{attachment.ContentId}'/><div></body></html>";
                            await emailService.SendMail(item.Email, body, attachment);
                            break;

                        default:
                            break;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}