using Microsoft.AspNetCore.Identity;

namespace IdentityMessageBoard.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Message> Messages { get; set; }
    }
}
