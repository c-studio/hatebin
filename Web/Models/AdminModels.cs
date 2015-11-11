using System.Collections.Generic;

namespace Interactive.HateBin.Models
{
    public enum PageDirection
    {
        Forward,
        Backward
    }

    public class UpdateRolesViewModel
    {
        public int UserId { get; set; }
        public string Role { get; set; }
    }

    public class HateListViewModel
    {
        public bool ShowPrev { get; set; }
        public bool ShowNext { get; set; }
        public IList<Hate> HateList { get; set; }
    }

    public class LoveListViewModel
    {
        public bool ShowPrev { get; set; }
        public bool ShowNext { get; set; }
        public IList<Love> LoveList { get; set; }
    }

    public class UserListViewModel
    {
        public bool ShowPrev { get; set; }
        public bool ShowNext { get; set; }
        public IList<User> UserList { get; set; }
    }
}