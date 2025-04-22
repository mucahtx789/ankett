using ankett.Models;

namespace ankett.Models.Dto
{
    public class RegisterDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
