using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace cngfapco.Models
{
    /// <summary>
    /// تعریف مقادیر پایه خدمات حین و پس از نصب خودرو
    /// </summary>
    [Table("tbl_ListofServices")]
    public class ListofServices
    {
        public int ID { get; set; }

        [Display(Name = "شرح خدمات")]
        public string Title { get; set; }

        [Display(Name = "اجرت خدمات")]
        [DataType(DataType.Currency)]
        public double ServiceRent { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "نوع خدمات")]
        public string Type { get; set; }

        [Display(Name = "واحد شمارش خدمات")]
        public string Unit { get; set; }

        [Display(Name = "نمایش در لیست")]
        public bool Presentable { get; set; }
    }
}