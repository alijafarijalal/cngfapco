using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cngfapco.Models
{
    [Table("tbl_Instructions")]
    public class Instruction
    {
        public int ID { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "تاریخ تصویب")]
        public DateTime? DateofApproval { get; set; }

        [Display(Name = "تاریخ اعتبار")]
        public DateTime? ValidityDate { get; set; }

        [Display(Name = "کد سند")]
        public string Code { get; set; }

        [Display(Name = "وضعیت")]
        public bool Status { get; set; }

        [Display(Name = "فایل")]
        public string Attachment { get; set; }

        [Display(Name = "دسته بندی موضوعی")]
        public int? CategoriesID { get; set; }
        [ForeignKey("CategoriesID")]
        public virtual Category Categories { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "سطح انتشار")]
        public string PublishLevel { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        public string Creator { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }
    }
    //
    [Table("tbl_InstructionForms")]
    public class InstructionForm
    {
        public int ID { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "فایل")]
        public string Attachment { get; set; }

        [Display(Name = "دستورالعمل مرتبط")]
        public int? InstructionID { get; set; }
        [ForeignKey("InstructionID")]
        public virtual Instruction Instruction { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        public string Creator { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }
    }
}