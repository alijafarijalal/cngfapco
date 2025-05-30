using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace cngfapco.Models
{
    [Table("Roles")]
    public partial class Role
    {
        public Role()
        {
            Permissions = new HashSet<Permission>();
            Users = new HashSet<User>();
        }

        [Key]
        public int Role_Id { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name ="عنوان نقش")]
        public string RoleName { get; set; }

        [Display(Name = "شرح نقش")]
        public string RoleDescription { get; set; }

        [Display(Name = "مدیر سیستم")]
        public bool IsSysAdmin { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
