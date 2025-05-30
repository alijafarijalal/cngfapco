using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cngfapco.Models
{
    public class Charts
    {

    }
    //
    public class ChartData
    {
        public string Category { get; set; }

        public double Value1 { get; set; }
        public double Value2 { get; set; }

        public string Color { get; set; }
    }
    //
    public class RadarChartData
    {
        /// <summary>
        /// ارسال و تبدیل
        /// </summary>
        /// <returns></returns>
        public static List<RadarChartData> GetData()
        {
            var data = new List<RadarChartData>();

            data.Add(new RadarChartData("62 لیتری", 70, 10));
            data.Add(new RadarChartData("75 لیتری", 1275, 1006));
            data.Add(new RadarChartData("100 لیتری", 828, 634));
            data.Add(new RadarChartData("113 لیتری", 250, 236));
            data.Add(new RadarChartData("120 لیتری", 99, 76));

            return data;
        }
        /// <summary>
        /// ثبت نام و تبدیل
        /// </summary>
        /// <returns></returns>
        public static List<RadarChartData> GetData2()
        {
            var data = new List<RadarChartData>();

            data.Add(new RadarChartData("RD /پراید/ پیکان", 8, 3));
            data.Add(new RadarChartData("وانت پراید	", 46, 1));
            data.Add(new RadarChartData("پژو/ سمند", 332, 264));
            data.Add(new RadarChartData("وانت پیکان/ مزدا", 475, 186));
            data.Add(new RadarChartData("وانت نیسان", 224, 71));
            data.Add(new RadarChartData("سایر", 9, 0));

            return data;
        }

        public RadarChartData(string label, double value1, double value2)
        {
            this.Label = label;
            this.Value1 = value1;
            this.Value2 = value2;
        }

        public string Label { get; set; }
        public double Value1 { get; set; }
        public double Value2 { get; set; }
    }
}