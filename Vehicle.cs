using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cngfapco.Models
{
    public class Vehicle
    {
    }
    /// <summary>
    /// اطلاعات اصلی در فرم ثبت خودرو
    /// </summary>
    [Table("tbl_VehicleRegistrations")]
    public class VehicleRegistration
    {
        [Key]
        public int ID { get; set; }

        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نوع خودرو")]
        public int? VehicleTypeID { get; set; }
        [ForeignKey("VehicleTypeID")]
        public virtual VehicleType VehicleType { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "سیستم")]
        public string System { get; set; }

        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نوع کاربری")]
        public int? TypeofUseID { get; set; }
        [ForeignKey("TypeofUseID")]
        public virtual TypeofUse TypeofUse { get; set; }
        //public string TypeofUse { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام")]
        public string OwnerName { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام خانوادگی مالک")]
        public string OwnerFamily { get; set; }

        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "اسکن کارت ملی (پشت و رو)")]
        //[DataType(DataType.Upload)]
        public string NationalCard { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "کد ملی")]
        public string NationalCode { get; set; }

       // [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تلفن ثابت")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تلفن همراه")]
        public string MobileNumber { get; set; }

        [Display(Name = "آدرس")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(4)]
        [Display(Name = "سال ساخت")]
        public string ConstructionYear { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "سمت چپ پلاک")]
        public string LeftNumberPlate { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "حروف پلاک")]
        public string AlphaPlate { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "سمت راست پلاک")]
        public string RightNumberPlate { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "قسمت ایران پلاک")]
        public string IranNumberPlate { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "شماره VIN")]
        public string VIN { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "شماره موتور")]
        public string EngineNumber { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "شماره شاسی")]
        public string ChassisNumber { get; set; }

        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "اسکن کارت خودرو (پشت و رو)")]
        //[DataType(DataType.Upload)]
        public string VehicleCard { get; set; }

        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "سریال ادونسر")]
        public string SerialSparkPreview { get; set; }

        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "سریال رگلاتور")]
        public string SerialKit { get; set; }

        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "سریال کلید انتخاب سوخت")]
        public string SerialKey { get; set; }

        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "سریال شیر سوخت گیری")]
        public string SerialRefuelingValve { get; set; }

        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "شماره بر چسب سوختگیری")]
        public string RefuelingLable { get; set; }

        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "کد رهگیری")]
        public string TrackingCode { get; set; }

        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "شماره گواهی پخش")]
        public string License { get; set; }

        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تصویر گواهی پخش")]
        public string LicenseImage { get; set; }

        [Display(Name = "نصب صورت گرفته در این خودرو ناقص می باشد")]
        public bool? InstallationStatus { get; set; }

        [Display(Name = "توضیحات نصب ناقص")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int? Creator { get; set; }
        public DateTime CreateDate { get; set; }

        [Display(Name = "شماره کارت سوخت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string FuelCard { get; set; }

        [Display(Name = "کارگاه مرتبط")]
        public int? WorkshopID { get; set; }
        [ForeignKey("WorkshopID")]
        public virtual Workshop Workshop { get; set; }

        public int? Editor { get; set; }
        public DateTime? EditDate { get; set; }

        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تصویر گواهی سلامت")]
        public string HealthCertificate { get; set; }

        [Display(Name = "Creator IP")]
        public string CreatorIPAddress { get; set; }

        [Display(Name = "Editor IP")]
        public string EditorIPAddress { get; set; }

        [Display(Name = "LockedEdit")]
        public bool Status { get; set; }

        [Display(Name = "شماره فاکتور")]
        public string InvoiceCode { get; set; }

        [Display(Name = "بررسی شد")]
        public bool Checked { get; set; }

        [Display(Name = "تاریخ بررسی")]
        public DateTime? CheckedDate { get; set; }

        [Display(Name = "بررسی کننده")]
        public string Checker { get; set; }

        [Display(Name = "تصویر معاینه فنی")]
        public string TechnicalDiagnosis { get; set; }

        [Display(Name = "وضعیت تبدیل")]
        public bool RegisterStatus { get; set; }

        [Display(Name = "تاریخ لغو تبدیل")]
        public DateTime? RegisterStatusDate { get; set; }

        [Display(Name = "کاربر لغو تبدیل")]
        public string RegisterStatusUser { get; set; }

        [Display(Name = "کد منحصر به تبدیل")]
        public string RegisterUniqueCode { get; set; }

        [Display(Name = "وضعیت تایید مالی")]
        public bool FinancialStatus { get; set; }

        [Display(Name = "تاریخ تایید مالی")]
        public DateTime? FinancialStatusDate { get; set; }

        [Display(Name = "کاربر تایید کننده")]
        public string FinancialStatusUser { get; set; }

        [Display(Name = "نوع طرح")]
        public int? RegistrationTypeID { get; set; }
        [ForeignKey("RegistrationTypeID")]
        public virtual RegistrationType RegistrationType { get; set; }
    }

    /// <summary>
    /// اطلاعات فیش های واریزی در فرم ثبت خودرو
    /// </summary>   
    [Table("tbl_VehicleInvoices")]
    public class VehicleInvoice
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "خودروی مرتبط")]
        public int? VehicleRegistrationID { get; set; }
        [ForeignKey("VehicleRegistrationID")]
        public virtual VehicleRegistration VehicleRegistration { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "شماره فیش")]
        public string Number { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "مبلغ فیش")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نوع واریزی")]
        public string Type { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تاریخ واریز")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تصویر فیش")]
        public string InvoiceFile { get; set; }

        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int? Creator { get; set; }
        public DateTime CreateDate { get; set; }

    }

    /// <summary>
    /// اطلاعات مخازن در فرم ثبت خودرو
    /// </summary>
    [Table("tbl_VehicleTanks")]
    public class VehicleTank
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "خودروی مرتبط")]
        public int? VehicleRegistrationID { get; set; }
        [ForeignKey("VehicleRegistrationID")]
        public virtual VehicleRegistration VehicleRegistration { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "سریال مخزن")]
        public string Serial { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "حجم مخزن")]
        public double Volume { get; set; }

        [Display(Name = "سازنده مخزن")]
        public int? TankConstractorID { get; set; }
        [ForeignKey("TankConstractorID")]
        public virtual TankConstractor TankConstractor { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تاریخ تولید")]
        public string ProductDate { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تاریخ انقضا")]
        public string ExpirationDate { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "سریال شیر مخزن")]
        public string SerialTankValve { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نوع شیر مخزن")]
        public string TypeTankValve { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "سازنده شیر مخزن")]
        public int? ValveConstractorID { get; set; }
        [ForeignKey("ValveConstractorID")]
        public virtual ValveConstractor ValveConstractor { get; set; }
        //public string ConstractorTankValve { get; set; }

        public int? Creator { get; set; }
        public DateTime CreateDate { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "سریال کیت (رگلاتور)")]
        public string RegulatorSerial { get; set; }

        [Display(Name = " سازنده کیت (رگلاتور)")]
        public int? RegulatorConstractorID { get; set; }
        [ForeignKey("RegulatorConstractorID")]
        public virtual RegulatorConstractor RegulatorConstractor { get; set; }

        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "سریال شیر قطع کن")]
        public string CutofValveSerial { get; set; }

        [Display(Name = " سازنده شیر قطع کن")]
        public int? CutofValveConstractorID { get; set; }
        [ForeignKey("CutofValveConstractorID")]
        public virtual CutofValveConstractor CutofValveConstractors { get; set; }

        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "سریال شیر پر کن")]
        public string FillingValveSerial { get; set; }

        [Display(Name = " سازنده شیر پر کن ")]
        public int? FillingValveConstractorID { get; set; }
        [ForeignKey("FillingValveConstractorID")]
        public virtual FillingValveConstractor FillingValveConstractors { get; set; }

        [Display(Name = "سریال ریل سوخت")]
        public string FuelRelaySerial { get; set; }

        [Display(Name = " سازنده ریل سوخت ")]
        public int? FuelRelayConstractorID { get; set; }
        [ForeignKey("FuelRelayConstractorID")]
        public virtual FuelRelayConstractor FuelRelayConstractors { get; set; }

        [Display(Name = "سریال Gas ECU")]
        public string GasECUSerial { get; set; }

        [Display(Name = " سازنده Gas ECU ")]
        public int? GasECUConstractorID { get; set; }
        [ForeignKey("GasECUConstractorID")]
        public virtual GasECUConstractor GasECUConstractors { get; set; }

        [Display(Name = "نسل کیت")]
        public int? GenarationID { get; set; }
        [ForeignKey("GenarationID")]
        public virtual GenerationofRegulator Genarations { get; set; }
        //public string Genaration { get; set; }

    }
    /// <summary>
    /// اطلاعات مخازن در فرم ثبت خودرو
    /// </summary>
    [Table("tbl_Insurances")]
    public class Insurance
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "خودروی مرتبط")]
        public int? VehicleRegistrationID { get; set; }
        [ForeignKey("VehicleRegistrationID")]
        public virtual VehicleRegistration VehicleRegistration { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "شماره بیمه نامه")]
        public string Number { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
    }

    /// <summary>
    /// ثبت تصاویر اسناد مانند کارت ملی، خودرو و ...
    /// </summary>   
    [Table("tbl_VehicleAttachments")]
    public class VehicleAttachment
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "خودروی مرتبط")]
        public int? VehicleRegistrationID { get; set; }
        [ForeignKey("VehicleRegistrationID")]
        public virtual VehicleRegistration VehicleRegistration { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "فایل پیوست")]
        public string Image { get; set; }

        [Display(Name = "محل نگهداری")]
        public string Folder { get; set; }

        public string Creator { get; set; }
        public DateTime CreateDate { get; set; }
    }

    /// <summary>
    /// تعیین نوع فعالیت طرح تبدیل یا طرح تعویض قطعات
    /// </summary>
    [Table("tbl_RegistrationTypes")]
    public class RegistrationType
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نوع طرح")]
        public string Type { get; set; }

        [Display(Name = "تصویر")]
        public string Image { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}

