using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
