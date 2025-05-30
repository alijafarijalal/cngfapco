using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cngfapco.Models
{
    [Table("tbl_DivisionPlans")]
    public class DivisionPlan
    {
        public int ID { get; set; }

        [Display(Name = "کد تقسیم")]
        public string Code { get; set; }

        [Display(Name = "کارگاه مربوطه")]
        public int? WorkshopID { get; set; }
        [ForeignKey("WorkshopID")]
        public virtual Workshop Workshop { get; set; }

        [Display(Name = "تاریخ تقسیم")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "کاربر تقسیم")]
        public int? Creator { get; set; }

        [Display(Name = "تایید")]
        public bool? Confirmation { get; set; }

        [Display(Name = "تاریخ تایید")]
        public DateTime? ConfirmationDate { get; set; }

        [Display(Name = "کاربر تایید")]
        public int? ConfirmationUser { get; set; }

        [Display(Name = "ارسال")]
        public bool? Send { get; set; }

        [Display(Name = "تاریخ ارسال")]
        public DateTime? SendDate { get; set; }

        [Display(Name = "کاربر ارسال")]
        public int? Sender { get; set; }

        [Display(Name = "شرح")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "ثبت نهایی")]
        public bool? FinalCheck { get; set; }

        [Display(Name = "تاریخ ثبت نهایی")]
        public DateTime? FinalCheckDate { get; set; }

        [Display(Name = "فایل پیوست")]
        public string Attachment { get; set; }

    }

    /// <summary>
    /// طرح تقسیم مربوط به شیر مخزن
    /// </summary>
    [Table("tbl_ValveDivisionPlans")]
    public class ValveDivisionPlan
    {
        public int ID { get; set; }

        [Display(Name = "کد تقسیم")]
        public int? DivisionPlanID { get; set; }
        [ForeignKey("DivisionPlanID")]
        public virtual DivisionPlan DivisionPlan { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نوع")]
        public string Type { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "شرکت سازنده")]
        public int? ValveConstractorID { get; set; }
        [ForeignKey("ValveConstractorID")]
        public virtual ValveConstractor ValveConstractor { get; set; }      

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تعداد")]
        public int Number { get; set; }

        [Display(Name = "شرح")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "تعداد ارسالی")]
        public int? NumberofSend { get; set; }

        [Display(Name = "نسل کیت")]
        //[System.ComponentModel.DefaultValue(1)]
        public int? GenarationID { get; set; }
        [ForeignKey("GenarationID")]
        public virtual GenerationofRegulator Genarations { get; set; }

        [Display(Name = "نوع طرح")]
        public int? RegistrationTypeID { get; set; }
        [ForeignKey("RegistrationTypeID")]
        public virtual RegistrationType RegistrationType { get; set; }
    }

    /// <summary>
    /// طرح تقسیم مربوط به اقلام کیت
    /// </summary>
    [Table("tbl_KitDivisionPlans")]
    public class KitDivisionPlan
    {
        public int ID { get; set; }

        [Display(Name = "کد تقسیم")]
        public int? DivisionPlanID { get; set; }
        [ForeignKey("DivisionPlanID")]
        public virtual DivisionPlan DivisionPlan { get; set; }      

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نوع")]
        public int? VehicleTypeID { get; set; }
        [ForeignKey("VehicleTypeID")]
        public virtual VehicleType VehicleType { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تعداد")]
        public int Number { get; set; }

        [Display(Name = "شرح")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "تعداد ارسالی")]
        public int? NumberofSend { get; set; }

        [Display(Name = "نسل کیت")]
        //[System.ComponentModel.DefaultValue(1)]
        public int? GenarationID { get; set; }
        [ForeignKey("GenarationID")]
        public virtual GenerationofRegulator Genarations { get; set; }

        [Display(Name = "نوع طرح")]
        public int? RegistrationTypeID { get; set; }
        [ForeignKey("RegistrationTypeID")]
        public virtual RegistrationType RegistrationType { get; set; }

    }

    /// <summary>
    /// طرح تقسیم مربوط به اقلام مخزن
    /// </summary>
    [Table("tbl_TankDivisionPlans")]
    public class TankDivisionPlan
    {
        public int ID { get; set; }

        [Display(Name = "کد تقسیم")]
        public int? DivisionPlanID { get; set; }
        [ForeignKey("DivisionPlanID")]
        public virtual DivisionPlan DivisionPlan { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نوع")]
        public int? TypeofTankID { get; set; }
        [ForeignKey("TypeofTankID")]
        public virtual TypeofTank TypeofTank { get; set; }

        [Display(Name = "سازنده مخزن")]
        public int? TankConstractorID { get; set; }
        [ForeignKey("TankConstractorID")]
        public virtual TankConstractor TankConstractor { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تعداد")]
        public int Number { get; set; }

        [Display(Name = "شرح")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "تعداد ارسالی")]
        public int? NumberofSend { get; set; }

        [Display(Name = "نسل کیت")]
       //[System.ComponentModel.DefaultValue(1)]
        public int? GenarationID { get; set; }
        [ForeignKey("GenarationID")]
        public virtual GenerationofRegulator Genarations { get; set; }

        [Display(Name = "نوع طرح")]
        public int? RegistrationTypeID { get; set; }
        [ForeignKey("RegistrationTypeID")]
        public virtual RegistrationType RegistrationType { get; set; }

    }

    /// <summary>
    /// طرح تقسیم مربوط به اقلام پایه مخزن
    /// </summary>
    [Table("tbl_TankBaseDivisionPlans")]
    public class TankBaseDivisionPlan
    {
        public int ID { get; set; }

        [Display(Name = "کد تقسیم")]
        public int? DivisionPlanID { get; set; }
        [ForeignKey("DivisionPlanID")]
        public virtual DivisionPlan DivisionPlan { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نوع")]
        public int? TypeofTankBaseID { get; set; }
        [ForeignKey("TypeofTankBaseID")]
        public virtual TypeofTankBase TypeofTankBase { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تعداد")]
        public int Number { get; set; }

        [Display(Name = "شرح")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "تعداد ارسالی")]
        public int? NumberofSend { get; set; }

        [Display(Name = "نسل کیت")]
        //[System.ComponentModel.DefaultValue(1)]
        public int? GenarationID { get; set; }
        [ForeignKey("GenarationID")]
        public virtual GenerationofRegulator Genarations { get; set; }

        [Display(Name = "نوع طرح")]
        public int? RegistrationTypeID { get; set; }
        [ForeignKey("RegistrationTypeID")]
        public virtual RegistrationType RegistrationType { get; set; }

    }

    /// <summary>
    /// طرح تقسیم مربوط به اقلام کاور مخزن
    /// </summary>
    [Table("tbl_TankCoverDivisionPlans")]
    public class TankCoverDivisionPlan
    {
        public int ID { get; set; }

        [Display(Name = "کد تقسیم")]
        public int? DivisionPlanID { get; set; }
        [ForeignKey("DivisionPlanID")]
        public virtual DivisionPlan DivisionPlan { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نوع")]
        public int? TypeofTankCoverID { get; set; }
        [ForeignKey("TypeofTankCoverID")]
        public virtual TypeofTankCover TypeofTankCover { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تعداد")]
        public int Number { get; set; }

        [Display(Name = "شرح")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "تعداد ارسالی")]
        public int? NumberofSend { get; set; }

        [Display(Name = "نسل کیت")]
        //[System.ComponentModel.DefaultValue(1)]
        public int? GenarationID { get; set; }
        [ForeignKey("GenarationID")]
        public virtual GenerationofRegulator Genarations { get; set; }

        [Display(Name = "نوع طرح")]
        public int? RegistrationTypeID { get; set; }
        [ForeignKey("RegistrationTypeID")]
        public virtual RegistrationType RegistrationType { get; set; }
    }

    /// <summary>
    /// طرح تقسیم مربوط به اقلام دیگر
    /// </summary>
    [Table("tbl_OtherThingsDivisionPlans")]
    public class OtherThingsDivisionPlan
    {
        public int ID { get; set; }

        [Display(Name = "کد تقسیم")]
        public int? DivisionPlanID { get; set; }
        [ForeignKey("DivisionPlanID")]
        public virtual DivisionPlan DivisionPlan { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "عنوان")]
        public int? DiThingsID { get; set; }
        [ForeignKey("DiThingsID")]
        public virtual Otherthings DiThings { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تعداد طرح")]
        public int Number { get; set; }

        [Display(Name = "شرح")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "تعداد ارسالی")]
        public int? NumberofSend { get; set; }

        [Display(Name = "نسل کیت")]
        //[System.ComponentModel.DefaultValue(1)]
        public int? GenarationID { get; set; }
        [ForeignKey("GenarationID")]
        public virtual GenerationofRegulator Genarations { get; set; }

        [Display(Name = "نوع طرح")]
        public int? RegistrationTypeID { get; set; }
        [ForeignKey("RegistrationTypeID")]
        public virtual RegistrationType RegistrationType { get; set; }

    }
    /// <summary>
    /// طرح تقسیم بر اساس BOM
    /// </summary>
    [Table("tbl_DivisionPlanBOMs")]
    public class DivisionPlanBOM
    {
        public int ID { get; set; }

        [Display(Name = "طرح تقسیم مربوطه")]
        public int? DivisionPlanID { get; set; }
        [ForeignKey("DivisionPlanID")]
        public virtual DivisionPlan DivisionPlan { get; set; }

        [Display(Name = "نوع")]
        public int? BOMID { get; set; }
        [ForeignKey("BOMID")]
        public virtual BOM BOM { get; set; }

        [Display(Name = "تعداد طرح")]
        public double Number { get; set; }

        [Display(Name = "شرح")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "تعداد ارسالی")]
        public double? NumberofSend { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        public string Creator { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "نسل کیت")]
        //[System.ComponentModel.DefaultValue(1)]
        public int? GenarationID { get; set; }
        [ForeignKey("GenarationID")]
        public virtual GenerationofRegulator Genarations { get; set; }

        [Display(Name = "نوع طرح")]
        public int? RegistrationTypeID { get; set; }
        [ForeignKey("RegistrationTypeID")]
        public virtual RegistrationType RegistrationType { get; set; }

    }

    //Return of parts
    /// <summary>
    /// برگشت/ انتقال اقلام مازاد کارگاه ها
    /// </summary>
    [Table("tbl_ReturnofParts")]
    public class ReturnofParts
    {
        public int ID { get; set; }

        [Display(Name = "کد تقسیم")]
        public string Code { get; set; }

        [Display(Name = "مبداء")]
        public int? WorkshopID { get; set; }
        [ForeignKey("WorkshopID")]
        public virtual Workshop Workshop { get; set; }
        
        [Display(Name = "نوع خودرو")]
        public int? VehicleTypeID { get; set; }
        [ForeignKey("VehicleTypeID")]
        public virtual VehicleType VehicleTypes { get; set; }

        [Display(Name = "قطعه")]
        public int? EquipmentID { get; set; }
        [ForeignKey("EquipmentID")]
        public virtual EquipmentList Equipments { get; set; }

        [Display(Name = "دسته بندی")]
        public string TypeofPiece { get; set; }

        [Display(Name = "مقصد")]
        public int? Transferee { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "کاربر")]
        public int? Creator { get; set; }

        [Display(Name = "نوع اقدام")]
        public string Action { get; set; }

        [Display(Name = "تعداد")]
        public int NumberofSend { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "تاریخ")]
        public DateTime Date { get; set; }
    }
}