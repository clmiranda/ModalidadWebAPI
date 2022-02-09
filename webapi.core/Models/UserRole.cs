using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace webapi.core.Models
{
    public partial class UserRole: IdentityUserRole<int>
    {
        [Required]
        public virtual Role Role { get; set; }
        [Required]
        public virtual User User { get; set; }
    }
}
