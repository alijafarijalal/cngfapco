using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cngfapco.Models
{
    /// <summary>
    /// جهت صدور حواله انبار
    /// </summary>
    [Table("tbl_Remittances")]
    public class Remittances
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "شماره حواله")]
        public string Number { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "طرح تقسیم مربوطه")]
        public int? DivisionPlanID { get; set; }
        [ForeignKey("DivisionPlanID")]
        public virtual DivisionPlan DivisionPlan { get; set; }

        [Display(Name = "تاریخ صدور")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "صادر کننده")]
        public int? Creator { get; set; }
        [ForeignKey("Creator")]
        public virtual User User { get; set; }

        [Display(Name = "وضعیت تایید کارگاه")]
        public bool? Status { get; set; }

        [Display(Name = "تاریخ تایید")]
        public DateTime? StatusDate { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "ناقص")]
        public bool? Incomplete { get; set; }

        [Display(Name = "موارد ناقص")]
        [DataType(DataType.MultilineText)]
        public string IncompleteDesc { get; set; }
       
    }

    /// <summary>
    /// جهت درج اطلاعات جزئیات حمل و تحویل کالاهای حواله انبار
    /// </summary>
    [Table("tbl_RemittanceDetails")]
    public class RemittanceDetails
    {
        public int ID { get; set; }

        [Display(Name = "شماره حواله")]
        public int? RemittancesID { get; set; }
        [ForeignKey("RemittancesID")]
        public virtual Remittances Remittances { get; set; }

        [Display(Name = "تاریخ صدور")]
        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "وسیله نقلیه")]
        public string Vehicle { get; set; }

        [Display(Name = "شماره پلاک")]
        public string Plate { get; set; }

        [Display(Name = "شماره بارنامه")]
        public string BillofLading { get; set; }

        [Display(Name = "تحویل گیرنده")]
        public string Transferee { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "صادر کننده")]
        public string Creator { get; set; }

        [Display(Name = "کرایه حمل")]
        public double? CarryFare { get; set; }

        [Display(Name = "نام باربری")]
        public string CarrierName { get; set; }

        [Display(Name = "فایل پیوست")]
        public string Attachment { get; set; }

        [Display(Name = "وضعیت تایید کارشناس")]
        public bool? CheckStatus { get; set; }

        [Display(Name = "تاریخ کنترل")]
        public DateTime? CkeckedDate { get; set; }

        [Display(Name = "تایید کارشناس ناظر")]
        public string CkeckedUser { get; set; }

        [Display(Name = "تاریخ تایید مالی")]
        public DateTime? FinancialDate { get; set; }

        [Display(Name = "تایید کارشناس مالی")]
        public string FinancialUser { get; set; }

        [Display(Name = "تاریخ تایید مدیر")]
        public DateTime? ManagerDate { get; set; }

        [Display(Name = "تایید مدیر تبدیل")]
        public string ManagerUser { get; set; }

        [Display(Name = "وضعیت تایید مالی")]
        public bool? FinancialCheckStatus { get; set; }

        [Display(Name = "وضعیت تایید مدیر")]
        public bool? ManagerCheckStatus { get; set; }

    }

    /// <summary>
    /// جهت صدور حواله انبار در طرح فروش آزاد کالا
    /// </summary>
    [Table("tbl_FreeSaleRemittances")]
    public class FreeSaleRemittances
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "شماره حواله")]
        public string Number { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "شماره فاکتور")]
        public string InvoiceCode { get; set; }

        [Display(Name = "تاریخ صدور")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "صادر کننده")]
        public string Creator { get; set; }

        [Display(Name = "وضعیت تایید تحویل")]
        public bool? Status { get; set; }

        [Display(Name = "تاریخ تایید")]
        public DateTime? StatusDate { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "ناقص")]
        public bool? Incomplete { get; set; }

        [Display(Name = "موارد ناقص")]
        [DataType(DataType.MultilineText)]
        public string IncompleteDesc { get; set; }

    }

    /// <summary>
    ///  جهت درج اطلاعات جزئیات حمل و تحویل کالاهای حواله انبار فروش آزاد
    /// </summary>
    [Table("tbl_FreeSaleRemittanceDetails")]
    public class FreeSaleRemittanceDetails
    {
        public int ID { get; set; }

        [Display(Name = "حواله مربوطه")]
        public int? RemittancesID { get; set; }
        [ForeignKey("RemittancesID")]
        public virtual FreeSaleRemittances Remittances { get; set; }

        [Display(Name = "تاریخ")]
        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "وسیله نقلیه")]
        public string Vehicle { get; set; }

        [Display(Name = "شماره پلاک")]
        public string Plate { get; set; }

        [Display(Name = "شماره بارنامه")]
        public string BillofLading { get; set; }

        [Display(Name = "تحویل گیرنده")]
        public string Transferee { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "تاریخ صدور")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "صادر کننده")]
        public string Creator { get; set; }

        [Display(Name = "کرایه حمل")]
        public double? CarryFare { get; set; }

        [Display(Name = "نام باربری")]
        public string CarrierName { get; set; }

        [Display(Name = "فایل پیوست")]
        public string Attachment { get; set; }

    }
}