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

        private const int PageSize = 15;

        public ActionResult Index(int hateid = 0, PageDirection dir = PageDirection.Forward)
        {           
            var hate = hateRepository.GetList(hateid, PageSize + 1, dir).ToList();
            var hatelist = hate.Take(PageSize).OrderByDescending(x => x.Id).ToList();
            return View(new HateListViewModel
            {
                ShowPrev = PagingHelper.ShowPrev(hateid, hate, dir, PageSize),
                ShowNext = PagingHelper.ShowNext(hate, dir, PageSize),
                HateList = hatelist,
                PrevId = hatelist.Count > 0 ? hatelist.First().Id : 0,
                NextId = hatelist.Count > 0 ? hatelist.Last().Id : 0
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ModerateHateViewModel model, int hateid = 0, PageDirection dir = PageDirection.Forward)
        {
            hateRepository.DeleteHate(model.DeleteId);
            return RedirectToAction("Index", new { hateid, dir});
        }

        public ActionResult Love(int loveid = 0, PageDirection dir = PageDirection.Forward)
        {
            var love = loveRepository.GetList(loveid, PageSize + 1, dir).ToList();

            return View(new LoveListViewModel
            {
                LoveList = love.Take(PageSize).OrderByDescending(x => x.Id).ToList(),
                ShowPrev = PagingHelper.ShowPrev(loveid, love, dir, PageSize),
                ShowNext = PagingHelper.ShowNext(love, dir, PageSize)
            });
        }

        public ActionResult Users(int userid = 0, PageDirection dir = PageDirection.Forward)
        {
            var users = userRepository.GetList(userid, PageSize + 1, dir).ToList();
            return View(new UserListViewModel
            {
                UserList = users.Take(PageSize).OrderByDescending(x => x.Id).ToList(),
                ShowPrev = PagingHelper.ShowPrev(userid, users, dir, PageSize),
                ShowNext = PagingHelper.ShowNext(users, dir, PageSize)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Users(UpdateRolesViewModel model, int userid = 0, PageDirection dir = PageDirection.Forward)
        {
            if (ModelState.IsValid)
            {
                var user = userRepository.GetById(model.UpdateUserId);
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
            return RedirectToAction("Users", new { userid, dir });
        }
    }
}