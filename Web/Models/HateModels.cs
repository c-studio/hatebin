using System;
using System.Collections.Generic;

namespace Interactive.HateBin.Models
{
    public class Hate
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string Network { get; set; }
        public long NetworkId { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public IList<string> Categories { get; set; }
        public Guid Token { get; set; }
    }

    public class HateStats
    {
        public int HateCount { get; set; }
        public int AdHominemCount { get; set; }
        public int RacistCount { get; set; }
        public int SexistCount { get; set; }
        public int OtherCount { get; set; }
    }
}