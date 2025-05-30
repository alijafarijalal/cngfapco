using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cngfapco.Models
{
    /// <summary>
    /// اطلاعات خروجی سامانه اتحادیه برای گزارش مغایرت و ...
    /// </summary>
    [Table("tbl_IRNGV")]
    public class IRNGV
    {       
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name = "تاریخ پذیرش")]
        public string Acceptance { get; set; }

        [Display(Name = "کد ملی مالک")]
        public string NationalCode { get; set; }

        [Display(Name = "سازنده خودرو")]
        public string Constractor { get; set; }

        [Display(Name = "نام خودرو")]
        public string VehicleType { get; set; }

        [Display(Name = "استان")]
        public string State { get; set; }

        [Display(Name = "شهر")]
        public string City { get; set; }

        [Display(Name = "پلاک")]
        public string Plate { get; set; }

        [Display(Name = "شماره موتور")]
        public string EngineNumber { get; set; }

        [Display(Name = "شماره شاسی")]
        public string ChassisNumber { get; set; }

        [StringLength(4)]
        [Display(Name = "سال ساخت")]
        public string ConstructionYear { get; set; }

        [Display(Name = "نام مرکز")]
        public string Workshop { get; set; }

        [Display(Name = "کد مرکز")]
        public string WorkshopID { get; set; }

        [Display(Name = "شماره گواهی سلامت")]
        public string InspectionCertificateNumber { get; set; }

        [Display(Name = "تاریخ صدور گواهی")]
        public string DateofCertification { get; set; }

        public string Column1 { get; set; }

        [Display(Name = "نام شرکت بیمه")]
        public string Insurance { get; set; }

        [Display(Name = "شماره بیمه نامه")]
        public string InsuranceNumber { get; set; }

        [Display(Name = "شرکت بازرسی")]
        public string InspectionCompany { get; set; }

        [Display(Name = "نام بازرس")]
        public string InspectorName { get; set; }

        [Display(Name = "مخزن")]
        public string Cylinder { get; set; }

        [Display(Name = "شیرمخزن")]
        public string Valve { get; set; }

        [Display(Name = "رگلاتور")]
        public string Regulator { get; set; }

        [Display(Name = "شیر قطع کن")]
        public string CutoffValve { get; set; }

        [Display(Name = "شیر پرکن")]
        public string FillingValve { get; set; }
        public string Creator { get; set; }
        public DateTime CreateDate { get; set; }

    }

    /// <summary>
    /// اطلاعات خروجی سامانه پخش برای گزارش مغایرت و ...
    /// </summary>
    [Table("tbl_GCR")]
    public class GCR
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "شماره پیگیری")]
        public string IssueTracking { get; set; }

        [Display(Name = "تاریخ نوبت")]
        public string DateofTurn { get; set; }

        [Display(Name = "وضعیت")]
        public string Status { get; set; }

        [Display(Name = "نام و نام خانوادگی مالک")]
        public string OwnerFullName { get; set; }

        [Display(Name = "کد ملی مالک")]
        public string NationalCode { get; set; }

        [Display(Name = "شماره مالک")]
        public string OwnerMobile { get; set; }

        [Display(Name = "پلاک")]
        public string Plate { get; set; }

        [Display(Name = "پلاک ناجی")]
        public string NajiPlate { get; set; }

        [Display(Name = "نوع کاربری")]
        public string TypeofUsed { get; set; }

        [Display(Name = "برند")]
        public string VehicleType { get; set; }

        [Display(Name = "مدل")]
        public string Model { get; set; }

        [Display(Name = "نام پیمانکار")]
        public string Contractor { get; set; }

        [Display(Name = "نام کارگاه")]
        public string Workshop { get; set; }

        [Display(Name = "استان کارگاه")]
        public string State { get; set; }

        [Display(Name = "شهر کارگاه")]
        public string City { get; set; }

        [Display(Name = "آدرس کارگاه")]
        public string WorkshopAddress { get; set; }

        [Display(Name = "شماره تماس کارگاه")]
        public string WorkshopPhone { get; set; }

        [Display(Name = "تاریخ تبدیل")]
        public string ConversionDate { get; set; }

        [Display(Name = "شناسه تبدیل")]
        public string ConversionID { get; set; }

        [Display(Name = "حجم مخزن")]
        public string CylinderBulk { get; set; }

        [Display(Name = "سریال مخزن")]
        public string CylinderSerial { get; set; }

        [Display(Name = "سریال شیرمخزن")]
        public string ValveSerial { get; set; }

        [Display(Name = "سریال رگلاتور")]
        public string RegulatorSerial { get; set; }

        [Display(Name = "شماره گواهی تبدیل")]
        public string ConversionCertificateNumber { get; set; }

        [Display(Name = "شناسه گواهی سلامت")]
        public string HealthCertificateID { get; set; }

        [Display(Name = "شماره موتور")]
        public string EngineNumber { get; set; }

        [Display(Name = "شماره pan")]
        public string panNumber { get; set; }

        [Display(Name = "شماره شاسی")]
        public string ChassisNumber { get; set; }

        [Display(Name = "مسافربر شخصی")]
        public string PersonalPassenger { get; set; }
        public string Creator { get; set; }
        public DateTime CreateDate { get; set; }

        [StringLength(4)]
        [Display(Name = "سال ساخت")]
        public string ConstructionYear { get; set; }

    }

    /// <summary>
    /// نگهداری اطلاعات خروجی سامانه اتحادیه برای محاسبه و صدور فاکتور مخازن ضایعاتی
    /// </summary>
    [Table("tbl_IRNGVDamages")]
    public class IRNGVDamages
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "تاریخ پذیرش")]
        public string Acceptance { get; set; }

        [Display(Name = "کد ملی مالک")]
        public string NationalCode { get; set; }

        [Display(Name = "سازنده خودرو")]
        public string Constractor { get; set; }

        [Display(Name = "نام خودرو")]
        public string VehicleType { get; set; }

        [Display(Name = "استان")]
        public string State { get; set; }

        [Display(Name = "شهر")]
        public string City { get; set; }

        [Display(Name = "پلاک")]
        public string Plate { get; set; }

        [Display(Name = "شماره موتور")]
        public string EngineNumber { get; set; }

        [Display(Name = "شماره شاسی")]
        public string ChassisNumber { get; set; }

        [StringLength(4)]
        [Display(Name = "سال ساخت")]
        public string ConstructionYear { get; set; }

        [Display(Name = "نام مرکز")]
        public string Workshop { get; set; }

        [Display(Name = "کد مرکز")]
        public string WorkshopID { get; set; }

        [Display(Name = "شماره گواهی سلامت")]
        public string InspectionCertificateNumber { get; set; }

        [Display(Name = "تاریخ صدور گواهی")]
        public string DateofCertification { get; set; }

        [Display(Name = "سریال")]
        public string Serial { get; set; }

        [Display(Name = "نام شرکت بیمه")]
        public string Insurance { get; set; }

        [Display(Name = "شماره بیمه نامه")]
        public string InsuranceNumber { get; set; }

        [Display(Name = "شرکت بازرسی")]
        public string InspectionCompany { get; set; }

        [Display(Name = "نام بازرس")]
        public string InspectorName { get; set; }

        [Display(Name = "مخزن")]
        public string Cylinder { get; set; }

        [Display(Name = "شیرمخزن")]
        public string Valve { get; set; }

        [Display(Name = "رگلاتور")]
        public string Regulator { get; set; }

        [Display(Name = "نسل")]
        public string Generation { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "حجم مخزن اول")]
        public string Literage { get; set; }

        [Display(Name = "حجم مخزن دوم")]
        public string Literage_2 { get; set; }
        public string Creator { get; set; }
        public DateTime CreateDate { get; set; }

    }

}