using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace cngfapco.Models
{

    /// <summary>
    /// ثبت مشخصات مشتریان خرید کالا در طرح فروش آزاد اقلام
    /// </summary>
    [Table("tbl_Customers")]
    public class Customer
    {
        public int ID { get; set; }

        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        public string LasstName { get; set; }

        [Display(Name = "کد ملی")]
        public string NationalCode { get; set; }

        [Display(Name = "تاریخ تولد")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "شماره اقتصادی")]
        public string Economicalnumber { get; set; }

        [Display(Name = "کد پستی")]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [Display(Name = "شهر")]
        public int? CitiesId { get; set; }
        [ForeignKey("CitiesId")]
        public virtual City Cities { get; set; }

        [Display(Name = "آدرس")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "شماره موبایل")]
        public string Mobile { get; set; }

        [Display(Name = "شماره ثابت")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "شماره نمابر")]
        [DataType(DataType.PhoneNumber)]
        public string Fax { get; set; }

        [Display(Name = "سقف اعتبار")]
        [DataType(DataType.Currency)]
        public double CreditLimit { get; set; }

        [Display(Name = "وضعیت")]
        public bool Status { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        public string Creator { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }
    }
    ///
    /// <summary>
    /// جهت ثبت اطلاعات فاکتورنهایی مشتریان
    /// </summary>
    [Table("tbl_CustomerFinallSaleInvoices")]
    public partial class CustomerFinallSaleInvoice
    {
        [Key]
        public int InvoiceID { get; set; }

        [Display(Name = "خریدار")]
        public int? CustomersID { get; set; }
        [ForeignKey("CustomersID")]
        public virtual Customer Customers { get; set; }

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
        public string Economicalnumber { get; set; }

        [Display(Name = "شماره ثبت / شماره ملی")]
        public string Registrationnumber { get; set; }

        [Display(Name = "استان")]
        public string State { get; set; }

        [Display(Name = "آدرس")]
        public string Address { get; set; }

        [Display(Name = "کد پستی")]
        public string Postalcode { get; set; }

        [Display(Name = "تلفن")]
        public string Phone { get; set; }

        [Display(Name = "نمـابر")]
        public string Fax { get; set; }

        [Display(Name = "شرح کالا / خدمات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.MultilineText)]
        public string ServiceDesc { get; set; }

        [Display(Name = "تعداد / مقدار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Number { get; set; }

        [Display(Name = "واحد اندازه گیری")]
        public string UnitofMeasurement { get; set; }

        [Display(Name = "مبلغ واحد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Currency)]
        public double UnitAmount { get; set; }

        [Display(Name = "مبلغ کل")]
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
    /// جهت ثبت و صدور اطلاعات پیش فاکتور مشتریان
    /// </summary>
    [Table("tbl_CustomerPreSaleInvoices")]
    public partial class CustomerPreSaleInvoice
    {
        [Key]
        public int InvoiceID { get; set; }

        [Display(Name = "خریدار")]
        public int? CustomersID { get; set; }
        [ForeignKey("CustomersID")]
        public virtual Customer Customers { get; set; }

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
        public string Economicalnumber { get; set; }

        [Display(Name = "شماره ثبت / شماره ملی")]
        public string Registrationnumber { get; set; }

        [Display(Name = "استان")]
        public string State { get; set; }

        [Display(Name = "آدرس")]
        public string Address { get; set; }

        [Display(Name = "کد پستی")]
        public string Postalcode { get; set; }

        [Display(Name = "تلفن")]
        public string Phone { get; set; }

        [Display(Name = "نمـابر")]
        public string Fax { get; set; }

        [Display(Name = "شرح کالا / خدمات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.MultilineText)]
        public string ServiceDesc { get; set; }

        [Display(Name = "تعداد / مقدار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Number { get; set; }

        [Display(Name = "واحد اندازه گیری")]
        public string UnitofMeasurement { get; set; }

        [Display(Name = "مبلغ واحد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Currency)]
        public double UnitAmount { get; set; }

        [Display(Name = "مبلغ کل")]
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
        public string RequestCode { get; set; }

    }

    /// <summary>
    /// جهت ثبت و صدور درخواست خرید مشتریان
    /// </summary>
    [Table("tbl_CustomerRequests")]
    public partial class CustomerRequest
    {
        [Key]
        public int InvoiceID { get; set; }

        [Display(Name = "خریدار")]
        public int? CustomersID { get; set; }
        [ForeignKey("CustomersID")]
        public virtual Customer Customers { get; set; }

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
        public string Economicalnumber { get; set; }

        [Display(Name = "شماره ثبت / شماره ملی")]
        public string Registrationnumber { get; set; }

        [Display(Name = "استان")]
        public string State { get; set; }

        [Display(Name = "آدرس")]
        public string Address { get; set; }

        [Display(Name = "کد پستی")]
        public string Postalcode { get; set; }

        [Display(Name = "تلفن")]
        public string Phone { get; set; }

        [Display(Name = "نمـابر")]
        public string Fax { get; set; }

        [Display(Name = "شرح کالا / خدمات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.MultilineText)]
        public string ServiceDesc { get; set; }

        [Display(Name = "تعداد / مقدار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Number { get; set; }

        [Display(Name = "واحد اندازه گیری")]
        public string UnitofMeasurement { get; set; }

        [Display(Name = "مبلغ واحد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Currency)]
        public double UnitAmount { get; set; }

        [Display(Name = "مبلغ کل")]
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

    }
}