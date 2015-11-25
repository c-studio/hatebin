using System;

namespace Interactive.HateBin.Models
{
    public class IncomingLove
    {
        public Guid Token { get; set; }
        public string Reason { get; set; }
    }

    public class Love
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Email { get; set; }
        public string Reason { get; set; }
        public int Sent { get; set; }
    }

    public class SendLoveViewModel
    {
        public Love Love { get; set; }
        public string Message { get; set; }
        public string SelectedItem { get; set; }      
    }

}