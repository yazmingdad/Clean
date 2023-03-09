namespace Clean.Core.Models.Auth
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public List<string> RoleNames { get; set; }

        public bool? IsFirstLogin { get; set; }

        public bool? IsDown { get; set; }

        public byte[]? Avatar { get; set; }

    }
}
