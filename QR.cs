using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cngfapco.Models
{
    public class QRCodeModel
    {
        [Display(Name = "QRCode سریال")]
        public string QRCodeText { get; set; }

        [Display(Name = "QRCode شمارنده")]
        public string QRCodeCount { get; set; }

        [Display(Name = "QRCode تصویر")]
        public string QRCodeImagePath { get; set; }
    }
}