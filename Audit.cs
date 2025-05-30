using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cngfapco.Models
{
    [Table("tbl_Audits")]
    public class Audit
    {
        public int ID { get; set; }

        [Display(Name = "بازرس")]
        public int? UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual User Users { get; set; }

        [Display(Name = "تاریخ بازرسی")]
        public DateTime AuditDate { get; set; }

        [Display(Name = "کارگاه")]
        public int? WorkshopID { get; set; }
        [ForeignKey("WorkshopID")]
        public virtual Workshop Workshops { get; set; }

        [Display(Name = "شرح نواقص")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "ثبت کننده")]
        public string Creator { get; set; }
    }

    [Table("tbl_AuditFiles")]
    public class AuditFile
    {
        public int ID { get; set; }

        [Display(Name = "ارزیابی مرتبط")]
        public int? AuditID { get; set; }
        [ForeignKey("AuditID")]
        public virtual Audit Audit { get; set; }

        [Display(Name = "نوع مستندات")]
        public int? CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public virtual AuditCategory Category { get; set; }

        [Display(Name = "چک لیست")]
        public string CheckList { get; set; }

        [Display(Name = "تصویر")]
        public string Picture { get; set; }
    }

    [Table("tbl_AuditCategories")]
    public class AuditCategory
    {
        public int ID { get; set; }

        [Display(Name = "نوع مستندات")]
        public string Title { get; set; }
       
    }

    [Table("tbl_Auditors")]
    public class Auditor
    {
        public int ID { get; set; }
        
        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }

        [Display(Name = "شرکت بازرسی")]
        public int? CompaniesID { get; set; }
        [ForeignKey("CompaniesID")]
        public virtual AuditCompany Companies { get; set; }

        [Display(Name = "تصویر")]
        public string Picture { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "ثبت کننده")]
        public string Creator { get; set; }
    }

}