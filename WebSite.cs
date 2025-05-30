using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace cngfapco.Models
{
    public class WebSite
    {
    }
    /// <summary>
    /// تعریف موارد قابل نمایش در اسلایدر اصلی وب سایت
    /// </summary>
    [Table("tbl_Sliders")]
    public partial class Slider
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "انتخاب تصویر (اندازه: 1400 * 377)")]
        public string Image { get; set; }

        [Display(Name = "موضوع")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "دسته بندی موضوعی")]
        public string Cat { get; set; }

        [Display(Name = "اولویت نمایش")]
        public int? Order { get; set; }

        [Display(Name = "قابل نمایش")]
        public bool Presentable { get; set; }

        [Display(Name = "مدت زمان نمایش (روز)")]
        public int DueDate { get; set; }

        [Display(Name = "آدرس لینک")]
        public string Url { get; set; }

        [Display(Name = "محل نمایش")]
        public string Section { get; set; }

        [Display(Name = "منبع")]
        public string Refrence { get; set; }

        [Display(Name = "ایجاد کننده")]
        public string Creator { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreatDate { get; set; }

    }

    /// <summary>
    /// تعریف موارد قابل نمایش در بخش لینک های پرکاربرد وب سایت
    /// </summary>
    [Table("tbl_ContactUs")]
    public partial class ContactUs
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "تصویر")]
        public string Image { get; set; }

        [Display(Name = "آیکن")]
        public string Icon { get; set; }

        [Display(Name = "متن ")]
        public string Title { get; set; }
        
        [Display(Name = "اولویت نمایش")]
        public int? Order { get; set; }

        [Display(Name = "قابل نمایش")]
        public bool Presentable { get; set; }

        [Display(Name = "آدرس لینک")]
        public string Url { get; set; }

        [Display(Name = "محل نمایش")]
        public string Section { get; set; }

        [Display(Name = "ایجاد کننده")]
        public string Creator { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreatDate { get; set; }

    }

    /// <summary>
    /// نمایش دفترچه راهنمای CNG
    /// </summary>
    [Table("tbl_CNGHandBooks")]
    public partial class CNGHandBooks
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "تصویر")]
        public string Image { get; set; }

        [Display(Name = "آیکن")]
        public string Icon { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "شرح ")]
        public string Description { get; set; }

        [Display(Name = "اولویت نمایش")]
        public int? Order { get; set; }

        [Display(Name = "قابل نمایش")]
        public bool Presentable { get; set; }

        [Display(Name = "آدرس لینک")]
        public string Url { get; set; }

        [Display(Name = "شماره صفحه")]
        public string Page { get; set; }

        [Display(Name = "ایجاد کننده")]
        public string Creator { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreatDate { get; set; }

    }

}