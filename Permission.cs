using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace cngfapco.Models
{
    [Table("Permissions")]
    public partial class Permission
    {
        public Permission()
        {
            Roles = new HashSet<Role>();
        }

        [Key]
        public int Permission_Id { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(500)]
        public string PermissionDescription { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(500)]
        public string PersianDescription { get; set; }

        public virtual ICollection<Role > Roles { get; set; }
    }
}
