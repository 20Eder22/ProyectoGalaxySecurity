using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Infraestructure
{
    public class UserExtension : IdentityUser
    {
        public Guid UserId { get; set; }

        [StringLength(200)]
        public string FullName { get; set; } = default!;
    }
}
