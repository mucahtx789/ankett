namespace ankett.Models.Dto
{
    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string RecaptchaToken { get; set; }
    }
}
