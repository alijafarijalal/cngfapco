using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cngfapco.Models
{
    public class MaterialsInformation
    {
    }
    /// <summary>
    /// اطلاعات بانک مخازن
    /// </summary>
    [Table("tbl_BankTanks")]
    public class BankTank
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "سریال")]
        public string serialNumber { get; set; }

        [Display(Name = "حجم")]
        public string bulk { get; set; }

        [Display(Name = "مدل")]
        public string model { get; set; }

        [Display(Name = "نوع")]
        public string type { get; set; }

        [Display(Name = "طول")]
        public string length { get; set; }

        [Display(Name = "فشار")]
        public string pressure { get; set; }

        [Display(Name = "قطر")]
        public string diameter { get; set; }

        [Display(Name = "رزوه")]
        public string rezve { get; set; }

        [Display(Name = "تاریخ تولید")]
        public string productDate { get; set; }

        [Display(Name = "تاریخ انقضا")]
        public string expireDate { get; set; }

        [Display(Name = "ماه تولید")]
        public string gregorianPMonth { get; set; }

        [Display(Name = "سال تولید")]
        public string gregorianPYear { get; set; }

        [Display(Name = "ماه انقضا")]
        public string gregorianEMonth { get; set; }

        [Display(Name = "سال انقضا")]
        public string gregorianEYear { get; set; }

        [Display(Name = "سازنده")]
        public string constractor { get; set; }

        [Display(Name = "نام کارگاه")]
        public string workshop { get; set; }

        [Display(Name = "وضعیت")]
        public string status { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "ثبت کننده")]
        public string Creator { get; set; }

        [Display(Name = "نام قطعه")]
        public string MaterailName { get; set; }
    }

    /// <summary>
    /// اطلاعات ثبت شده توسط کارگاه ها بابت مخازن
    /// </summary>
    [Table("tbl_Tanks")]
    public class Tank
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "سریال")]
        public string serialNumber { get; set; }

        [Display(Name = "حجم")]
        public string bulk { get; set; }

        [Display(Name = "مدل")]
        public string model { get; set; }

        [Display(Name = "نوع")]
        public string type { get; set; }

        [Display(Name = "طول")]
        public string length { get; set; }

        [Display(Name = "فشار")]
        public string pressure { get; set; }

        [Display(Name = "قطر")]
        public string diameter { get; set; }

        [Display(Name = "رزوه")]
        public string rezve { get; set; }

        [Display(Name = "تاریخ تولید")]
        public string productDate { get; set; }

        [Display(Name = "تاریخ انقضا")]
        public string expireDate { get; set; }

        [Display(Name = "ماه تولید")]
        public string gregorianPMonth { get; set; }

        [Display(Name = "سال تولید")]
        public string gregorianPYear { get; set; }

        [Display(Name = "ماه انقضا")]
        public string gregorianEMonth { get; set; }

        [Display(Name = "سال انقضا")]
        public string gregorianEYear { get; set; }

        [Display(Name = "سازنده")]
        public string constractor { get; set; }

        [Display(Name = "نام کارگاه")]
        public string workshop { get; set; }

        [Display(Name = "وضعیت")]
        public string status { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "ثبت کننده")]
        public string Creator { get; set; }

        [Display(Name = "تاریخ تغییر وضعیت")]
        public DateTime? RefreshDate { get; set; }

        [Display(Name = "تغییر دهنده")]
        public string RefreshCreator { get; set; }

        [Display(Name = "نام قطعه")]
        public string MaterailName { get; set; }
    }

    /// <summary>
    /// اطلاعات بانک شیر مخزن ها
    /// </summary>
    [Table("tbl_BankTankValves")]
    public class BankTankValve
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "سریال")]
        public string serialNumber { get; set; }

        [Display(Name = "سازنده")]
        public string constractor { get; set; }

        [Display(Name = "مدل")]
        public string model { get; set; }

        [Display(Name = "نوع")]
        public string type { get; set; }

        [Display(Name = "رزوه")]
        public string rezve { get; set; }

        [Display(Name = "تاریخ تولید")]
        public string productDate { get; set; }

        [Display(Name = "نام کارگاه")]
        public string workshop { get; set; }

        [Display(Name = "وضعیت")]
        public string status { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "ثبت کننده")]
        public string Creator { get; set; }

        [Display(Name = "نام قطعه")]
        public string MaterailName { get; set; }
    }


    /// <summary>
    /// اطلاعات ثبت شده توسط کارگاه ها بابت شیر مخزن ها
    /// </summary>
    [Table("tbl_TankValves")]
    public class TankValve
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "سریال")]
        public string serialNumber { get; set; }

        [Display(Name = "سازنده")]
        public string constractor { get; set; }        

        [Display(Name = "مدل")]
        public string model { get; set; }

        [Display(Name = "نوع")]
        public string type { get; set; }

        [Display(Name = "رزوه")]
        public string rezve { get; set; }

        [Display(Name = "تاریخ تولید")]
        public string productDate { get; set; }

        [Display(Name = "نام کارگاه")]
        public string workshop { get; set; }

        [Display(Name = "وضعیت")]
        public string status { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "ثبت کننده")]
        public string Creator { get; set; }

        [Display(Name = "تاریخ تغییر وضعیت")]
        public DateTime? RefreshDate { get; set; }

        [Display(Name = "تغییر دهنده")]
        public string RefreshCreator { get; set; }

        [Display(Name = "نام قطعه")]
        public string MaterailName { get; set; }
    }

    /// <summary>
    /// اطلاعات بانک رگلاتور
    /// </summary>
    [Table("tbl_BankKits")]
    public class BankKit
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "سریال")]
        public string serialNumber { get; set; }

        [Display(Name = "سازنده")]
        public string constractor { get; set; }

        [Display(Name = "نسل")]
        public string generation { get; set; }

        [Display(Name = "مدل")]
        public string model { get; set; }

        [Display(Name = "نوع")]
        public string type { get; set; }

        [Display(Name = "تاریخ تولید")]
        public string productDate { get; set; }

        [Display(Name = "نام کارگاه")]
        public string workshop { get; set; }

        [Display(Name = "وضعیت")]
        public string status { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "ثبت کننده")]
        public string Creator { get; set; }

        [Display(Name = "نام قطعه")]
        public string MaterailName { get; set; }
    }

    /// <summary>
    /// اطلاعات کیت (رگلاتور) مخزن ها
    /// </summary>
    [Table("tbl_Kits")]
    public class Kit
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "سریال")]
        public string serialNumber { get; set; }

        [Display(Name = "سازنده")]
        public string constractor { get; set; }

        [Display(Name = "نسل")]
        public string generation { get; set; }

        [Display(Name = "مدل")]
        public string model { get; set; }

        [Display(Name = "نوع")]
        public string type { get; set; }        

        [Display(Name = "تاریخ تولید")]
        public string productDate { get; set; }

        [Display(Name = "نام کارگاه")]
        public string workshop { get; set; }

        [Display(Name = "وضعیت")]
        public string status { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "ثبت کننده")]
        public string Creator { get; set; }

        [Display(Name = "تاریخ تغییر وضعیت")]
        public DateTime? RefreshDate { get; set; }

        [Display(Name = "تغییر دهنده")]
        public string RefreshCreator { get; set; }

        [Display(Name = "نام قطعه")]
        public string MaterailName { get; set; }
    }

    /// <summary>
    /// اطلاعات بانک شیر قطع کن
    /// </summary>
    [Table("tbl_BankCutofValves")]
    public class BankCutofValve
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "سریال")]
        public string serialNumber { get; set; }

        [Display(Name = "سازنده")]
        public string constractor { get; set; }

        [Display(Name = "مدل")]
        public string model { get; set; }        

        [Display(Name = "تاریخ تولید")]
        public string productDate { get; set; }

        [Display(Name = "نام کارگاه")]
        public string workshop { get; set; }

        [Display(Name = "وضعیت")]
        public string status { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "ثبت کننده")]
        public string Creator { get; set; }

        [Display(Name = "نام قطعه")]
        public string MaterailName { get; set; }
    }


    /// <summary>
    /// اطلاعات ثبت شده توسط کارگاه ها بابت شیر قطع کن
    /// </summary>
    [Table("tbl_CutofValves")]
    public class CutofValve
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "سریال")]
        public string serialNumber { get; set; }

        [Display(Name = "سازنده")]
        public string constractor { get; set; }

        [Display(Name = "مدل")]
        public string model { get; set; }

        [Display(Name = "تاریخ تولید")]
        public string productDate { get; set; }

        [Display(Name = "نام کارگاه")]
        public string workshop { get; set; }

        [Display(Name = "وضعیت")]
        public string status { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "ثبت کننده")]
        public string Creator { get; set; }

        [Display(Name = "تاریخ تغییر وضعیت")]
        public DateTime? RefreshDate { get; set; }

        [Display(Name = "تغییر دهنده")]
        public string RefreshCreator { get; set; }

        [Display(Name = "نام قطعه")]
        public string MaterailName { get; set; }
    }

    /// <summary>
    /// اطلاعات بانک شیر پر کن
    /// </summary>
    [Table("tbl_BankFillingValves")]
    public class BankFillingValve
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "سریال")]
        public string serialNumber { get; set; }

        [Display(Name = "سازنده")]
        public string constractor { get; set; }

        [Display(Name = "مدل")]
        public string model { get; set; }

        [Display(Name = "تاریخ تولید")]
        public string productDate { get; set; }

        [Display(Name = "نام کارگاه")]
        public string workshop { get; set; }

        [Display(Name = "وضعیت")]
        public string status { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "ثبت کننده")]
        public string Creator { get; set; }

        [Display(Name = "نام قطعه")]
        public string MaterailName { get; set; }
    }


    /// <summary>
    /// اطلاعات ثبت شده توسط کارگاه ها بابت شیر پر کن
    /// </summary>
    [Table("tbl_FillingValves")]
    public class FillingValve
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "سریال")]
        public string serialNumber { get; set; }

        [Display(Name = "سازنده")]
        public string constractor { get; set; }

        [Display(Name = "مدل")]
        public string model { get; set; }

        [Display(Name = "تاریخ تولید")]
        public string productDate { get; set; }

        [Display(Name = "نام کارگاه")]
        public string workshop { get; set; }

        [Display(Name = "وضعیت")]
        public string status { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "ثبت کننده")]
        public string Creator { get; set; }

        [Display(Name = "تاریخ تغییر وضعیت")]
        public DateTime? RefreshDate { get; set; }

        [Display(Name = "تغییر دهنده")]
        public string RefreshCreator { get; set; }

        [Display(Name = "QR")]
        public string QRCodeText { get; set; }

        [Display(Name = "نام قطعه")]
        public string MaterailName { get; set; }
    }

    /// <summary>
    /// اطلاعات بانک رله سوخت
    /// </summary>
    [Table("tbl_BankFuelRelays")]
    public class BankFuelRelay
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "نام قطعه")]
        public string MaterailName { get; set; }

        [Display(Name = "سریال")]
        public string serialNumber { get; set; }

        [Display(Name = "سازنده")]
        public string constractor { get; set; }

        [Display(Name = "نسل")]
        public string generation { get; set; }

        [Display(Name = "مدل")]
        public string model { get; set; }

        [Display(Name = "نوع")]
        public string type { get; set; }

        [Display(Name = "تاریخ تولید")]
        public string productDate { get; set; }

        [Display(Name = "تاریخ انقضا")]
        public string expireDate { get; set; }

        [Display(Name = "نام کارگاه")]
        public string workshop { get; set; }

        [Display(Name = "وضعیت")]
        public string status { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "ثبت کننده")]
        public string Creator { get; set; }
        
    }

    /// <summary>
    /// ثبت اطلاعات کارگاه ها برای رله سوخت
    /// </summary>
    [Table("tbl_FuelRelays")]
    public class FuelRelay
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "نام قطعه")]
        public string MaterailName { get; set; }

        [Display(Name = "سریال")]
        public string serialNumber { get; set; }

        [Display(Name = "سازنده")]
        public string constractor { get; set; }

        [Display(Name = "نسل")]
        public string generation { get; set; }

        [Display(Name = "مدل")]
        public string model { get; set; }

        [Display(Name = "نوع")]
        public string type { get; set; }

        [Display(Name = "تاریخ تولید")]
        public string productDate { get; set; }

        [Display(Name = "تاریخ انقضا")]
        public string expireDate { get; set; }

        [Display(Name = "نام کارگاه")]
        public string workshop { get; set; }

        [Display(Name = "وضعیت")]
        public string status { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "ثبت کننده")]
        public string Creator { get; set; }

        [Display(Name = "تاریخ تغییر وضعیت")]
        public DateTime? RefreshDate { get; set; }

        [Display(Name = "تغییر دهنده")]
        public string RefreshCreator { get; set; }
        
    }
    //////////////////////////////////////////////////
    /// <summary>
    /// اطلاعات بانک Gas ECU
    /// </summary>
    [Table("tbl_BankGasECU")]
    public class BankGasECU
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "نام قطعه")]
        public string MaterailName { get; set; }

        [Display(Name = "سریال")]
        public string serialNumber { get; set; }

        [Display(Name = "سازنده")]
        public string constractor { get; set; }

        [Display(Name = "نسل")]
        public string generation { get; set; }

        [Display(Name = "مدل")]
        public string model { get; set; }

        [Display(Name = "نوع")]
        public string type { get; set; }

        [Display(Name = "تاریخ تولید")]
        public string productDate { get; set; }

        [Display(Name = "تاریخ انقضا")]
        public string expireDate { get; set; }

        [Display(Name = "نام کارگاه")]
        public string workshop { get; set; }

        [Display(Name = "وضعیت")]
        public string status { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "ثبت کننده")]
        public string Creator { get; set; }

    }

    /// <summary>
    /// ثبت اطلاعات کارگاه ها برای Gass ECU
    /// </summary>
    [Table("tbl_GasECU")]
    public class GasECU
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "نام قطعه")]
        public string MaterailName { get; set; }

        [Display(Name = "سریال")]
        public string serialNumber { get; set; }

        [Display(Name = "سازنده")]
        public string constractor { get; set; }

        [Display(Name = "نسل")]
        public string generation { get; set; }

        [Display(Name = "مدل")]
        public string model { get; set; }

        [Display(Name = "نوع")]
        public string type { get; set; }

        [Display(Name = "تاریخ تولید")]
        public string productDate { get; set; }

        [Display(Name = "تاریخ انقضا")]
        public string expireDate { get; set; }

        [Display(Name = "نام کارگاه")]
        public string workshop { get; set; }

        [Display(Name = "وضعیت")]
        public string status { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "ثبت کننده")]
        public string Creator { get; set; }

        [Display(Name = "تاریخ تغییر وضعیت")]
        public DateTime? RefreshDate { get; set; }

        [Display(Name = "تغییر دهنده")]
        public string RefreshCreator { get; set; }

    }
    //////////////////////////////////////////////////
}