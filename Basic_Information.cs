using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cngfapco.Models
{
    public class Basic_Information
    {
    }
    /// <summary>
    /// تعریف موارد قابل نمایش در منوی سمت راست
    /// </summary>
    [Table("tbl_SideBarItems")]
    public partial class SideBarItem
    {
        public SideBarItem()
        {
            Users = new HashSet<User>();
        }
        //Cat Id
        [Key]
        public int ID { get; set; }

        //Cat nameOption
        [Display(Name = "nameOption")]
        public string nameOption { get; set; }

        //Cat controller
        [Display(Name = "controller")]
        public string controller { get; set; }

        //Cat action
        [Display(Name = "action")]
        public string action { get; set; }

        //Cat action
        [Display(Name = "imageClass")]
        public string imageClass { get; set; }

        //Cat status
        [Display(Name = "status")]
        public bool status { get; set; }

        //Cat status
        [Display(Name = "isParent")]
        public bool isParent { get; set; }

        //represnts Parent ID and it's nullable
        public int? parentId { get; set; }
        [ForeignKey("parentId")]
        public virtual SideBarItem Parent { get; set; }
        public virtual ICollection<SideBarItem> Childs { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public int? orderBy { get; set; }
        public string Category { get; set; }

    }

    /// <summary>
    /// اطلاعات پایه نوع خودرو
    /// </summary>
    [Table("tbl_VehicleTypes")]
    public class VehicleType
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نوع خودرو")]
        public string Type { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "حجم مخزن")]
        //[DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "عکس خودرو")]
        public string Image { get; set; }

        //[Display(Name = "نوع طرح")]
        //public int? RegistrationTypeID { get; set; }
        //[ForeignKey("RegistrationTypeID")]
        //public virtual RegistrationType RegistrationType { get; set; }
    }

    /// <summary>
    /// اطلاعات پایه سازنده مخزن
    /// </summary>
    [Table("tbl_TankConstractors")]
    public class TankConstractor
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "سازنده مخزن")]
        public string Constractor { get; set; }

        [Display(Name = "علامت اختصاری")]
        public string Code { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }

    /// <summary>
    /// اطلاعات پایه سازنده شیر مخزن
    /// </summary>
    [Table("tbl_ValveConstractors")]
    public class ValveConstractor
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "سازنده شیر مخزن")]
        public string Valve { get; set; }

        [Display(Name = "علامت اختصاری")]
        public string Code { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
    
    /// <summary>
    /// اطلاعات پایه سازنده رگلاتور
    /// </summary>
    [Table("tbl_RegulatorConstractors")]
    public class RegulatorConstractor
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "سازنده کیت (رگلاتور)")]
        public string Regulator { get; set; }

        [Display(Name = "علامت اختصاری")]
        public string Code { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }

    /// <summary>
    /// اطلاعات پایه سازنده رله سوخت
    /// </summary>
    [Table("tbl_FuelRelayConstractors")]
    public class FuelRelayConstractor
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "سازنده ریل سوخت")]
        public string FuelRelay { get; set; }

        [Display(Name = "علامت اختصاری")]
        public string Code { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }

    /// <summary>
    /// اطلاعات پایه سازنده Gas ECU
    /// </summary>
    [Table("tbl_GasECUConstractors")]
    public class GasECUConstractor
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "سازنده Gas ECU")]
        public string GasECU { get; set; }

        [Display(Name = "علامت اختصاری")]
        public string Code { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }

    /// <summary>
    /// اطلاعات پایه نوع کاربری خودرو
    /// </summary>
    [Table("tbl_TypeofUses")]
    public class TypeofUse
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نوع کاربری")]
        public string Type { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }

    /// <summary>
    /// اطلاعات پایه نوع مخزن
    /// </summary>
    [Table("tbl_TypeofTanks")]
    public class TypeofTank
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نوع مخزن")]
        public string Type { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "نوع خودرو")]
        public int? VehicleTypeId { get; set; }
        [ForeignKey("VehicleTypeId")]
        public virtual VehicleType VehicleType { get; set; }
    }

    /// <summary>
    /// اطلاعات پایه نوع پایه مخزن
    /// </summary>
    [Table("tbl_TypeofTankBases")]
    public class TypeofTankBase
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نوع پایه مخزن")]
        public string Type { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نوع خودرو")]
        public int? VehicleTypeId { get; set; }
        [ForeignKey("VehicleTypeId")]
        public virtual VehicleType VehicleType { get; set; }
        public TypeofTankBase()
        {
            VehicleTypeId = 1 ;
        }
    }

    /// <summary>
    /// اطلاعات پایه نوع کاور مخزن
    /// </summary>
    [Table("tbl_TypeofTankCovers")]
    public class TypeofTankCover
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نوع کاور مخزن")]
        public string Type { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نوع خودرو")]
        public int? VehicleTypeId { get; set; }
        [ForeignKey("VehicleTypeId")]
        public virtual VehicleType VehicleType { get; set; }
        //public TypeofTankCover()
        //{
        //    VehicleTypeId = 1;
        //}
    }

    /// <summary>
    /// اطلاعات پایه سایر اقلام
    /// </summary>
    [Table("tbl_Otherthings")]
    public class Otherthings
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }

    /// <summary>
    /// اطلاعات پایه سازنده شیر قطع کن
    /// </summary>
    [Table("tbl_CutofValveConstractors")]
    public class CutofValveConstractor
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "سازنده شیر قطع کن")]
        public string CutofValve { get; set; }

        [Display(Name = "علامت اختصاری")]
        public string Code { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
    /// <summary>
    /// اطلاعات پایه سازنده شیر پر کن
    /// </summary>
    [Table("tbl_FillingValveConstractors")]
    public class FillingValveConstractor
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "سازنده شیر پر کن")]
        public string FillingValve { get; set; }

        [Display(Name = "علامت اختصاری")]
        public string Code { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }

    /// <summary>
    /// اطلاعات تکمیلی مخازن مانندحجم، طول و ... 
    /// </summary>
    [Table("tbl_CylinderDetails")]
    public class CylinderDetail
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "سازنده مخزن")]
        public int? ConstractorId { get; set; }
        [ForeignKey("ConstractorId")]
        public virtual TankConstractor Constractors { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "حجم")]
        public string Bulk { get; set; }

        [Display(Name = "طول")]
        public string Lenght { get; set; }

        [Display(Name = "فشار")]
        public string Pressure { get; set; }

        [Display(Name = "قطر")]
        public string Diameter { get; set; }

        [Display(Name = "رزوه")]
        public string Rezve { get; set; }

        [Display(Name = "مدل")]
        public string Model { get; set; }
    }

}