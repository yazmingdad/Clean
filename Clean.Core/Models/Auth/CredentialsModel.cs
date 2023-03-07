namespace Clean.Core.Models.Auth
{
    public class CredentialsModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string Grant_type { get; set; } = "password";
    }
}
