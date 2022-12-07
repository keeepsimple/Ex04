using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Ex04.Models
{
    public class User : IdentityUser
    {
        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Rate> Rates { get; set; }
    }
}
