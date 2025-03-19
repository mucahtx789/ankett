namespace ankett.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; } // Role olarak IK personeli veya personel olacak
    }
    public enum UserRole
    {
        Admin, // İK personeli
        Employee // Personel
    }
}
