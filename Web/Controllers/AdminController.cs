using System.Collections;
using System.Linq;
using Interactive.HateBin.Data;
using Interactive.HateBin.Models;
using System.Web.Mvc;

namespace Interactive.HateBin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private UserRepository userRepository => new UserRepository();
        private HateRepository hateRepository => new HateRepository();
        private LoveRepository loveRepository => new LoveRepository();

        private const int PageSize = 20;

        public ActionResult Index(int id = 0, PageDirection dir = PageDirection.Forward)
        {           
            var hate = hateRepository.GetList(id, PageSize + 1, dir).ToList();

            return View(new HateListViewModel
            {
                HateList = hate.Take(PageSize).OrderByDescending(x => x.Id).ToList(),
                ShowPrev = ShowPrev(id, hate, dir),
                ShowNext = ShowNext(hate, dir)
            });
        }

        public ActionResult Love(int id = 0, PageDirection dir = PageDirection.Forward)
        {
            var love = loveRepository.GetList(id, PageSize + 1, dir).ToList();

            return View(new LoveListViewModel
            {
                LoveList = love.Take(PageSize).OrderByDescending(x => x.Id).ToList(),
                ShowPrev = ShowPrev(id, love, dir),
                ShowNext = ShowNext(love, dir)
            });
        }

        public ActionResult Users(int id = 0, PageDirection dir = PageDirection.Forward)
        {
            var users = userRepository.GetList(id, PageSize + 1, dir).ToList();
            return View(new UserListViewModel
            {
                UserList = users.Take(PageSize).OrderByDescending(x => x.Id).ToList(),
                ShowPrev = ShowPrev(id, users, dir),
                ShowNext = ShowNext(users, dir)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Users(UpdateRolesViewModel model, int id = 0, PageDirection dir = PageDirection.Forward)
        {
            if (ModelState.IsValid)
            {
                var user = userRepository.GetById(model.UserId);
                if (user.Roles.Contains(model.Role))
                {
                    user.Roles.Remove(model.Role);
                }
                else
                {
                    user.Roles.Add(model.Role);
                }
                userRepository.Save(user);
            }
            return RedirectToAction("Users", new {id, dir});
        }


        private bool ShowPrev(int id, ICollection list, PageDirection direction)
        {
            return (direction == PageDirection.Forward && id != 0) ||
                   (direction == PageDirection.Backward && list.Count == PageSize + 1);
        }

        private bool ShowNext(ICollection list, PageDirection direction)
        {
            return (direction == PageDirection.Forward && list.Count == PageSize + 1) ||
                   (direction == PageDirection.Backward);
        }
    }
}