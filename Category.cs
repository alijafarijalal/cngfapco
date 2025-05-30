using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cngfapco.Models
{
    [Table("tbl_Categories")]
    public class Category
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "دسته بندی موضوعی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Cat { get; set; }

        [Display(Name = "موضوع")]
        public int? SubCategory { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "ایجاد کننده")]
        public string Creator { get; set; }
    }
}