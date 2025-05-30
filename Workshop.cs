using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cngfapco.Models
{
    [Table("tbl_Workshops")]
    public class Workshop
    {
        public Workshop()
        {
            Users = new HashSet<User>();
        }
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام کارگاه/ شرکت")]        
        public string Title { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "شهر")]
        public int? CityID { get; set; }
        [ForeignKey("CityID")]
        public virtual City City { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام")]
        public string OwnerName { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام خانوادگی مالک")]
        public string OwnerFamily { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تلفن ثابت")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تلفن همراه")]
        public string MobileNumber { get; set; }

        [Display(Name = "نمابر")]
        [DataType(DataType.PhoneNumber)]
        public string FaxNumber { get; set; }

        [Display(Name = "پست الکترونیک")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "آدرس")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "پروانه کسب")]
        public string BusinessLicense { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        public string Creator { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "کد کارگاه")]
        public string FapCode { get; set; }

        [Display(Name = "مرکز خدمات")]
        public bool? isServices { get; set; }

        [Display(Name = "شماره اقتصادی")]
        public string Economicalnumber { get; set; }

        [Display(Name = "شماره ثبت / شماره ملی")]
        public string Registrationnumber { get; set; }

        [Display(Name = "کد پستی")]
        public string Postalcode { get; set; }

        [Display(Name = "لوگو")]
        public string Logo { get; set; }

        public virtual ICollection<User> Users { get; set; }

        [Display(Name = "شرکت بازرسی")]
        public int? CompaniesID { get; set; }
        [ForeignKey("CompaniesID")]
        public virtual AuditCompany Companies { get; set; }

        [Display(Name = "نام بازرس")]
        public string Auditor { get; set; }

        [Display(Name = "اتمام همکاری")]
        public bool? closedServices { get; set; }

        [Display(Name = "تاریخ اتمام همکاری")]
        public DateTime? closedDate { get; set; }

        [Display(Name = "کد مرکز در پخش")]
        public string IRNGVCod { get; set; }
    }

    /// <summary>
    /// جهت ثبت اطلاعات انواع بیمه نامه هر کارگاه
    /// </summary>
    [Table("tbl_WorkshopInsurance")]
    public class WorkshopInsurance
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "نام کارگاه/ شرکت")]
        public int? WorkshopID { get; set; }
        [ForeignKey("WorkshopID")]
        public virtual Workshop Workshop { get; set; }

        [Display(Name = "نام شرکت بیمه گر")]
        public int? InsuranceCompaniesID { get; set; }
        [ForeignKey("InsuranceCompaniesID")]
        public virtual InsuranceCompany InsuranceCompanies { get; set; }

        [Display(Name = "نوع بیمه نام")]
        public int? InsuranceTypesID { get; set; }
        [ForeignKey("InsuranceTypesID")]
        public virtual InsuranceType InsuranceTypes { get; set; }

        [Display(Name = "تاریخ شروع بیمه نامه")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "تاریخ خاتمه بیمه نامه")]
        public DateTime? FinishDate { get; set; }

        [Display(Name = "تصویر بیمه نامه")]
        public string Image { get; set; }

        [Display(Name = "ملاحظات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        public string Creator { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "مبلغ پوشش")]
        [DataType(DataType.Currency)]
        public double Value { get; set; }
    }

    /// <summary>
    /// جهت ثبت اطلاعات شرکت های بیمه گر
    /// </summary>
    [Table("tbl_InsuranceCompanies")]
    public class InsuranceCompany
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام شرکت بیمه گر")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }

    /// <summary>
    /// جهت ثبت اطلاعات نوع بیمه نامه
    /// </summary>
    [Table("tbl_InsuranceTypes")]
    public class InsuranceType
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نوع بیمه نامه")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }

    /// <summary>
    /// جهت ثبت اطلاعات شرکت های بازرسی هر کارگاه
    /// </summary>
    [Table("tbl_AuditComponies")]
    public class AuditCompany
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "نام شرکت بازرسی")]
        public string Title { get; set; }
        
        [Display(Name = "ملاحظات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }

    /// <summary>
    /// جهت ثبت اطلاعات اپراتور هر کارگاه
    /// </summary>
    [Table("tbl_Operators")]
    public class Operator
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام کارگاه/ شرکت")]
        public int? WorkshopID { get; set; }
        [ForeignKey("WorkshopID")]
        public virtual Workshop Workshop { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام")]
        public string Name { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام خانوادگی")]
        public string Family { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تلفن ثابت")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تلفن همراه")]
        public string MobileNumber { get; set; }

        [Display(Name = "پست الکترونیک")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "آدرس")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "عکس")]
        public string Image { get; set; }

        [Display(Name = "وضعیت")]
        public bool? Status { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        public string Creator { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }

    }
    
    /// <summary>
    /// جهت ثبت اطلاعات استان ها
    /// </summary>
    [Table("tbl_States")]
    public class States
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "نام استان")]
        public string Title { get; set; }
    }
   
    /// <summary>
    /// جهت ثبت اطلاعات شهرها
    /// </summary>
    [Table("tbl_Cities")]
    public class City
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "نام شهر")]
        public string Title { get; set; }

        public int? StateID { get; set; }
        [ForeignKey("StateID")]
        public virtual States State { get; set; }
    }
}