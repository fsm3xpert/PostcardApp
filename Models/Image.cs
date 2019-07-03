using System;

namespace PostcardApp.Models 
{
    public class Image 
    {
        public long ImageId { get; set; }
        public string ImageName { get; set; }
        public string GeoTag { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}