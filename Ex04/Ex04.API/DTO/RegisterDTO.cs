using System.ComponentModel.DataAnnotations;

namespace Ex04.API.DTO
{
    public class RegisterDTO
    {
        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
