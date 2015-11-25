using System.Collections;
using System.Linq;
using Interactive.HateBin.Data;
using Interactive.HateBin.Models;
using System.Web.Mvc;
using Interactive.HateBin.Controllers.Helpers;

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
                ShowPrev = PagingHelper.ShowPrev(id, hate, dir, PageSize),
                ShowNext = PagingHelper.ShowNext(hate, dir, PageSize)
            });
        }

        public ActionResult Love(int id = 0, PageDirection dir = PageDirection.Forward)
        {
            var love = loveRepository.GetList(id, PageSize + 1, dir).ToList();

            return View(new LoveListViewModel
            {
                LoveList = love.Take(PageSize).OrderByDescending(x => x.Id).ToList(),
                ShowPrev = PagingHelper.ShowPrev(id, love, dir, PageSize),
                ShowNext = PagingHelper.ShowNext(love, dir, PageSize)
            });
        }

        public ActionResult Users(int id = 0, PageDirection dir = PageDirection.Forward)
        {
            var users = userRepository.GetList(id, PageSize + 1, dir).ToList();
            return View(new UserListViewModel
            {
                UserList = users.Take(PageSize).OrderByDescending(x => x.Id).ToList(),
                ShowPrev = PagingHelper.ShowPrev(id, users, dir, PageSize),
                ShowNext = PagingHelper.ShowNext(users, dir, PageSize)
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
    }
}