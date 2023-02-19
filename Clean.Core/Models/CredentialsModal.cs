namespace Clean.Core.Models
{
    public class CredentialsModal
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string Grant_type { get; set; } = "password";
    }
}
