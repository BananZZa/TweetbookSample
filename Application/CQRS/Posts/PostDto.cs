using System;

namespace Application.CQRS.Posts
{
    public class PostDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Tags { get; set; }
        public long UserId { get; set; }
    }
}