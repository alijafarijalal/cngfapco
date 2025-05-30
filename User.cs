using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace cngfapco.Models
{
    [Table("Users")]
    public partial class User
    {
        public User()
        {
            Roles = new HashSet<Role>();
            SideBarItems = new HashSet<SideBarItem>();
            Workshops = new HashSet<Workshop>();
            Companies = new HashSet<AuditCompany>();
        }

        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(30)]
        [Display(Name ="نام کاربری")]
        [System.Web.Mvc.Remote("IsUserExists", "User", ErrorMessage = "نام کاربری قبلا استفاده شده !")]
        public string Username { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(30)]
        [Display(Name = "کلمه عبور")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "تاریخ آخرین ویرایش")]
        public DateTime? LastModified { get; set; }

        [Display(Name = "وضعیت کاربر")]
        public bool? Inactive { get; set; }

        [StringLength(50)]
        [Display(Name = "نام")]
        public string Firstname { get; set; }

        [StringLength(50)]
        [Display(Name = "نام خانوادگی")]
        public string Lastname { get; set; }

        [StringLength(100)]
        [Display(Name = "پست الکترونیک")]
        public string EMail { get; set; }

        [StringLength(100)]
        [Display(Name = "تلفن همراه")]
        public string MobileNumber { get; set; }

        [StringLength(1000)]
        [Display(Name = "سطح دسترسی")]
        public string DataPermission { get; set; }

        public int? WorkshopID { get; set; }
        [ForeignKey("WorkshopID")]
        public virtual Workshop Workshop { get; set; }

        [Display(Name = "عکس")]
        public string Image { get; set; }

        public int? AuditCompaniesID { get; set; }
        [ForeignKey("AuditCompaniesID")]
        public virtual AuditCompany AuditCompanies { get; set; }

        [Display(Name = "مدیر شرکت بازرسی")]
        public bool isAuditManager { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<SideBarItem> SideBarItems { get; set; }
        public virtual ICollection<Workshop> Workshops { get; set; }
        public virtual ICollection<AuditCompany> Companies { get; set; }

    }
    /// <summary>
    /// تغییر کلمه عبور کاربران
    /// </summary>
    public class ChangePassword
    {
        [Display(Name = "نام کاربری")]
        public string Username { get; set; }

        [Display(Name = "کلمه عبور قدیم")]
        [Required(ErrorMessage = "این فیلد خالیست.")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Display(Name = "کلمه عبور جدید")]
        [Required(ErrorMessage = "این فیلد خالیست.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "تکرار کلمه عبور جدید")]
        [Required(ErrorMessage = "این فیلد خالیست.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "رمز عبور جدید با تایید رمز عبور یکسان نیست.")]
        public string ReNewPassword { get; set; }
    }

    /// <summary>
    /// ثبت اطلاعات ورودهای کاربران
    /// </summary>
    [Table("UserEntranceInfo")]
    public class UserEntranceInfo
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "آخرین تاریخ ورود")]
        public DateTime EntranceDate { get; set; }

        [Display(Name = "IP Address")]
        public string IPAddress { get; set; }

        [Display(Name = "StatusLog")]
        public bool? Status { get; set; }

        public int? UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; }       
    }
    
}
