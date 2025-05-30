using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cngfapco.Models
{
    /// <summary>
    /// برای ارسال پیام
    /// </summary>
    [Table("tbl_Messages")]
    public class Message
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "موضوع")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "متن")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "اولویت")]
        public string Priority { get; set; }

        [Display(Name = "شماره نامه")]
        public string LetterNumber { get; set; }

        [Display(Name = "ارسال کننده")]
        public int? SenderID { get; set; }
        [ForeignKey("SenderID")]
        public virtual User Sender { get; set; }

        [Display(Name = "تاریخ")]
        public DateTime SenderDate { get; set; }

        [Display(Name = "دریافت کننده")]
        public int? WorkshopID { get; set; }
        [ForeignKey("WorkshopID")]
        public virtual Workshop Workshop { get; set; }

        [Display(Name = "وضعیت مشاهده")]
        public bool? ReadStatus { get; set; }

        [Display(Name = "تاریخ مشاهده")]
        public DateTime? ReadDate { get; set; }

        [Display(Name = "فایل پیوست")]
        public string Attachment { get; set; }

        [Display(Name = "کاربر دریافت کننده")]
        public int? ReciverID { get; set; }

        [Display(Name = "پیام مرتبط")]
        public int? MessageID { get; set; }
        [ForeignKey("MessageID")]
        public virtual Message Messages { get; set; }
        public virtual ICollection<Message> Childs { get; set; }

        [Display(Name = "نوع")]
        public int? Type { get; set; }

    }

    /// <summary>
    /// برای پاسخ به پیام های دریافتی
    /// </summary>
    [Table("tbl_MessageReplies")]
    public class MessageReply
    {
        public int ID { get; set; }

        [Display(Name = "پیام مربوطه")]
        public int? MessageID { get; set; }
        [ForeignKey("MessageID")]
        public virtual Message Message { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "موضوع")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "متن")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }

        [Display(Name = "ارسال کننده")]
        public int? SenderID { get; set; }
        [ForeignKey("SenderID")]
        public virtual User Sender { get; set; }

        [Display(Name = "تاریخ ارسال")]
        public DateTime SenderDate { get; set; }

        [Display(Name = "دریافت کننده")]
        public int? ReciverID { get; set; }

        [Display(Name = "وضعیت مشاهده")]
        public bool? ReadStatus { get; set; }

        [Display(Name = "تاریخ مشاهده")]
        public DateTime? ReadDate { get; set; }

        [Display(Name = "فایل پیوست")]
        public string Attachment { get; set; }

    }

    /// <summary>
    /// برای ارسال پیام های دریافتی به دیگران
    /// </summary>
    [Table("tbl_MessageForwards")]
    public class MessageForward
    {
        public int ID { get; set; }

        [Display(Name = "پیام مربوطه")]
        public int? MessageID { get; set; }
        [ForeignKey("MessageID")]
        public virtual Message Message { get; set; }

        [Display(Name = "ارسال کننده")]
        public int? SenderID { get; set; }
        [ForeignKey("SenderID")]
        public virtual User Sender { get; set; }

        [Display(Name = "تاریخ ارسال")]
        public DateTime SenderDate { get; set; }

        [Display(Name = "دریافت کننده")]
        public int? ReciverID { get; set; }

        [Display(Name = "وضعیت مشاهده")]
        public bool? ReadStatus { get; set; }

        [Display(Name = "تاریخ مشاهده")]
        public DateTime? ReadDate { get; set; }

        [Display(Name = "فایل پیوست")]
        public string Attachment { get; set; }

    }

}