using System;

namespace PostcardApp.Models 
{
    public class History 
    {
        public long HistoryId { get; set; }
        public long ImageId { get; set; }
        public string EmailSentTo { get; set; }
        public DateTime EmailSentOn { get; set; }

        public Image Image { get; set; }
    }
}