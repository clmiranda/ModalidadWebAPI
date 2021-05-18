using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace webapi.core.Models
{
    public partial class Role: IdentityRole<int>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
