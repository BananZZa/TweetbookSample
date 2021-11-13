using System.Collections.Generic;

namespace Domain.Entities
{
    public class Tag
    {
        public string TagId { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}