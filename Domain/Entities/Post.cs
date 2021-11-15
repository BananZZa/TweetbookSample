using System.Collections.Generic;

namespace Domain.Entities
{
    public class Post
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ApplicationUser User { get; set; }
        public long UserId { get; set; }
    }
}