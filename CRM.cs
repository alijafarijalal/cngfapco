using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cngfapco.Models
{
    /// <summary>
    /// برای ثبت اطلاعات فرم سنجش رضایت مشتری با کد  : IF-02-003
    /// </summary>
    [Table("tbl_CRMs")]
    public class CRM
    {
        public int ID { get; set; }

        [Display(Name = "عنوان شاخص")]
        public int? IndexId { get; set; }
        [ForeignKey("IndexId")]
        public virtual CRMIndex Index { get; set; }

        [Display(Name = "نظر دهنده")]
        public int? OwnersId { get; set; }
        [ForeignKey("OwnersId")]
        public virtual VehicleRegistration Owners { get; set; }

        [Display(Name = "امتیاز شاخص")]
        public int Point { get; set; }

        public string Description1 { get; set; }
        public string Description2 { get; set; }

        [Display(Name = "پیشنهادات")]
        public string Suggestion { get; set; }

        [Display(Name = "تاریخ ثبت نظر")]
        public DateTime CreateDate { get; set; }
    }
    /// <summary>
    /// برای تعریف شاخص های فرم سنجش رضایت مشتری
    /// </summary>
    [Table("tbl_CRMIndexes")]
    public class CRMIndex
    {
        public CRMIndex()
        {
            CRMs = new HashSet<CRM>();
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "عنوان شاخص")]
        public string Title { get; set; }

        [Display(Name = "عنوان شاخص اصلی")]
        public int? PId { get; set; }
        [ForeignKey("PId")]
        public virtual CRMIndex Parent { get; set; }
        public virtual ICollection<CRMIndex> Childs { get; set; }

        //Cat Description
        [Display(Name = "نمایش در لیست")]
        public bool Presentable { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        public string Creator { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }

        public virtual ICollection<CRM> CRMs { get; set; }

    }

    public class clsMain
    {
        public string[] selectedAnswer { get; set; }

        public List<ClsQuestions> lstQuestion { get; set; }
        public List<ClsOptions> lstOptions { get; set; }
    }

    public class ClsQuestions
    {
        public string question { get; set; }
    }

    public class ClsOptions
    {
        public int optionid { get; set; }
        public string optionvalue { get; set; }
        public string optionlable { get; set; }
    }

    /// <summary>
    /// برای ثبت اطلاعات پیام های ارسالی
    /// </summary>
    [Table("tbl_SMSPanelResults")]
    public class SMSPanelResult
    {       
        public int ID { get; set; }

        [Display(Name = "شماره موبایل")]
        public string Mobile { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }

        [Display(Name = "کارگاه مربوطه")]
        public string Workshop { get; set; }

        [Display(Name = "کد دریافت کننده")]
        public string refID { get; set; }

        [Display(Name = "وضعیت")]
        public string Result { get; set; }

        [Display(Name = "ارسال کننده")]
        public string Sender { get; set; }

        [Display(Name = "IP ارسال کننده")]
        public string SenderIP { get; set; }

        [Display(Name = "تاریخ ارسال")]
        public DateTime SendDate { get; set; }

        [Display(Name = "Section")]
        public string Section { get; set; }

    }

    /// <summary>
    /// برای تعریف فرم نظر سنجی از مشتریان
    /// </summary>
    [Table("tbl_CRMDynamicForms")]
    public class CRMDynamicForm
    {
        public int ID { get; set; }

        [Display(Name = "متن نظر")]
        public string Title { get; set; }

        [Display(Name = "امکان بله و خیر")]
        public bool OkNotOk { get; set; }

        [Display(Name = "امکان توضیحات")]
        public bool Description { get; set; }

        [Display(Name = "امکان نمایش")]
        public bool isShow { get; set; }       

        [Display(Name = "ایجاد کننده")]
        public string Creator { get; set; }       

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }
        
    }

    /// <summary>
    /// برای ثبت اطلاعات نظر سنجی از مشتریان
    /// </summary>
    [Table("tbl_AnswerQuestions")]
    public class AnswerQuestion
    {
        public int ID { get; set; }

        [Display(Name = "سوال")]
        public int? QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual CRMDynamicForm Question { get; set; }

        [Display(Name = "شرح نظر")]
        public string Description { get; set; }

        [Display(Name = "بله و خیر")]
        public bool OkNotOk { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }

        [Display(Name = "شماره موبایل")]
        public string Mobile { get; set; }

        [Display(Name = "ip")]
        public string ip { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "مشتری")]
        public int? RegistrationsId { get; set; }
        [ForeignKey("RegistrationsId")]
        public virtual VehicleRegistration Registrations { get; set; }

    }

}