namespace JWT_Auth.Entities
{
    public class User
    {
        public Guid ID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; }
    }
}
