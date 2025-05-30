using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cngfapco.Models
{
    public class Financial
    {
        public string Type { get; set; }
        public int Count { get; set; }

        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [DataType(DataType.Currency)]
        public double Salary { get; set; }
    }

    /// <summary>
    /// برای ثبت اطلاعات تجهیزات مورد مصرف در طرح تبدیل و BOM
    /// </summary>
    [Table("tbl_EquipmentList")]
    public class EquipmentList
    {
        public EquipmentList()
        {
            BOMs = new HashSet<BOM>();
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "عنوان کالا")]
        public string Title { get; set; }

        [Display(Name = "کد همکاران")]
        public string FinancialCode { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }
       
        //represnts Parent ID and it's nullable
        [Display(Name = "عنوان والد")]
        public int? Pid { get; set; }
        [ForeignKey("Pid")]
        public virtual EquipmentList Parent { get; set; }
        public virtual ICollection<EquipmentList> Childs { get; set; }

        //Cat Description
        [Display(Name = "نمایش در لیست")]
        public bool Presentable { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        public string Creator { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }

        public virtual ICollection<BOM> BOMs { get; set; }

        [Display(Name = "قیمت کارگاه")]
        public double? Value { get; set; }

        [Display(Name = " قیمت مصرف کننده")]
        public double? Value2 { get; set; }

        [Display(Name = "نوع طرح")]
        public int? RegistrationTypeID { get; set; }
        [ForeignKey("RegistrationTypeID")]
        public virtual RegistrationType RegistrationType { get; set; }

    }

    /// <summary>
    /// ثبت اطلاعات BOM
    /// </summary>
    [Table("tbl_BOMs")]
    public class BOM
    {
        public int ID { get; set; }

        [Display(Name = "ردیف مصرف")]
        public int? VehicleTypeID { get; set; }
        [ForeignKey("VehicleTypeID")]
        public virtual VehicleType VehicleType { get; set; }

        [Display(Name = "کالای مصرفی")]
        public int? EquipmentListID { get; set; }
        [ForeignKey("EquipmentListID")]
        public virtual EquipmentList EquipmentList { get; set; }

        [Display(Name = "ضریب مصرف")]
        public double? Ratio { get; set; }

        [Display(Name = "واحد مصرف")]
        public string Unit { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        //Cat Description
        [Display(Name = "نمایش در لیست")]
        public bool Presentable { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        public string Creator { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "نسل کیت")]
        public int? GenerationID { get; set; }
        [ForeignKey("GenerationID")]
        public virtual GenerationofRegulator Generations { get; set; }

    }
    /// <summary>
    /// ثبت اطلاعات نسل کیت
    /// </summary>
    [Table("tbl_GenerationofRegulators")]
    public class GenerationofRegulator
    {
        public int ID { get; set; }

        [Display(Name = "نسل کیت")]
        public string Title { get; set; }

        [Display(Name = "نسل کیت")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
    /// <summary>
    /// ثبت اطلاعات موجودی انبار
    /// </summary>
    [Table("tbl_Warehouses")]
    public class Warehouse
    {
        public int ID { get; set; }

        [Display(Name = "تاریخ")]
        public DateTime Date { get; set; }

        [Display(Name = "عنوان انبار")]
        public string Name { get; set; }

        [Display(Name = "کد همکاران")]
        public string FinancialCode { get; set; }

        [Display(Name = "عنوان کالا")]
        public string Title { get; set; }

        [Display(Name = "واحد سنجش")]
        public string Units { get; set; }

        [Display(Name = "موجودی")]
        public double? Rem { get; set; }

        [Display(Name = "آخرین موجودی")]
        public double? CurrentRem { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        public string Creator { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }

    }
    ///
    /// <summary>
    /// جهت ثبت اطلاعات صورتحساب فروش کالا و خدمات غیر مصوب کارگاه ها به مشتریان
    /// </summary>
    [Table("tbl_Invoices")]
    public partial class Invoice
    {
        [Key]
        public int InvoiceID { get; set; }

        [Display(Name = "خریدار")]
        public int? OwnersID { get; set; }
        [ForeignKey("OwnersID")]
        public virtual VehicleRegistration Owners { get; set; }

        [Display(Name = "عنوان کارگاه")]
        public int? WorkshopsID { get; set; }
        [ForeignKey("WorkshopsID")]
        public virtual Workshop Workshops { get; set; }

        [Display(Name = "اقلام مصرفی")]
        public int? EquipmentsID { get; set; }
        [ForeignKey("EquipmentsID")]
        public virtual EquipmentList Equipments { get; set; }

        [Display(Name = "کد کالا / خدمات")]
        public string ServiceCode { get; set; }

        [Display(Name = "شماره سریال")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string InvoiceCode { get; set; }

        [Display(Name = "تاریخ")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "شماره اقتصادی")]
        public string EmployerEconomicalnumber { get; set; }

        [Display(Name = "شماره ثبت / شماره ملی")]
        public string Employerregistrationnumber { get; set; }

        [Display(Name = "استان")]
        public string EmployerState { get; set; }

        [Display(Name = "آدرس")]
        public string EmployerAddress { get; set; }

        [Display(Name = "کد پستی")]
        public string EmployerPostalcode { get; set; }

        [Display(Name = "تلفن")]
        public string EmployerPhone { get; set; }

        [Display(Name = "نمـابر")]
        public string EmployerFax { get; set; }

        [Display(Name = "شرح کالا / خدمات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.MultilineText)]
        public string ServiceDesc { get; set; }

        [Display(Name = "تعداد / مقدار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Number { get; set; }

        [Display(Name = "واحد اندازه گیری")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string UnitofMeasurement { get; set; }

        [Display(Name = "مبلغ واحد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Currency)]
        public double UnitAmount { get; set; }

        [Display(Name = "مبلغ کل")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Currency)]
        public double? TotalAmount { get; set; }

        [Display(Name = "مبلغ تخفیف")]
        [DataType(DataType.Currency)]
        public double? DiscountAmount { get; set; }

        [Display(Name = "مبلغ کل پس از تخفیف")]
        [DataType(DataType.Currency)]
        public double? TotalAmountafterDiscount { get; set; }

        [Display(Name = "مالیات")]
        [DataType(DataType.Currency)]
        public double? Tax { get; set; }

        [Display(Name = "عوارض")]
        [DataType(DataType.Currency)]
        public double? Complications { get; set; }

        [Display(Name = "جمع مالیات و عوارض")]
        [DataType(DataType.Currency)]
        public double? AmountTaxandComplications { get; set; }

        [Display(Name = "جمع مبلغ کل و مالیات و عوارض")]
        [DataType(DataType.Currency)]
        public double? TotalAmountTaxandComplications { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "شرایط و نحوه فروش")]
        public string SaleCondition { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        [Display(Name = "مبلغ تایید شده")]
        [DataType(DataType.Currency)]
        public double? AcceptedAmount { get; set; }

        [Display(Name = "تاریخ تایید شده")]
        public DateTime? AcceptedDate { get; set; }

        [Display(Name = "وضعیت")]
        public bool? Status { get; set; }

        [Display(Name = "نوع  ارز")]
        public int? CurrencyTypeID { get; set; }
        [ForeignKey("CurrencyTypeID")]
        public virtual CurrencyType CurrencyType { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        public int? CreatorUser { get; set; }

        [Display(Name = "نوع طرح")]
        public int? RegistrationTypeID { get; set; }
        [ForeignKey("RegistrationTypeID")]
        public virtual RegistrationType RegistrationType { get; set; }

    }
    //
    /// <summary>
    /// جهت ثبت اطلاعات صورتحساب دستمزد تبدیل کارگاه ها 
    /// </summary>
    [Table("tbl_InvoicesFapa")]
    public partial class InvoiceFapa
    {
        [Key]
        public int InvoiceID { get; set; }

        [Display(Name = "نوع خودرو")]
        public int? VehicleTypesID { get; set; }
        [ForeignKey("VehicleTypesID")]
        public virtual VehicleType VehicleTypes { get; set; }

        [Display(Name = "عنوان کارگاه")]
        public int? WorkshopsID { get; set; }
        [ForeignKey("WorkshopsID")]
        public virtual Workshop Workshops { get; set; }

        [Display(Name = "اقلام مصرفی")]
        public int? EquipmentsID { get; set; }
        [ForeignKey("EquipmentsID")]
        public virtual EquipmentList Equipments { get; set; }

        [Display(Name = "کد کالا / خدمات")]
        public string ServiceCode { get; set; }

        [Display(Name = "شماره سریال")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string InvoiceCode { get; set; }

        [Display(Name = "تاریخ")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "شماره اقتصادی")]
        public string EmployerEconomicalnumber { get; set; }

        [Display(Name = "شماره ثبت / شماره ملی")]
        public string Employerregistrationnumber { get; set; }

        [Display(Name = "استان")]
        public string EmployerState { get; set; }

        [Display(Name = "آدرس")]
        public string EmployerAddress { get; set; }

        [Display(Name = "کد پستی")]
        public string EmployerPostalcode { get; set; }

        [Display(Name = "تلفن")]
        public string EmployerPhone { get; set; }

        [Display(Name = "نمـابر")]
        public string EmployerFax { get; set; }

        [Display(Name = "شرح کالا / خدمات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.MultilineText)]
        public string ServiceDesc { get; set; }

        [Display(Name = "تعداد / مقدار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Number { get; set; }

        [Display(Name = "واحد اندازه گیری")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string UnitofMeasurement { get; set; }

        [Display(Name = "مبلغ واحد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Currency)]
        public double UnitAmount { get; set; }

        [Display(Name = "مبلغ کل")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Currency)]
        public double? TotalAmount { get; set; }

        [Display(Name = "مبلغ تخفیف")]
        [DataType(DataType.Currency)]
        public double? DiscountAmount { get; set; }

        [Display(Name = "مبلغ کل پس از تخفیف")]
        [DataType(DataType.Currency)]
        public double? TotalAmountafterDiscount { get; set; }

        [Display(Name = "مالیات")]
        [DataType(DataType.Currency)]
        public double? Tax { get; set; }

        [Display(Name = "عوارض")]
        [DataType(DataType.Currency)]
        public double? Complications { get; set; }

        [Display(Name = "جمع مالیات و عوارض")]
        [DataType(DataType.Currency)]
        public double? AmountTaxandComplications { get; set; }

        [Display(Name = "جمع مبلغ کل و مالیات و عوارض")]
        [DataType(DataType.Currency)]
        public double? TotalAmountTaxandComplications { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "شرایط و نحوه فروش")]
        public string SaleCondition { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        [Display(Name = "مبلغ تایید شده")]
        [DataType(DataType.Currency)]
        public double? AcceptedAmount { get; set; }

        [Display(Name = "تاریخ تایید شده")]
        public DateTime? AcceptedDate { get; set; }

        [Display(Name = "وضعیت")]
        public bool? Status { get; set; }

        [Display(Name = "نوع  ارز")]
        public int? CurrencyTypeID { get; set; }
        [ForeignKey("CurrencyTypeID")]
        public virtual CurrencyType CurrencyType { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        public int? CreatorUser { get; set; }

        [Display(Name = "کنترل کارشناسی")]
        public bool? CheckStatus { get; set; }

        [Display(Name = "تاریخ کنترل")]
        public DateTime? CkeckedDate { get; set; }

        [Display(Name = "کارشناس")]
        public string CkeckedUser { get; set; }

        [Display(Name = "کنترل مالی")]
        public bool? FinancialStatus { get; set; }

        [Display(Name = "تاریخ کنترل")]
        public DateTime? FinancialDate { get; set; }

        [Display(Name = "کارشناس")]
        public string FinancialUser { get; set; }

        [Display(Name = "تایید دریافت")]
        public bool? ReciveStatus { get; set; }

        [Display(Name = "تاریخ دریافت")]
        public DateTime? ReciveDate { get; set; }

        [Display(Name = "دریافت کننده")]
        public string ReciveUser { get; set; }
    }

    /// <summary>
    /// ثبت اطلاعات نوع مبالغ
    /// </summary>

    [Table("tbl_CurrencyTypes")]
    public class CurrencyType
    {
        public int ID { get; set; }

        [Display(Name = "واحد ارز")]
        public string Title { get; set; }
    }

    /// <summary>
    /// شرح سرفصل های منابع و هزینه 
    /// </summary>
    [Table("tbl_FinancialDescs")]
    public class FinancialDesc
    {
        public int ID { get; set; }

        [Display(Name = "عنوان سرفصل")]
        public string Title { get; set; }

    }

    /// <summary>
    /// ثبت اطلاعات پرداخت های مالی به کارگاه ها 
    /// </summary>
    [Table("tbl_FinancialPayments")]
    public class FinancialPayment
    {
        public int ID { get; set; }

        [Display(Name = "عنوان سرفصل")]
        public int? FinancialDescID { get; set; }
        [ForeignKey("FinancialDescID")]
        public virtual FinancialDesc FinancialDesc { get; set; }

        [Display(Name = "کارگاه مرتبط")]
        public int? WorkshopID { get; set; }
        [ForeignKey("WorkshopID")]
        public virtual Workshop Workshops { get; set; }

        [Display(Name = "تاریخ پرداخت")]
        public DateTime Date { get; set; }

        [Display(Name = "مبلغ پرداخت شده")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public double Value { get; set; }

        [Display(Name = "شرح پرداخت")]
        [DataType(DataType.MultilineText)]        
        public string Description { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "ایجاد کننده")]
        public string Creator { get; set; }

        [Display(Name = "وضعیت مرتبط با مالی")]
        public bool? Status { get; set; }

        [Display(Name = "شرح مرتبط با اسناد مالی")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
    }
    //
    /// <summary>
    /// ثبت اطلاعات موجودی بانک
    /// </summary>

    [Table("tbl_BankAccounts")]
    public class BankAccount
    {
        public int ID { get; set; }

        [Display(Name = "تاریخ")]
        public DateTime Date { get; set; }

        [Display(Name = "زمان")]
        public DateTime Times { get; set; }

        [Display(Name = "کد ملی")]
        public string NationalCode { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }

        [Display(Name = "کد رهگیری")]
        public string TrackingCode { get; set; }

        [Display(Name = "کارگاه")]
        public string Workshop { get; set; }        

        [Display(Name = "بانک")]
        public string Bank { get; set; }

        [Display(Name = "شعبه")]
        public string Branch { get; set; }

        [Display(Name = "کد حسابگری")]
        public string AccounterCode { get; set; }

        [Display(Name = "شناسه واریز کننده")]
        public string DepositorID { get; set; }

        [Display(Name = "سریال")]
        public string Serial { get; set; }

        [Display(Name = "واریز کننده")]
        public string Depositor { get; set; }

        [Display(Name = "شرح")]
        public string Description { get; set; }

        [Display(Name = "مبلغ")]
        public double Amount { get; set; }

        [Display(Name = "مانده")]
        public double Rem { get; set; }
    }

    /// <summary>
    /// جدول تغییرات دستمزد تبدیل کارگاه ها 
    /// </summary>
    [Table("tbl_RegistrationPrice")]
    public class RegistrationPrice
    {
        public int ID { get; set; }

        [Display(Name = "نوع خودرو(سواری/ وانت)")]
        public string Type { get; set; }

        [Display(Name = "مبلغ جدید (ریال)")]
        public double Price { get; set; }

        [Display(Name = "از تاریخ")]
        public DateTime FromDate { get; set; }

        [Display(Name = "تا تاریخ")]
        public DateTime ToDate { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "مبلغ قبلی (ریال)")]
        public double? OldPrice { get; set; }

        [Display(Name = "نوع کارگاه")]
        public string DepType { get; set; }

    }

    /// <summary>
    /// جدول ثبت اطلاعات درگاه پرداخت آنلاین بانک ملت 
    /// </summary>
    [Table("tbl_Payments")]
    public class Payment
    {
        public int ID { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }

        [Display(Name = "کد پرداخت کننده")]
        public int? PayerCode { get; set; }

        [Display(Name = "کد ملی")]
        public string NationalCode { get; set; }

        [Display(Name = "شماره موبایل")]
        public string MobileNumber { get; set; }

        [Display(Name = "پست الکترونیک")]
        public string EMailAddress { get; set; }

        [Display(Name = "مبلغ (ریال)")]
        public double Amount { get; set; }

        [Display(Name = "تاریخ پرداخت")]
        public DateTime PayDate { get; set; }

        [Display(Name = "Payer IP Address")]
        public string PayerIPAddress { get; set; }

        [Display(Name = "وضعیت پرداخت")]
        public string Status { get; set; }

        [Display(Name = "شماره درخواست")]
        public string OrderID { get; set; }

        [Display(Name = "شماره ارجاع")]
        public string RefID { get; set; }

        [Display(Name = "شماره پیگیری")]
        public string SaleReferenceId { get; set; }

        [Display(Name = "شماره لیست درخواست")]
        public string RequestInvoiceCode { get; set; }

        [Display(Name = "شماره پیش فاکتور")]
        public string PreInvoiceCode { get; set; }

        [Display(Name = "شماره فاکتور ")]
        public string InvoiceCode { get; set; }

        [Display(Name = "روش پرداخت")]
        public string PaymentMethod { get; set; }

    }
    ///
    /// <summary>
    /// جهت ثبت درخواست فروش کالا در فروش آزاد قطعات توسط کارگاه ها به مشتریان
    /// </summary>
    [Table("tbl_RequestFreeSales")]
    public partial class RequestFreeSale
    {
        [Key]
        public int InvoiceID { get; set; }

        [Display(Name = "خریدار")]
        public string Owners { get; set; }

        [Display(Name = "عنوان کارگاه")]
        public int? WorkshopsID { get; set; }
        [ForeignKey("WorkshopsID")]
        public virtual Workshop Workshops { get; set; }

        [Display(Name = "اقلام مصرفی")]
        public int? EquipmentsID { get; set; }
        [ForeignKey("EquipmentsID")]
        public virtual EquipmentList Equipments { get; set; }

        [Display(Name = "کد کالا / خدمات")]
        public string ServiceCode { get; set; }

        [Display(Name = "شماره درخواست")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string InvoiceCode { get; set; }

        [Display(Name = "تاریخ")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "شماره اقتصادی")]
        public string EmployerEconomicalnumber { get; set; }

        [Display(Name = "شماره ثبت / شماره ملی")]
        public string Employerregistrationnumber { get; set; }

        [Display(Name = "استان")]
        public string EmployerState { get; set; }

        [Display(Name = "آدرس")]
        public string EmployerAddress { get; set; }

        [Display(Name = "کد پستی")]
        public string EmployerPostalcode { get; set; }

        [Display(Name = "تلفن")]
        public string EmployerPhone { get; set; }

        [Display(Name = "نمـابر")]
        public string EmployerFax { get; set; }

        [Display(Name = "شرح کالا / خدمات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.MultilineText)]
        public string ServiceDesc { get; set; }

        [Display(Name = "تعداد / مقدار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Number { get; set; }

        [Display(Name = "واحد اندازه گیری")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string UnitofMeasurement { get; set; }

        [Display(Name = "مبلغ واحد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Currency)]
        public double UnitAmount { get; set; }

        [Display(Name = "مبلغ کل")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Currency)]
        public double? TotalAmount { get; set; }

        [Display(Name = "مبلغ تخفیف")]
        [DataType(DataType.Currency)]
        public double? DiscountAmount { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "شرایط و نحوه فروش")]
        public string SaleCondition { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        [Display(Name = "وضعیت")]
        public bool Status { get; set; }

        [Display(Name = "نوع  ارز")]
        public int? CurrencyTypeID { get; set; }
        [ForeignKey("CurrencyTypeID")]
        public virtual CurrencyType CurrencyType { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        public int? CreatorUser { get; set; }

        [Display(Name = "وضعیت مشاهده")]
        public bool ViewStatus { get; set; }

        [Display(Name = "تاریخ مشاهده")]
        public DateTime? ViewDate { get; set; }

        [Display(Name = "مشاهده کننده")]
        public string Viewer { get; set; }

        [Display(Name = "وضعیت نهایی")]
        public bool FinalStatus { get; set; }

        [Display(Name = "نوع طرح")]
        public int? RegistrationTypeID { get; set; }
        [ForeignKey("RegistrationTypeID")]
        public virtual RegistrationType RegistrationType { get; set; }
    }
    ///
    /// <summary>
    /// جهت ثبت اطلاعات پیش فاکتور فروش کالا و خدمات فروش آزاد قطعات توسط کارگاه ها به مشتریان
    /// </summary>
    [Table("tbl_FreeSaleInvoices")]
    public partial class FreeSaleInvoice
    {
        [Key]
        public int InvoiceID { get; set; }

        [Display(Name = "خریدار")]
        public int? CustomersID { get; set; }
        [ForeignKey("CustomersID")]
        public virtual Customer Customers { get; set; }

        [Display(Name = "عنوان کارگاه")]
        public int? WorkshopsID { get; set; }
        [ForeignKey("WorkshopsID")]
        public virtual Workshop Workshops { get; set; }

        [Display(Name = "اقلام مصرفی")]
        public int? EquipmentsID { get; set; }
        [ForeignKey("EquipmentsID")]
        public virtual EquipmentList Equipments { get; set; }

        [Display(Name = "کد کالا / خدمات")]
        public string ServiceCode { get; set; }

        [Display(Name = "شماره پیش فاکتور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string InvoiceCode { get; set; }

        [Display(Name = "تاریخ")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "شماره اقتصادی")]
        public string EmployerEconomicalnumber { get; set; }

        [Display(Name = "شماره ثبت / شماره ملی")]
        public string Employerregistrationnumber { get; set; }

        [Display(Name = "استان")]
        public string EmployerState { get; set; }

        [Display(Name = "آدرس")]
        public string EmployerAddress { get; set; }

        [Display(Name = "کد پستی")]
        public string EmployerPostalcode { get; set; }

        [Display(Name = "تلفن")]
        public string EmployerPhone { get; set; }

        [Display(Name = "نمـابر")]
        public string EmployerFax { get; set; }

        [Display(Name = "شرح کالا / خدمات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.MultilineText)]
        public string ServiceDesc { get; set; }

        [Display(Name = "تعداد / مقدار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Number { get; set; }

        [Display(Name = "واحد اندازه گیری")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string UnitofMeasurement { get; set; }

        [Display(Name = "مبلغ واحد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Currency)]
        public double UnitAmount { get; set; }

        [Display(Name = "مبلغ کل")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Currency)]
        public double? TotalAmount { get; set; }

        [Display(Name = "مبلغ تخفیف")]
        [DataType(DataType.Currency)]
        public double? DiscountAmount { get; set; }

        [Display(Name = "مبلغ کل پس از تخفیف")]
        [DataType(DataType.Currency)]
        public double? TotalAmountafterDiscount { get; set; }

        [Display(Name = "مالیات")]
        [DataType(DataType.Currency)]
        public double? Tax { get; set; }

        [Display(Name = "عوارض")]
        [DataType(DataType.Currency)]
        public double? Complications { get; set; }

        [Display(Name = "جمع مالیات و عوارض")]
        [DataType(DataType.Currency)]
        public double? AmountTaxandComplications { get; set; }

        [Display(Name = "جمع مبلغ کل و مالیات و عوارض")]
        [DataType(DataType.Currency)]
        public double? TotalAmountTaxandComplications { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "شرایط و نحوه فروش")]
        public string SaleCondition { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        [Display(Name = "مبلغ تایید شده")]
        [DataType(DataType.Currency)]
        public double? AcceptedAmount { get; set; }

        [Display(Name = "تاریخ تایید شده")]
        public DateTime? AcceptedDate { get; set; }

        [Display(Name = "وضعیت")]
        public bool Status { get; set; }

        [Display(Name = "نوع  ارز")]
        public int? CurrencyTypeID { get; set; }
        [ForeignKey("CurrencyTypeID")]
        public virtual CurrencyType CurrencyType { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        public int? CreatorUser { get; set; }

        [Display(Name = "وضعیت مشاهده")]
        public bool ViewStatus { get; set; }

        [Display(Name = "تاریخ مشاهده")]
        public DateTime? ViewDate { get; set; }

        [Display(Name = "مشاهده کننده")]
        public string Viewer { get; set; }

        [Display(Name = "شماره درخواست")]
        public string RequestInvoiceCode { get; set; }

    }
    ///
    /// <summary>
    /// جهت ثبت اطلاعات فاکتور نهایی فروش کالا و خدمات فروش آزاد قطعات توسط کارگاه ها به مشتریان
    /// </summary>
    [Table("tbl_FinallFreeSaleInvoices")]
    public partial class FinallFreeSaleInvoice
    {
        [Key]
        public int InvoiceID { get; set; }

        [Display(Name = "خریدار")]
        public int? CustomersID { get; set; }
        [ForeignKey("CustomersID")]
        public virtual Customer Customers { get; set; }

        [Display(Name = "عنوان کارگاه")]
        public int? WorkshopsID { get; set; }
        [ForeignKey("WorkshopsID")]
        public virtual Workshop Workshops { get; set; }

        [Display(Name = "اقلام مصرفی")]
        public int? EquipmentsID { get; set; }
        [ForeignKey("EquipmentsID")]
        public virtual EquipmentList Equipments { get; set; }

        [Display(Name = "کد کالا / خدمات")]
        public string ServiceCode { get; set; }

        [Display(Name = "شماره فاکتور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string InvoiceCode { get; set; }

        [Display(Name = "تاریخ")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "شماره اقتصادی")]
        public string EmployerEconomicalnumber { get; set; }

        [Display(Name = "شماره ثبت / شماره ملی")]
        public string Employerregistrationnumber { get; set; }

        [Display(Name = "استان")]
        public string EmployerState { get; set; }

        [Display(Name = "آدرس")]
        public string EmployerAddress { get; set; }

        [Display(Name = "کد پستی")]
        public string EmployerPostalcode { get; set; }

        [Display(Name = "تلفن")]
        public string EmployerPhone { get; set; }

        [Display(Name = "نمـابر")]
        public string EmployerFax { get; set; }

        [Display(Name = "شرح کالا / خدمات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.MultilineText)]
        public string ServiceDesc { get; set; }

        [Display(Name = "تعداد / مقدار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Number { get; set; }

        [Display(Name = "واحد اندازه گیری")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string UnitofMeasurement { get; set; }

        [Display(Name = "مبلغ واحد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Currency)]
        public double UnitAmount { get; set; }

        [Display(Name = "مبلغ کل")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Currency)]
        public double? TotalAmount { get; set; }

        [Display(Name = "مبلغ تخفیف")]
        [DataType(DataType.Currency)]
        public double? DiscountAmount { get; set; }

        [Display(Name = "مبلغ کل پس از تخفیف")]
        [DataType(DataType.Currency)]
        public double? TotalAmountafterDiscount { get; set; }

        [Display(Name = "مالیات")]
        [DataType(DataType.Currency)]
        public double? Tax { get; set; }

        [Display(Name = "عوارض")]
        [DataType(DataType.Currency)]
        public double? Complications { get; set; }

        [Display(Name = "جمع مالیات و عوارض")]
        [DataType(DataType.Currency)]
        public double? AmountTaxandComplications { get; set; }

        [Display(Name = "جمع مبلغ کل و مالیات و عوارض")]
        [DataType(DataType.Currency)]
        public double? TotalAmountTaxandComplications { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "شرایط و نحوه فروش")]
        public string SaleCondition { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        [Display(Name = "مبلغ تایید شده")]
        [DataType(DataType.Currency)]
        public double? AcceptedAmount { get; set; }

        [Display(Name = "تاریخ تایید شده")]
        public DateTime? AcceptedDate { get; set; }

        [Display(Name = "وضعیت")]
        public bool Status { get; set; }

        [Display(Name = "نوع  ارز")]
        public int? CurrencyTypeID { get; set; }
        [ForeignKey("CurrencyTypeID")]
        public virtual CurrencyType CurrencyType { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        public int? CreatorUser { get; set; }

        [Display(Name = "وضعیت مشاهده")]
        public bool ViewStatus { get; set; }

        [Display(Name = "تاریخ مشاهده")]
        public DateTime? ViewDate { get; set; }

        [Display(Name = "مشاهده کننده")]
        public string Viewer { get; set; }

        [Display(Name = "شماره پیش فاکتور")]
        public string PreInvoiceCode { get; set; }

    }
    /// <summary>
    /// ثبت اطلاعات موجودی انبار فروش
    /// </summary>
    [Table("tbl_SaleWarehouses")]
    public class SaleWarehouse
    {
        public int ID { get; set; }

        [Display(Name = "تاریخ")]
        public DateTime Date { get; set; }

        [Display(Name = "عنوان انبار")]
        public string Name { get; set; }

        [Display(Name = "کد همکاران")]
        public string FinancialCode { get; set; }

        [Display(Name = "عنوان کالا")]
        public string Title { get; set; }

        [Display(Name = "واحد سنجش")]
        public string Units { get; set; }

        [Display(Name = "موجودی")]
        public double? Rem { get; set; }

        [Display(Name = "آخرین موجودی")]
        public double? CurrentRem { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        public string Creator { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }

    }

    /// <summary>
    /// ثبت اطلاعات دستور پرداخت علی الحساب به کارگاه ها 
    /// </summary>
    [Table("tbl_OfferedPrices")]
    public class OfferedPrice
    {
        public int ID { get; set; }

        [Display(Name = "شماره درخواست")]
        public int Number { get; set; }

        [Display(Name = "شماره درخواست")]
        public string Serial { get; set; }

        [Display(Name = "کارگاه مرتبط")]
        public int? WorkshopID { get; set; }
        [ForeignKey("WorkshopID")]
        public virtual Workshop Workshops { get; set; }

        [Display(Name = "تاریخ درخواست")]
        public DateTime Date { get; set; }

        [Display(Name = "مبلغ پیشنهادی")]
        [DataType(DataType.Currency)]
        public double Value { get; set; }

        [Display(Name = "شرح پرداخت")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "ایجاد کننده")]
        public string Creator { get; set; }

        [Display(Name = "وضعیت پرداخت")]
        public bool StatusPay { get; set; }

        [Display(Name = "تاریخ پرداخت")]
        public DateTime? PayDate { get; set; }

        [Display(Name = "مبلغ پرداخت شده")]
        [DataType(DataType.Currency)]
        public double PaiedValue { get; set; }
    }
    /// <summary>
    /// جدول تغییرات هزینه بازرسی کارگاه ها 
    /// </summary>
    [Table("tbl_AuditsPrice")]
    public class AuditsPrice
    {
        public int ID { get; set; }

        [Display(Name = "شرکت بازرسی")]
        public int? AuditCompanyID { get; set; }
        [ForeignKey("AuditCompanyID")]
        public virtual AuditCompany AuditCompanies { get; set; }

        [Display(Name = "مبلغ جدید (ریال)")]
        public double Price { get; set; }

        [Display(Name = "از تاریخ")]
        public DateTime FromDate { get; set; }

        [Display(Name = "تا تاریخ")]
        public DateTime ToDate { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "مبلغ قبلی (ریال)")]
        public double? OldPrice { get; set; }

        [Display(Name = "نوع")]
        public string Type { get; set; }

    }

    /// <summary>
    /// برای اعداد مالیات و ارزش افزوده سالهای مختلف
    /// </summary>
    [Table("tbl_TaxValueAdded")]
    public class TaxValueAdded
    {
        public int ID { get; set; }
        public int Year { get; set; }

        [DataType(DataType.Currency)]
        public double? ValueAdded { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
    /// <summary>
    /// جدول تغییرات قیمت ماهیانه مخازن ضایعاتی طرح تعویض 
    /// </summary>
    [Table("tbl_ReplacementPlanPrice")]
    public class ReplacementPlanPrice
    {
        public int ID { get; set; }

        [Display(Name = "نوع مخزن")]
        public int? EquipmentTypeID { get; set; }
        [ForeignKey("EquipmentTypeID")]
        public virtual EquipmentList EquipmentType { get; set; }

        [Display(Name = "قیمت (ریال)")]
        public double Price { get; set; }

        [Display(Name = "قیمت با تخفیف (ریال)")]
        public double DiscountedPrice { get; set; }

        [Display(Name = "از تاریخ")]
        public DateTime FromDate { get; set; }

        [Display(Name = "تا تاریخ")]
        public DateTime ToDate { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

    }

    /// <summary>
    /// جدول تغییرات قیمت ماهیانه شیر مخزنهای ضایعاتی طرح تعویض 
    /// </summary>
    [Table("tbl_ReplacementPlanValvePrice")]
    public class ReplacementPlanValvePrice
    {
        public int ID { get; set; }

        [Display(Name = "نوع مخزن")]
        public int? EquipmentTypeID { get; set; }
        [ForeignKey("EquipmentTypeID")]
        public virtual EquipmentList EquipmentType { get; set; }

        [Display(Name = "قیمت (ریال)")]
        public double Price { get; set; }

        [Display(Name = "قیمت با تخفیف (ریال)")]
        public double DiscountedPrice { get; set; }

        [Display(Name = "از تاریخ")]
        public DateTime FromDate { get; set; }

        [Display(Name = "تا تاریخ")]
        public DateTime ToDate { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

    }
    /// <summary>
    /// جهت ثبت اطلاعات صورتحساب فروش قطعات ضایعاتی در طرح تعویض به کارگاه ها 
    /// </summary>
    [Table("tbl_InvoicesDamages")]
    public partial class InvoicesDamages
    {
        [Key]
        public int InvoiceID { get; set; }

        [Display(Name = "نوع خودرو")]
        public int? VehicleTypesID { get; set; }
        [ForeignKey("VehicleTypesID")]
        public virtual VehicleType VehicleTypes { get; set; }

        [Display(Name = "عنوان کارگاه")]
        public int? WorkshopsID { get; set; }
        [ForeignKey("WorkshopsID")]
        public virtual Workshop Workshops { get; set; }

        [Display(Name = "اقلام مصرفی")]
        public int? EquipmentsID { get; set; }
        [ForeignKey("EquipmentsID")]
        public virtual EquipmentList Equipments { get; set; }

        [Display(Name = "کد کالا / خدمات")]
        public string ServiceCode { get; set; }

        [Display(Name = "شماره سریال")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string InvoiceCode { get; set; }

        [Display(Name = "تاریخ")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "شماره اقتصادی")]
        public string EmployerEconomicalnumber { get; set; }

        [Display(Name = "شماره ثبت / شماره ملی")]
        public string Employerregistrationnumber { get; set; }

        [Display(Name = "استان")]
        public string EmployerState { get; set; }

        [Display(Name = "آدرس")]
        public string EmployerAddress { get; set; }

        [Display(Name = "کد پستی")]
        public string EmployerPostalcode { get; set; }

        [Display(Name = "تلفن")]
        public string EmployerPhone { get; set; }

        [Display(Name = "نمـابر")]
        public string EmployerFax { get; set; }

        [Display(Name = "شرح کالا / خدمات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.MultilineText)]
        public string ServiceDesc { get; set; }

        [Display(Name = "تعداد / مقدار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Number { get; set; }

        [Display(Name = "واحد اندازه گیری")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string UnitofMeasurement { get; set; }

        [Display(Name = "مبلغ واحد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Currency)]
        public double UnitAmount { get; set; }

        [Display(Name = "مبلغ کل")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Currency)]
        public double? TotalAmount { get; set; }

        [Display(Name = "مبلغ تخفیف")]
        [DataType(DataType.Currency)]
        public double? DiscountAmount { get; set; }

        [Display(Name = "مبلغ کل پس از تخفیف")]
        [DataType(DataType.Currency)]
        public double? TotalAmountafterDiscount { get; set; }

        [Display(Name = "مالیات")]
        [DataType(DataType.Currency)]
        public double? Tax { get; set; }

        [Display(Name = "عوارض")]
        [DataType(DataType.Currency)]
        public double? Complications { get; set; }

        [Display(Name = "جمع مالیات و عوارض")]
        [DataType(DataType.Currency)]
        public double? AmountTaxandComplications { get; set; }

        [Display(Name = "جمع مبلغ کل و مالیات و عوارض")]
        [DataType(DataType.Currency)]
        public double? TotalAmountTaxandComplications { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "شرایط و نحوه فروش")]
        public string SaleCondition { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        [Display(Name = "مبلغ تایید شده")]
        [DataType(DataType.Currency)]
        public double? AcceptedAmount { get; set; }

        [Display(Name = "تاریخ تایید شده")]
        public DateTime? AcceptedDate { get; set; }

        [Display(Name = "وضعیت")]
        public bool? Status { get; set; }

        [Display(Name = "نوع  ارز")]
        public int? CurrencyTypeID { get; set; }
        [ForeignKey("CurrencyTypeID")]
        public virtual CurrencyType CurrencyType { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        public int? CreatorUser { get; set; }

        [Display(Name = "کنترل کارشناسی")]
        public bool? CheckStatus { get; set; }

        [Display(Name = "تاریخ کنترل")]
        public DateTime? CkeckedDate { get; set; }

        [Display(Name = "کارشناس")]
        public string CkeckedUser { get; set; }

        [Display(Name = "کنترل مالی")]
        public bool? FinancialStatus { get; set; }

        [Display(Name = "تاریخ کنترل")]
        public DateTime? FinancialDate { get; set; }

        [Display(Name = "کارشناس")]
        public string FinancialUser { get; set; }

        [Display(Name = "تایید دریافت")]
        public bool? ReciveStatus { get; set; }

        [Display(Name = "تاریخ دریافت")]
        public DateTime? ReciveDate { get; set; }

        [Display(Name = "دریافت کننده")]
        public string ReciveUser { get; set; }

        [Display(Name = "سال تبدیل")]
        public string Year { get; set; }

        [Display(Name = "ماه تبدیل")]
        public string Month { get; set; }
    }

    /// <summary>
    /// جهت ثبت اطلاعات صورتحساب دستمزد طرح تعویض مخازن تعویضی کارگاه ها 
    /// </summary>
    [Table("tbl_InvoicesFapa_DamagesCylinder")]
    public partial class InvoicesFapa_DamagesCylinder
    {
        [Key]
        public int InvoiceID { get; set; }

        [Display(Name = "نوع خودرو")]
        public int? VehicleTypesID { get; set; }
        [ForeignKey("VehicleTypesID")]
        public virtual VehicleType VehicleTypes { get; set; }

        [Display(Name = "عنوان کارگاه")]
        public int? WorkshopsID { get; set; }
        [ForeignKey("WorkshopsID")]
        public virtual Workshop Workshops { get; set; }

        [Display(Name = "اقلام مصرفی")]
        public int? EquipmentsID { get; set; }
        [ForeignKey("EquipmentsID")]
        public virtual EquipmentList Equipments { get; set; }

        [Display(Name = "کد کالا / خدمات")]
        public string ServiceCode { get; set; }

        [Display(Name = "شماره سریال")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string InvoiceCode { get; set; }

        [Display(Name = "تاریخ")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "شماره اقتصادی")]
        public string EmployerEconomicalnumber { get; set; }

        [Display(Name = "شماره ثبت / شماره ملی")]
        public string Employerregistrationnumber { get; set; }

        [Display(Name = "استان")]
        public string EmployerState { get; set; }

        [Display(Name = "آدرس")]
        public string EmployerAddress { get; set; }

        [Display(Name = "کد پستی")]
        public string EmployerPostalcode { get; set; }

        [Display(Name = "تلفن")]
        public string EmployerPhone { get; set; }

        [Display(Name = "نمـابر")]
        public string EmployerFax { get; set; }

        [Display(Name = "شرح کالا / خدمات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.MultilineText)]
        public string ServiceDesc { get; set; }

        [Display(Name = "تعداد / مقدار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Number { get; set; }

        [Display(Name = "واحد اندازه گیری")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string UnitofMeasurement { get; set; }

        [Display(Name = "مبلغ واحد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Currency)]
        public double UnitAmount { get; set; }

        [Display(Name = "مبلغ کل")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Currency)]
        public double? TotalAmount { get; set; }

        [Display(Name = "مبلغ تخفیف")]
        [DataType(DataType.Currency)]
        public double? DiscountAmount { get; set; }

        [Display(Name = "مبلغ کل پس از تخفیف")]
        [DataType(DataType.Currency)]
        public double? TotalAmountafterDiscount { get; set; }

        [Display(Name = "مالیات")]
        [DataType(DataType.Currency)]
        public double? Tax { get; set; }

        [Display(Name = "عوارض")]
        [DataType(DataType.Currency)]
        public double? Complications { get; set; }

        [Display(Name = "جمع مالیات و عوارض")]
        [DataType(DataType.Currency)]
        public double? AmountTaxandComplications { get; set; }

        [Display(Name = "جمع مبلغ کل و مالیات و عوارض")]
        [DataType(DataType.Currency)]
        public double? TotalAmountTaxandComplications { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "شرایط و نحوه فروش")]
        public string SaleCondition { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        [Display(Name = "مبلغ تایید شده")]
        [DataType(DataType.Currency)]
        public double? AcceptedAmount { get; set; }

        [Display(Name = "تاریخ تایید شده")]
        public DateTime? AcceptedDate { get; set; }

        [Display(Name = "وضعیت")]
        public bool? Status { get; set; }

        [Display(Name = "نوع  ارز")]
        public int? CurrencyTypeID { get; set; }
        [ForeignKey("CurrencyTypeID")]
        public virtual CurrencyType CurrencyType { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        public int? CreatorUser { get; set; }

        [Display(Name = "کنترل کارشناسی")]
        public bool? CheckStatus { get; set; }

        [Display(Name = "تاریخ کنترل")]
        public DateTime? CkeckedDate { get; set; }

        [Display(Name = "کارشناس")]
        public string CkeckedUser { get; set; }

        [Display(Name = "کنترل مالی")]
        public bool? FinancialStatus { get; set; }

        [Display(Name = "تاریخ کنترل")]
        public DateTime? FinancialDate { get; set; }

        [Display(Name = "کارشناس")]
        public string FinancialUser { get; set; }

        [Display(Name = "تایید دریافت")]
        public bool? ReciveStatus { get; set; }

        [Display(Name = "تاریخ دریافت")]
        public DateTime? ReciveDate { get; set; }

        [Display(Name = "دریافت کننده")]
        public string ReciveUser { get; set; }
    }

    /// <summary>
    /// جهت ثبت و نگهداری تغییرات اطلاعات ارزش افزوده سالیانه 
    /// </summary>
    [Table("tbl_TaxandComplications")]
    public partial class TaxandComplications
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "مبلغ ارزش افزوده")]
        public double Value { get; set; }

        [Display(Name = "سال")]
        public string Year { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        public string Creator { get; set; }
    }


    /// <summary>
    /// جهت ثبت اطلاعات صورتحساب فروش قطعات شیر مخزن های ضایعاتی در طرح تعویض به کارگاه ها 
    /// </summary>
    [Table("tbl_InvoicesValveDamages")]
    public partial class InvoicesValveDamages
    {
        [Key]
        public int InvoiceID { get; set; }

        [Display(Name = "نوع خودرو")]
        public int? VehicleTypesID { get; set; }
        [ForeignKey("VehicleTypesID")]
        public virtual VehicleType VehicleTypes { get; set; }

        [Display(Name = "عنوان کارگاه")]
        public int? WorkshopsID { get; set; }
        [ForeignKey("WorkshopsID")]
        public virtual Workshop Workshops { get; set; }

        [Display(Name = "اقلام مصرفی")]
        public int? EquipmentsID { get; set; }
        [ForeignKey("EquipmentsID")]
        public virtual EquipmentList Equipments { get; set; }

        [Display(Name = "کد کالا / خدمات")]
        public string ServiceCode { get; set; }

        [Display(Name = "شماره سریال")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string InvoiceCode { get; set; }

        [Display(Name = "تاریخ")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "شماره اقتصادی")]
        public string EmployerEconomicalnumber { get; set; }

        [Display(Name = "شماره ثبت / شماره ملی")]
        public string Employerregistrationnumber { get; set; }

        [Display(Name = "استان")]
        public string EmployerState { get; set; }

        [Display(Name = "آدرس")]
        public string EmployerAddress { get; set; }

        [Display(Name = "کد پستی")]
        public string EmployerPostalcode { get; set; }

        [Display(Name = "تلفن")]
        public string EmployerPhone { get; set; }

        [Display(Name = "نمـابر")]
        public string EmployerFax { get; set; }

        [Display(Name = "شرح کالا / خدمات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.MultilineText)]
        public string ServiceDesc { get; set; }

        [Display(Name = "تعداد / مقدار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Number { get; set; }

        [Display(Name = "واحد اندازه گیری")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string UnitofMeasurement { get; set; }

        [Display(Name = "مبلغ واحد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Currency)]
        public double UnitAmount { get; set; }

        [Display(Name = "مبلغ کل")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Currency)]
        public double? TotalAmount { get; set; }

        [Display(Name = "مبلغ تخفیف")]
        [DataType(DataType.Currency)]
        public double? DiscountAmount { get; set; }

        [Display(Name = "مبلغ کل پس از تخفیف")]
        [DataType(DataType.Currency)]
        public double? TotalAmountafterDiscount { get; set; }

        [Display(Name = "مالیات")]
        [DataType(DataType.Currency)]
        public double? Tax { get; set; }

        [Display(Name = "عوارض")]
        [DataType(DataType.Currency)]
        public double? Complications { get; set; }

        [Display(Name = "جمع مالیات و عوارض")]
        [DataType(DataType.Currency)]
        public double? AmountTaxandComplications { get; set; }

        [Display(Name = "جمع مبلغ کل و مالیات و عوارض")]
        [DataType(DataType.Currency)]
        public double? TotalAmountTaxandComplications { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "شرایط و نحوه فروش")]
        public string SaleCondition { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        [Display(Name = "مبلغ تایید شده")]
        [DataType(DataType.Currency)]
        public double? AcceptedAmount { get; set; }

        [Display(Name = "تاریخ تایید شده")]
        public DateTime? AcceptedDate { get; set; }

        [Display(Name = "وضعیت")]
        public bool? Status { get; set; }

        [Display(Name = "نوع  ارز")]
        public int? CurrencyTypeID { get; set; }
        [ForeignKey("CurrencyTypeID")]
        public virtual CurrencyType CurrencyType { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        public int? CreatorUser { get; set; }

        [Display(Name = "کنترل کارشناسی")]
        public bool? CheckStatus { get; set; }

        [Display(Name = "تاریخ کنترل")]
        public DateTime? CkeckedDate { get; set; }

        [Display(Name = "کارشناس")]
        public string CkeckedUser { get; set; }

        [Display(Name = "کنترل مالی")]
        public bool? FinancialStatus { get; set; }

        [Display(Name = "تاریخ کنترل")]
        public DateTime? FinancialDate { get; set; }

        [Display(Name = "کارشناس")]
        public string FinancialUser { get; set; }

        [Display(Name = "تایید دریافت")]
        public bool? ReciveStatus { get; set; }

        [Display(Name = "تاریخ دریافت")]
        public DateTime? ReciveDate { get; set; }

        [Display(Name = "دریافت کننده")]
        public string ReciveUser { get; set; }

        [Display(Name = "سال تبدیل")]
        public string Year { get; set; }

        [Display(Name = "ماه تبدیل")]
        public string Month { get; set; }
    }

    /// <summary>
    /// ثبت اطلاعات واریزی کارگاه ها و غیره بابت خرید اقلام و خدمات
    /// </summary>

    [Table("tbl_Deposits")]
    public class Deposit
    {
        public int ID { get; set; }

        [Display(Name = "تاریخ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public DateTime Date { get; set; }

        [Display(Name = "عنوان کارگاه")]
        public int? WorkshopsID { get; set; }
        [ForeignKey("WorkshopsID")]
        public virtual Workshop Workshops { get; set; }

        [Display(Name = "کد ملی")]
        public string NationalCode { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }

        [Display(Name = "کد رهگیری")]
        public string TrackingCode { get; set; }

        [Display(Name = "مبلغ (ریال)")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Currency)]
        public double Value { get; set; }

        [Display(Name = "بانک")]
        public string Bank { get; set; }

        [Display(Name = "شعبه")]
        public string Branch { get; set; }

        [Display(Name = "کد قطعی")]
        public string DifinitiveCode { get; set; }

        [Display(Name = "کد عطفی")]
        public string InflectionCode { get; set; }

        [Display(Name = "شناسه واریز کننده")]
        public string DepositorID { get; set; }

        [Display(Name = "سریال")]
        public string Serial { get; set; }

        [Display(Name = "واریز کننده")]
        public string Depositor { get; set; }

        [Display(Name = "شرح")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "کاربر ثبت کننده")]
        public string Creator { get; set; }

    }

    public class DynamicRow
    {
        public string SelectedItem { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class DynamicFormViewModel
    {
        public List<DynamicRow> Rows { get; set; } = new List<DynamicRow>();
    }
}
