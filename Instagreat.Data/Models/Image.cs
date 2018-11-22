namespace Instagreat.Data.Models
{
    using System.Collections.Generic;
    
    public class Image
    {
        public int Id { get; set; }
        
        public byte[] Picture { get; set; }
        
        public User User { get; set; }

        public string UserId { get; set; }

    }
}
