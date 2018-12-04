namespace Instagreat.Data.Models
{
    using System;
    using System.Collections.Generic;
    
    public class Image
    {
        public int Id { get; set; }
        
        public byte[] Picture { get; set; }
        
        public User User { get; set; }

        public string UserId { get; set; }

        public string ToImageString()
        {
            return string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(this.Picture));
        }

    }
}
