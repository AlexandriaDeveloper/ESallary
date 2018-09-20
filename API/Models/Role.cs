using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Models {
    public class Role : IdentityRole<int> {
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ICollection<UserRole> UserRoles  { get; set; }
    }
}