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
        public int UpdateUserId { get; set; }
        public string Role { get; set; }
    }

    public class HateListViewModel
    {
        public bool ShowPrev { get; set; }
        public bool ShowNext { get; set; }
        public int PrevId { get; set; }
        public int NextId { get; set; }
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

    public class ModerateHateViewModel
    {
        public int DeleteId { get; set; }
    }
}