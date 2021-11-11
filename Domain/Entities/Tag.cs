using System.Collections.Generic;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class Tag
    {
        public string TagId { get; set; }

        public ICollection<Post> Posts { get; set; }
        
        public long UserId { get; set; }
        public IUser User { get; set; }
    }
}