using System.Collections;
using Interactive.HateBin.Models;

namespace Interactive.HateBin.Controllers.Helpers
{
    public class PagingHelper
    {
        public static bool ShowPrev(int id, ICollection list, PageDirection direction, int pageSize )
        {
            return (direction == PageDirection.Forward && id != 0) ||
                   (direction == PageDirection.Backward && list.Count == pageSize + 1);
        }

        public static bool ShowNext(ICollection list, PageDirection direction, int pageSize)
        {
            return (direction == PageDirection.Forward && list.Count == pageSize + 1) ||
                   (direction == PageDirection.Backward);
        }
    }
}