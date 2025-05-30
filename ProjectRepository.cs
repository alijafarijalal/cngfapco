using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.IO;
using System.Data.SqlClient;
using cngfapco.Models;
using static cngfapco.Controllers.VehicleRegistrationsController;
using System.Configuration;
using System.Data;
using System.Data.Entity;

namespace cngfapco.Models
{
    public class ProjectRepository
    {
        private ContextDB db = new ContextDB();
        public List<Registration> GetProjectList(int?[] WorkshopID, DateTime fromDate, DateTime toDate, bool? Post)
        {
           //string projectFile = HostingEnvironment.MapPath("~/App_Data/Projects.txt");
            List<Registration> tempList = new List<Registration>();
            if (Post != true)
                fromDate = Convert.ToDateTime("1399/01/01"); //DateTime.Now.AddYears(-2);
            List<Workshop> tableOuts = new List<Workshop>();
            var workshops = db.tbl_Workshops.ToList();
            //string permission = "7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,"; //db.tbl_Users.Where(u => u.Username == userName).SingleOrDefault()
            var registration = db.tbl_VehicleRegistrations.Where(w=> w.RegisterStatus == true).Include(v=>v.VehicleType).Include(v=>v.TypeofUse).Include(v=>v.Workshop).ToList();
            var userRole = Helper.Helpers.GetCurrentUserRoles();

            //foreach (var role in userRole)
            //{
            //    if (role.RoleName.Contains("مدیر تبدیل ناوگان") || role.RoleName.Contains("admin"))
            //    {
            //        foreach (var item in workshops)
            //        {
            //            permission += item.ID + ",";
            //        }
            //    }

            //    if (role.RoleName.Contains("مرکز خدمات (کارگاه)"))
            //    {
            //        permission = Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
            //    }

            //    if (!role.RoleName.Contains("مرکز خدمات (کارگاه)") && !role.RoleName.Contains("مدیر تبدیل ناوگان") && !role.RoleName.Contains("admin"))
            //    {
            //        if (WorkshopID == null || string.IsNullOrEmpty(WorkshopID[0].ToString()))
            //        {
            //            foreach (var item in workshops)
            //            {
            //                Workshop workshop = db.tbl_Workshops.Find(item.ID);
            //                User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

            //                if (workshop.Users.Contains(_user))
            //                {
            //                    permission += item.ID + ",";
            //                }

            //            };
            //        }
            //        else
            //        {
            //            foreach (var item in WorkshopID)
            //            {
            //                Workshop workshop = db.tbl_Workshops.Find(item);
            //                User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

            //                if (workshop.Users.Contains(_user))
            //                {
            //                    permission += item + ",";
            //                }

            //            };

            //        }
            //    }
            //}

            //برای بخش جدول اطلاعات ماشین های تبدیل شده
            //string ID = "";
            //string WorkshopTitle = "";
            //string VehicleType = "";
            //string TypeofUse = "";
            //string FullName = "";
            //string MobileNumber = "";
            //LeftNumberPlate, AlphaPlate, RightNumberPlate, IranNumberPlate, EngineNumber, ChassisNumber,
            //string Plate = "";
            //string EngineNumber = "";
            //string ChassisNumber = "";
            //string VIN = "";
            //string InsuranceNumber = "";
            //
            List<VehicleRegistrationList> vehicleregistration = new List<VehicleRegistrationList>();
            //
            //SqlDataReader reader;
            //var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
           foreach(var item in registration)
            {
                tempList.Add(new Registration
                {
                    ID = item.ID.ToString(),
                    ChassisNumber = item.ChassisNumber,
                    TypeofUse =item.TypeofUseID.HasValue? item.TypeofUse.Type: item.TypeofUseID.ToString(),
                    WorkshopTitle = item.Workshop.Title,
                    EngineNumber = item.EngineNumber,
                    FullName = item.OwnerName + " " + item.OwnerFamily,
                    MobileNumber = item.MobileNumber,
                    Plate = item.AlphaPlate,
                    VehicleType = item.VehicleType.Type,
                    VIN = item.VIN,
                    //InsuranceNumber = cngfapco.Controllers.VehicleRegistrationsController.GetInsuranceCode(Convert.ToInt32(ID))
                });
            }

            return tempList;
        }
    }
}