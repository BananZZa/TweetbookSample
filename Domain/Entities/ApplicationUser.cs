using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser<long>
    {
        public ICollection<Post> Posts { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}