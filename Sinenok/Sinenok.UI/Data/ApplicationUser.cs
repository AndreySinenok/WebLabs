using Microsoft.AspNetCore.Identity;

namespace Sinenok.UI.Data
{
    public class ApplicationUser : IdentityUser
    {
        public byte[]? Avatar { get; set; }
        public string? MimeType { get; set; }
    }
}
