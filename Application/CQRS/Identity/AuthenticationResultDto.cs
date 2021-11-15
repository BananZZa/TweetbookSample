using System.Collections.Generic;

namespace Application.CQRS.Identity
{
    public class AuthenticationResultDto
    {
        public string Token { get; set; }
        public  bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public string RefreshToken { get; set; }
    }
}