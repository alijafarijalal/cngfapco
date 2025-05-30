using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cngfapco.Models
{
    [Table("tbl_Educations")]
    public class Education
    {
        public int ID { get; set; }
        public int? VehicleTypeId { get; set; }
        [ForeignKey("VehicleTypeId")]
        public virtual VehicleType VehicleType { get; set; }

        [Display(Name = "فایل رویه نصب")]
        public string File { get; set; }

        [Display(Name = "نوع سیستم")]
        public string Description { get; set; }

        [Display(Name = "تاریخ درج")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "وضعیت اعتبار")]
        public bool Status { get; set; }

        [Display(Name = "نسخه")]
        public string Version { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "لینک دانلود نرم افزار")]
        public string DownloadUrl { get; set; }

        [Display(Name = "دسته بندی")]
        public string Cat { get; set; }
    }
}