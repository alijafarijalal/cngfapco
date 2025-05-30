using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cngfapco.Models
{
    /// <summary>
    /// [جدول مغایرت های سایت پخش، اتحادیه و فن آوران در ثبت نام و تبدیل
    /// </summary>
    [Table("tbl_Contradictions")]
    public class Contradiction
    {
        [Key]
        public int ID { get; set; }
        public int? WorkshopId { get; set; }
        [ForeignKey("WorkshopId")]
        public virtual Workshop Workshops { get; set; }

        [Display(Name = "شرح عملیات")]
        public string Description { get; set; }

        [Display(Name = "RD /پراید/ پیکان")]
        public int? VehicleType1 { get; set; }        

        [Display(Name = "پژو/ سمند")]
        public int? VehicleType2 { get; set; }

        [Display(Name = "وانت پیکان/ مزدا")]
        public int? VehicleType3 { get; set; }

        [Display(Name = "وانت نیسان")]
        public int? VehicleType4 { get; set; }

        [Display(Name = "وانت پراید")]
        public int? VehicleType5 { get; set; }

        [Display(Name = "سایر")]
        public int? VehicleTypeOther { get; set; }

        //public int? VehicleTypeId { get; set; }
        //[ForeignKey("VehicleTypeId")]
        //public virtual VehicleType VehicleTypes { get; set; }

        [Display(Name = "تاریخ عملیات")]
        public DateTime Date { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "ایجاد کننده")]
        public string Creator { get; set; }

    }

    /// <summary>
    /// [جدول کلی مغایرت های سایت پخش، اتحادیه و فن آوران در ثبت نام و تبدیل
    /// </summary>
    [Table("tbl_ContradictionTotals")]
    public class ContradictionTotal
    {
        [Key]
        public int ID { get; set; }
        public int? WorkshopId { get; set; }
        [ForeignKey("WorkshopId")]
        public virtual Workshop Workshops { get; set; }

        [Display(Name = "شرح عملیات")]
        public int? ContradictionTypeId { get; set; }
        [ForeignKey("ContradictionTypeId")]
        public virtual ContradictionType ContradictionType { get; set; }

        [Display(Name = "تاریخ عملیات")]
        public DateTime Date { get; set; }

        [Display(Name = "تعداد کل")]
        public double Count { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "ایجاد کننده")]
        public string Creator { get; set; }

    }

    [Table("tbl_ContradictionTypes")]
    public class ContradictionType
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "شرح عملیات")]
        public string Description { get; set; }

    }

}