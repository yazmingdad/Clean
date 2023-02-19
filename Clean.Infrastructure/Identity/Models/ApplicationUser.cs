using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Clean.Infrastructure.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [DefaultValue(false)]
        public bool IsDown { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsFirstLogin { get; set; } = true;

    }
}
