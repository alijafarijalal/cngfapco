using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZXing;
using cngfapco.Models;
using System.Data.Entity;
using Rotativa;

namespace cngfapco.Controllers
{
    public class QRsController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: QRs
        public ActionResult Index()
        {
            var workshops = db.tbl_Workshops.Where(w => w.isServices == true).ToList();
            List<Workshop> list = new List<Workshop>();

            foreach (var item in workshops)
            {
                Workshop workshop = db.tbl_Workshops.Find(item.ID);
                User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                if (workshop.Users.Contains(_user))
                {
                    list.Add(new Workshop
                    {
                        ID = item.ID,
                        Title = item.Title + " - " + item.City.Title,
                        OwnerName = item.OwnerName,
                        OwnerFamily = item.OwnerFamily,
                        City = item.City
                    });
                }

            };
            ViewBag.Workshop = new SelectList(list, "ID", "Title");
            return View();
        }
        //
        public ActionResult CertificateQR(string QRCodeText)
        {
            List<QRCodeModel> qrcode = new List<QRCodeModel>();

            try
            {
                string folderPath = "~/UploadedFiles/QrCode/";
                string imagePath = "~/UploadedFiles/QrCode/" + QRCodeText + ".jpg";
                // If the directory doesn't exist then create it.
                if (!Directory.Exists(Server.MapPath(folderPath)))
                {
                    Directory.CreateDirectory(Server.MapPath(folderPath));
                }

                var barcodeWriter = new BarcodeWriter();
                barcodeWriter.Format = BarcodeFormat.QR_CODE;
                var result = barcodeWriter.Write(QRCodeText);

                string barcodePath = Server.MapPath(imagePath);
                qrcode.Add(new QRCodeModel
                {
                    QRCodeImagePath = GenerateQRCode(QRCodeText, "1"),
                    QRCodeCount="1",
                    QRCodeText=QRCodeText
                });
                

                ViewBag.qrcodeText = QRCodeText;
                ViewBag.Message = "QR Code  با موفقیت تولید شد.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "ساخت QR Code با خطا مواجه شد!";
                //catch exception if there is any
            }
            return PartialView(qrcode.ToList());
        }
        //
        public ActionResult CylinderQR(string QRCodeText)
        {
            List<QRCodeModel> qrcode = new List<QRCodeModel>();
            QRCodeText = QRCodeText.Replace("/", "_");
            try
            {
                string folderPath = "~/UploadedFiles/QrCode/";
                string imagePath = "~/UploadedFiles/QrCode/" + QRCodeText + ".jpg";
                // If the directory doesn't exist then create it.
                if (!Directory.Exists(Server.MapPath(folderPath)))
                {
                    Directory.CreateDirectory(Server.MapPath(folderPath));
                }

                var barcodeWriter = new BarcodeWriter();
                barcodeWriter.Format = BarcodeFormat.QR_CODE;
                var result = barcodeWriter.Write(QRCodeText);

                string barcodePath = Server.MapPath(imagePath);
                qrcode.Add(new QRCodeModel
                {
                    QRCodeImagePath = GenerateQRCode(QRCodeText, "1"),
                    QRCodeCount = "1",
                    QRCodeText = QRCodeText
                });


                ViewBag.qrcodeText = QRCodeText;
                ViewBag.Message = "QR Code  با موفقیت تولید شد.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "ساخت QR Code با خطا مواجه شد!";
                //catch exception if there is any
            }
            return PartialView(qrcode.ToList());
        }

        public ActionResult ValveQR(string QRCodeText)
        {
            List<QRCodeModel> qrcode = new List<QRCodeModel>();
            QRCodeText = QRCodeText.Replace("/", "_");
            try
            {
                string folderPath = "~/UploadedFiles/QrCode/";
                string imagePath = "~/UploadedFiles/QrCode/" + QRCodeText + ".jpg";
                // If the directory doesn't exist then create it.
                if (!Directory.Exists(Server.MapPath(folderPath)))
                {
                    Directory.CreateDirectory(Server.MapPath(folderPath));
                }

                var barcodeWriter = new BarcodeWriter();
                barcodeWriter.Format = BarcodeFormat.QR_CODE;
                var result = barcodeWriter.Write(QRCodeText);

                string barcodePath = Server.MapPath(imagePath);
                qrcode.Add(new QRCodeModel
                {
                    QRCodeImagePath = GenerateQRCode(QRCodeText, "1"),
                    QRCodeCount = "1",
                    QRCodeText = QRCodeText
                });


                ViewBag.qrcodeText = QRCodeText;
                ViewBag.Message = "QR Code  با موفقیت تولید شد.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "ساخت QR Code با خطا مواجه شد!";
                //catch exception if there is any
            }
            return PartialView(qrcode.ToList());
        }

        public ActionResult RegulatorQR(string QRCodeText)
        {
            List<QRCodeModel> qrcode = new List<QRCodeModel>();
            QRCodeText = QRCodeText.Replace("/", "_");
            try
            {
                string folderPath = "~/UploadedFiles/QrCode/";
                string imagePath = "~/UploadedFiles/QrCode/" + QRCodeText + ".jpg";
                // If the directory doesn't exist then create it.
                if (!Directory.Exists(Server.MapPath(folderPath)))
                {
                    Directory.CreateDirectory(Server.MapPath(folderPath));
                }

                var barcodeWriter = new BarcodeWriter();
                barcodeWriter.Format = BarcodeFormat.QR_CODE;
                var result = barcodeWriter.Write(QRCodeText);

                string barcodePath = Server.MapPath(imagePath);
                qrcode.Add(new QRCodeModel
                {
                    QRCodeImagePath = GenerateQRCode(QRCodeText, "1"),
                    QRCodeCount = "1",
                    QRCodeText = QRCodeText
                });


                ViewBag.qrcodeText = QRCodeText;
                ViewBag.Message = "QR Code  با موفقیت تولید شد.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "ساخت QR Code با خطا مواجه شد!";
                //catch exception if there is any
            }
            return PartialView(qrcode.ToList());
        }

        public ActionResult FillingValveQR(string QRCodeText)
        {
            List<QRCodeModel> qrcode = new List<QRCodeModel>();
            QRCodeText = QRCodeText.Replace("/", "_");
            try
            {
                string folderPath = "~/UploadedFiles/QrCode/";
                string imagePath = "~/UploadedFiles/QrCode/" + QRCodeText + ".jpg";
                // If the directory doesn't exist then create it.
                if (!Directory.Exists(Server.MapPath(folderPath)))
                {
                    Directory.CreateDirectory(Server.MapPath(folderPath));
                }

                var barcodeWriter = new BarcodeWriter();
                barcodeWriter.Format = BarcodeFormat.QR_CODE;
                var result = barcodeWriter.Write(QRCodeText);

                string barcodePath = Server.MapPath(imagePath);
                qrcode.Add(new QRCodeModel
                {
                    QRCodeImagePath = GenerateQRCode(QRCodeText, "1"),
                    QRCodeCount = "1",
                    QRCodeText = QRCodeText
                });


                ViewBag.qrcodeText = QRCodeText;
                ViewBag.Message = "QR Code  با موفقیت تولید شد.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "ساخت QR Code با خطا مواجه شد!";
                //catch exception if there is any
            }
            return PartialView(qrcode.ToList());
        }

        public ActionResult CutoffValveQR(string QRCodeText)
        {
            List<QRCodeModel> qrcode = new List<QRCodeModel>();
            QRCodeText = QRCodeText.Replace("/", "_");
            try
            {
                string folderPath = "~/UploadedFiles/QrCode/";
                string imagePath = "~/UploadedFiles/QrCode/" + QRCodeText + ".jpg";
                // If the directory doesn't exist then create it.
                if (!Directory.Exists(Server.MapPath(folderPath)))
                {
                    Directory.CreateDirectory(Server.MapPath(folderPath));
                }

                var barcodeWriter = new BarcodeWriter();
                barcodeWriter.Format = BarcodeFormat.QR_CODE;
                var result = barcodeWriter.Write(QRCodeText);

                string barcodePath = Server.MapPath(imagePath);
                qrcode.Add(new QRCodeModel
                {
                    QRCodeImagePath = GenerateQRCode(QRCodeText, "1"),
                    QRCodeCount = "1",
                    QRCodeText = QRCodeText
                });


                ViewBag.qrcodeText = QRCodeText;
                ViewBag.Message = "QR Code  با موفقیت تولید شد.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "ساخت QR Code با خطا مواجه شد!";
                //catch exception if there is any
            }
            return PartialView(qrcode.ToList());
        }

        public ActionResult FuelRelayQR(string QRCodeText)
        {
            List<QRCodeModel> qrcode = new List<QRCodeModel>();
            QRCodeText = QRCodeText.Replace("/", "_");
            try
            {
                string folderPath = "~/UploadedFiles/QrCode/";
                string imagePath = "~/UploadedFiles/QrCode/" + QRCodeText + ".jpg";
                // If the directory doesn't exist then create it.
                if (!Directory.Exists(Server.MapPath(folderPath)))
                {
                    Directory.CreateDirectory(Server.MapPath(folderPath));
                }

                var barcodeWriter = new BarcodeWriter();
                barcodeWriter.Format = BarcodeFormat.QR_CODE;
                var result = barcodeWriter.Write(QRCodeText);

                string barcodePath = Server.MapPath(imagePath);
                qrcode.Add(new QRCodeModel
                {
                    QRCodeImagePath = GenerateQRCode(QRCodeText, "1"),
                    QRCodeCount = "1",
                    QRCodeText = QRCodeText
                });


                ViewBag.qrcodeText = QRCodeText;
                ViewBag.Message = "QR Code  با موفقیت تولید شد.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "ساخت QR Code با خطا مواجه شد!";
                //catch exception if there is any
            }
            return PartialView(qrcode.ToList());
        }

        public ActionResult GasECUQR(string QRCodeText)
        {
            List<QRCodeModel> qrcode = new List<QRCodeModel>();
            QRCodeText = QRCodeText.Replace("/", "_");
            try
            {
                string folderPath = "~/UploadedFiles/QrCode/";
                string imagePath = "~/UploadedFiles/QrCode/" + QRCodeText + ".jpg";
                // If the directory doesn't exist then create it.
                if (!Directory.Exists(Server.MapPath(folderPath)))
                {
                    Directory.CreateDirectory(Server.MapPath(folderPath));
                }

                var barcodeWriter = new BarcodeWriter();
                barcodeWriter.Format = BarcodeFormat.QR_CODE;
                var result = barcodeWriter.Write(QRCodeText);

                string barcodePath = Server.MapPath(imagePath);
                qrcode.Add(new QRCodeModel
                {
                    QRCodeImagePath = GenerateQRCode(QRCodeText, "1"),
                    QRCodeCount = "1",
                    QRCodeText = QRCodeText
                });


                ViewBag.qrcodeText = QRCodeText;
                ViewBag.Message = "QR Code  با موفقیت تولید شد.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "ساخت QR Code با خطا مواجه شد!";
                //catch exception if there is any
            }
            return PartialView(qrcode.ToList());
        }
        //
        public ActionResult QRGenarator()
        {
            return View();
        }
        //
        [HttpPost]
        public ActionResult Generate(Models.QRCodeModel qrcode)
        {
            try
            {
                //var fillingValve = db.tbl_FillingValves.OrderByDescending(f => f.ID).Take(20).ToList();
                
                ///foreach (var item in fillingValve)
                //{
                    //
                    string folderPath = "~/UploadedFiles/QrCode/";
                    //string imagePath = "~/UploadedFiles/QrCode/" + item.ID + ".jpg";
                    string imagePath = "~/UploadedFiles/QrCode/" + qrcode.QRCodeText + ".jpg";
                    // If the directory doesn't exist then create it.
                    if (!Directory.Exists(Server.MapPath(folderPath)))
                    {
                        Directory.CreateDirectory(Server.MapPath(folderPath));
                    }

                    var barcodeWriter = new BarcodeWriter();
                    barcodeWriter.Format = BarcodeFormat.QR_CODE;
                    //var result = barcodeWriter.Write(item.serialNumber);
                    var result = barcodeWriter.Write(qrcode.QRCodeText);

                    string barcodePath = Server.MapPath(imagePath);
                    //
                    //item.QRCodeText = item.ID.ToString()+".jpg";
                    //db.Entry(item).State = EntityState.Modified;
                    //db.SaveChanges();
                    //qrcode.QRCodeImagePath = GenerateQRCode(item.serialNumber, item.ID.ToString());
                    qrcode.QRCodeImagePath = GenerateQRCode(qrcode.QRCodeText, qrcode.QRCodeCount);
                //}
                ViewBag.qrcodeText = qrcode.QRCodeText;
                ViewBag.Message = "QR Code  با موفقیت تولید شد.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "ساخت QR Code با خطا مواجه شد!";
                //catch exception if there is any
            }
            return View("QRGenarator", qrcode);
        }

        private string GenerateQRCode(string qrcodeText,string qrcodeCount)
        {
            string folderPath = "~/UploadedFiles/QrCode/";
            //string imagePath = "~/UploadedFiles/QrCode/"+qrcodeCount+".jpg";
            string imagePath = "~/UploadedFiles/QrCode/" + qrcodeText + ".jpg";
            // If the directory doesn't exist then create it.
            if (!Directory.Exists(Server.MapPath(folderPath)))
            {
                Directory.CreateDirectory(Server.MapPath(folderPath));
            }

            var barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            var result = barcodeWriter.Write(qrcodeText);

            string barcodePath = Server.MapPath(imagePath);
            var barcodeBitmap = new Bitmap(result);
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(barcodePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            return imagePath;

        }
        //
        public ActionResult Read(string qrcodeText)
        {
            ViewBag.qrcodeText = qrcodeText;
            return View(ReadQRCode(qrcodeText));
        }

        private QRCodeModel ReadQRCode(string qrcodeText)
        {
            QRCodeModel barcodeModel = new QRCodeModel();
            
            string barcodeText = "";
            string imagePath = "~/UploadedFiles/QrCode/"+ qrcodeText + ".jpg";
            string barcodePath = Server.MapPath(imagePath);
            var barcodeReader = new BarcodeReader();

            var result = barcodeReader.Decode(new Bitmap(barcodePath));
            if (result != null)
            {
                barcodeText = result.Text;
            }
            //byte[] bytes = Encoding.UTF8.GetBytes(result.Text);
            //barcodeText = Encoding.Default.GetString(bytes);

            return new QRCodeModel() { QRCodeText = barcodeText, QRCodeImagePath = imagePath };
        }
        //
        public ActionResult QRCodeList(string qrcodeText)
        {
            ViewBag.qrcodeText = qrcodeText;
            QRCodeModel barcodeModel = new QRCodeModel();
            List<QRCodeModel> tableOut = new List<QRCodeModel>();
            //var fillingValve = db.tbl_FillingValves.OrderByDescending(f => f.ID).Take(20).ToList();

            //foreach (var item in fillingValve)
            //{
                string barcodeText = "";
               // string imagePath = "~/UploadedFiles/QrCode/" + item.QRCodeText;
                string imagePath = "~/UploadedFiles/QrCode/" + qrcodeText + ".jpg";
                string barcodePath = Server.MapPath(imagePath);
                var barcodeReader = new BarcodeReader();

                var result = barcodeReader.Decode(new Bitmap(barcodePath));
                if (result != null)
                {
                    barcodeText = result.Text;
                }

                tableOut.Add(new QRCodeModel
                {
                    QRCodeText = barcodeText,
                    QRCodeImagePath = imagePath
                });
            //}
            //ViewBag.tableOut = tableOut.ToList();
            return View(tableOut.ToList());
        }
        //
        public ActionResult PrintQRCodeList(string qrcodeText)
        {
            string footer = "--footer-center \"Printed on: " + DateTime.Now.Date.ToString("yyyy/MM/dd") + "  Page: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            return new ActionAsPdf("QRCodeList",new { qrcodeText= qrcodeText })
            {
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = footer
                
            };
            //return report;
        }
        //
        public ActionResult ShowData(string barcodeText)
        {
            QRCodeModel barcodeModel = new QRCodeModel();
            List<FillingValve> tableOut = new List<FillingValve>();
            var fillingValve = db.tbl_FillingValves.Where(f=>f.serialNumber.Equals(barcodeText));
            ViewBag.imagePath = "~/UploadedFiles/QrCode/" + barcodeText + ".jpg";
            ViewBag.barcodeText = barcodeText;

            foreach (var item in fillingValve)
            {
                //string imagePath = "~/UploadedFiles/QrCode/" + item.QRCodeText;
                string imagePath = "~/UploadedFiles/QrCode/" + barcodeText + ".jpg";
                string barcodePath = Server.MapPath(imagePath);
                var barcodeReader = new BarcodeReader();

                var result = barcodeReader.Decode(new Bitmap(barcodePath));
                if (result != null)
                {
                    barcodeText = result.Text;
                }
                //
                tableOut.Add(new FillingValve
                {
                    constractor=item.constractor,
                    CreateDate=item.CreateDate,
                    model=item.model,
                    productDate=item.productDate,
                    serialNumber=item.serialNumber,
                    status=item.status,
                    workshop=item.workshop,
                    QRCodeText=imagePath
                });
            }

            return View(tableOut.ToList());
        }
        //
        public class DoubledTankItems
        {
            public string serialNumber { get; set; }
        }
        //
        [HttpPost]
        public ActionResult AddQRs(string[] serialNumber, string Workshop, string MaterialName)
        {
            List<DoubledTankItems> doubletankitems = new List<DoubledTankItems>();
            Tank cylinder = new Tank();
            TankValve valve = new TankValve();
            Kit regulator = new Kit();
            FillingValve fillingvalve = new FillingValve();
            CutofValve cutoffvalve = new CutofValve();
            FuelRelay fuelrelay = new FuelRelay();
            GasECU gasecu = new GasECU();

            // برای ذخیره اطلاعات قطعات از نوع مخزن
            if (MaterialName.Equals("مخزن"))
            {
                //    Message = "حتما یک ردیف باید پر شود!";
                if (serialNumber != null)
                {
                    //foreach (var item in serialNumber)
                    for (int i = 0; i < serialNumber.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(serialNumber[i]))
                        {
                            var serialId = serialNumber[i].Replace(" ", "");
                            //برای مواردی که در بانک اطلاعاتی سریال وجود دارد
                            var isExist = db.tbl_Tanks.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).Count();
                            var isExistBank = db.tbl_BankTanks.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).Count();

                            if (isExistBank > 0)
                            {
                                if (isExist == 0)
                                {
                                    var ExistBank = db.tbl_BankTanks.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).FirstOrDefault();
                                    cylinder.model = ExistBank.model;
                                    cylinder.constractor = ExistBank.constractor;
                                    cylinder.serialNumber = ExistBank.serialNumber.ToUpper();
                                    cylinder.status = "تایید شده";
                                    cylinder.workshop = Workshop;//Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                    cylinder.productDate = ExistBank.productDate;
                                    cylinder.expireDate = ExistBank.expireDate;
                                    cylinder.CreateDate = DateTime.Now;
                                    cylinder.Creator = User.Identity.Name;
                                    cylinder.MaterailName = "مخزن";

                                    db.tbl_Tanks.Add(cylinder);
                                    db.SaveChanges();
                                }
                            }
                            else
                            {
                                if (isExist == 0)
                                {

                                    cylinder.serialNumber = serialNumber[i].ToUpper();
                                    cylinder.status = "نیاز به بررسی";
                                    cylinder.workshop = Workshop;//Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                    cylinder.productDate = "";
                                    cylinder.expireDate = "";
                                    cylinder.CreateDate = DateTime.Now;
                                    cylinder.Creator = User.Identity.Name;
                                    cylinder.MaterailName = "مخزن";

                                    db.tbl_Tanks.Add(cylinder);
                                    db.SaveChanges();
                                    //countRow += 1;
                                }
                            }
                        }
                        else
                        {
                            //Message = "سریال وارد شده صحیح نمی باشد!";
                        }
                    }
                    return RedirectToAction("Index");
                    //countRow += 1;
                    //Message = "تعداد " + countRow + "با موفقیت ثبت شد";
                }

            }
            
            // برای ذخیره اطلاعات قطعات از نوع شیر مخزن
            if (MaterialName.Equals("شیر مخزن"))
            {
                //    Message = "حتما یک ردیف باید پر شود!";
                if (serialNumber != null)
                {
                    //foreach (var item in serialNumber)
                    for (int i = 0; i < serialNumber.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(serialNumber[i]))
                        {
                            var serialId = serialNumber[i].Replace(" ", "");
                            //برای مواردی که در بانک اطلاعاتی سریال وجود دارد
                            var isExist = db.tbl_TankValves.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).Count();
                            var isExistBank = db.tbl_BankTankValves.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).Count();

                            if (isExistBank > 0)
                            {
                                if (isExist == 0)
                                {
                                    var ExistBank = db.tbl_BankTankValves.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).FirstOrDefault();
                                    valve.model = ExistBank.model;
                                    valve.constractor = ExistBank.constractor;
                                    valve.serialNumber = ExistBank.serialNumber.ToUpper();
                                    valve.status = "تایید شده";
                                    valve.workshop = Workshop;//Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                    valve.productDate = ExistBank.productDate;
                                    valve.CreateDate = DateTime.Now;
                                    valve.Creator = User.Identity.Name;
                                    valve.MaterailName = "شیر مخزن";

                                    db.tbl_TankValves.Add(valve);
                                    db.SaveChanges();
                                }
                            }
                            else
                            {
                                if (isExist == 0)
                                {

                                    valve.serialNumber = serialNumber[i].ToUpper();
                                    valve.status = "نیاز به بررسی";
                                    valve.workshop = Workshop;//Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                    valve.productDate = "";
                                    valve.CreateDate = DateTime.Now;
                                    valve.Creator = User.Identity.Name;
                                    valve.MaterailName = "شیر مخزن";

                                    db.tbl_TankValves.Add(valve);
                                    db.SaveChanges();
                                    //countRow += 1;
                                }
                            }
                        }
                        else
                        {
                            //Message = "سریال وارد شده صحیح نمی باشد!";
                        }
                    }
                    return RedirectToAction("Index");
                    //countRow += 1;
                    //Message = "تعداد " + countRow + "با موفقیت ثبت شد";
                }

            }

            // برای ذخیره اطلاعات قطعات از نوع رگلاتور
            if (MaterialName.Equals("رگلاتور"))
            {
                //    Message = "حتما یک ردیف باید پر شود!";
                if (serialNumber != null)
                {
                    //foreach (var item in serialNumber)
                    for (int i = 0; i < serialNumber.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(serialNumber[i]))
                        {
                            var serialId = serialNumber[i].Replace(" ", "");
                            //برای مواردی که در بانک اطلاعاتی سریال وجود دارد
                            var isExist = db.tbl_Kits.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).Count();
                            var isExistBank = db.tbl_BankKits.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).Count();

                            if (isExistBank > 0)
                            {
                                if (isExist == 0)
                                {
                                    var ExistBank = db.tbl_BankKits.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).FirstOrDefault();
                                    regulator.model = ExistBank.model;
                                    regulator.constractor = ExistBank.constractor;
                                    regulator.serialNumber = ExistBank.serialNumber.ToUpper();
                                    regulator.status = "تایید شده";
                                    regulator.workshop = Workshop;//Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                    regulator.productDate = ExistBank.productDate;
                                    regulator.CreateDate = DateTime.Now;
                                    regulator.Creator = User.Identity.Name;
                                    regulator.MaterailName = "شیر مخزن";

                                    db.tbl_Kits.Add(regulator);
                                    db.SaveChanges();
                                }
                            }
                            else
                            {
                                if (isExist == 0)
                                {

                                    regulator.serialNumber = serialNumber[i].ToUpper();
                                    regulator.status = "نیاز به بررسی";
                                    regulator.workshop = Workshop;//Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                    regulator.productDate = "";
                                    regulator.CreateDate = DateTime.Now;
                                    regulator.Creator = User.Identity.Name;
                                    regulator.MaterailName = "شیر مخزن";

                                    db.tbl_Kits.Add(regulator);
                                    db.SaveChanges();
                                    //countRow += 1;
                                }
                            }
                        }
                        else
                        {
                            //Message = "سریال وارد شده صحیح نمی باشد!";
                        }
                    }
                    return RedirectToAction("Index");
                    //countRow += 1;
                    //Message = "تعداد " + countRow + "با موفقیت ثبت شد";
                }

            }

            // برای ذخیره اطلاعات قطعات از نوع شیر پر کن
            if (MaterialName.Equals("شیر پر کن"))
            {
                //    Message = "حتما یک ردیف باید پر شود!";
                if (serialNumber != null)
                {
                    //foreach (var item in serialNumber)
                    for (int i = 0; i < serialNumber.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(serialNumber[i]))
                        {
                            var serialId = serialNumber[i].Replace(" ", "");
                            //برای مواردی که در بانک اطلاعاتی سریال وجود دارد
                            var isExist = db.tbl_FillingValves.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).Count();
                            var isExistBank = db.tbl_BankFillingValves.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).Count();

                            if (isExistBank > 0)
                            {
                                if (isExist == 0)
                                {
                                    var ExistBank = db.tbl_BankFillingValves.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).FirstOrDefault();
                                    fillingvalve.model = ExistBank.model;
                                    fillingvalve.constractor = ExistBank.constractor;
                                    fillingvalve.serialNumber = ExistBank.serialNumber.ToUpper();
                                    fillingvalve.status = "تایید شده";
                                    fillingvalve.workshop = Workshop;//Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                    fillingvalve.productDate = ExistBank.productDate;
                                    fillingvalve.CreateDate = DateTime.Now;
                                    fillingvalve.Creator = User.Identity.Name;
                                    fillingvalve.MaterailName = "شیر پرکن";

                                    db.tbl_FillingValves.Add(fillingvalve);
                                    db.SaveChanges();
                                }
                            }
                            else
                            {
                                if (isExist == 0)
                                {

                                    fillingvalve.serialNumber = serialNumber[i].ToUpper();
                                    fillingvalve.status = "نیاز به بررسی";
                                    fillingvalve.workshop = Workshop;//Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                    fillingvalve.productDate = "";
                                    fillingvalve.CreateDate = DateTime.Now;
                                    fillingvalve.Creator = User.Identity.Name;
                                    fillingvalve.MaterailName = "شیر پرکن";

                                    db.tbl_FillingValves.Add(fillingvalve);
                                    db.SaveChanges();
                                    //countRow += 1;
                                }
                            }
                        }
                        else
                        {
                            //Message = "سریال وارد شده صحیح نمی باشد!";
                        }
                    }
                    return RedirectToAction("Index");
                    //countRow += 1;
                    //Message = "تعداد " + countRow + "با موفقیت ثبت شد";
                }

            }

            // برای ذخیره اطلاعات قطعات از نوع شیر قطع کن
            if (MaterialName.Equals("شیر قطع کن"))
            {
                //    Message = "حتما یک ردیف باید پر شود!";
                if (serialNumber != null)
                {
                    //foreach (var item in serialNumber)
                    for (int i = 0; i < serialNumber.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(serialNumber[i]))
                        {
                            var serialId = serialNumber[i].Replace(" ", "");
                            //برای مواردی که در بانک اطلاعاتی سریال وجود دارد
                            var isExist = db.tbl_CutofValves.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).Count();
                            var isExistBank = db.tbl_BankCutofValves.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).Count();

                            if (isExistBank > 0)
                            {
                                if (isExist == 0)
                                {
                                    var ExistBank = db.tbl_BankCutofValves.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).FirstOrDefault();
                                    cutoffvalve.model = ExistBank.model;
                                    cutoffvalve.constractor = ExistBank.constractor;
                                    cutoffvalve.serialNumber = ExistBank.serialNumber.ToUpper();
                                    cutoffvalve.status = "تایید شده";
                                    cutoffvalve.workshop = Workshop;//Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                    cutoffvalve.productDate = ExistBank.productDate;
                                    cutoffvalve.CreateDate = DateTime.Now;
                                    cutoffvalve.Creator = User.Identity.Name;
                                    cutoffvalve.MaterailName = "شیر قطع کن";

                                    db.tbl_CutofValves.Add(cutoffvalve);
                                    db.SaveChanges();
                                }
                            }
                            else
                            {
                                if (isExist == 0)
                                {

                                    cutoffvalve.serialNumber = serialNumber[i].ToUpper();
                                    cutoffvalve.status = "نیاز به بررسی";
                                    cutoffvalve.workshop = Workshop;//Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                    cutoffvalve.productDate = "";
                                    cutoffvalve.CreateDate = DateTime.Now;
                                    cutoffvalve.Creator = User.Identity.Name;
                                    cutoffvalve.MaterailName = "شیر قطع کن";

                                    db.tbl_CutofValves.Add(cutoffvalve);
                                    db.SaveChanges();
                                    //countRow += 1;
                                }
                            }
                        }
                        else
                        {
                            //Message = "سریال وارد شده صحیح نمی باشد!";
                        }
                    }
                    return RedirectToAction("Index");
                    //countRow += 1;
                    //Message = "تعداد " + countRow + "با موفقیت ثبت شد";
                }

            }

            // برای ذخیره اطلاعات قطعات از نوع رله سوخت
            if (MaterialName.Equals("رله سوخت"))
            {
                //    Message = "حتما یک ردیف باید پر شود!";
                if (serialNumber != null)
                {
                    //foreach (var item in serialNumber)
                    for (int i = 0; i < serialNumber.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(serialNumber[i]))
                        {
                            var serialId = serialNumber[i].Replace(" ", "");
                            //برای مواردی که در بانک اطلاعاتی سریال وجود دارد
                            var isExist = db.tbl_FuelRelays.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).Count();
                            var isExistBank = db.tbl_BankFuelRelays.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).Count();

                            if (isExistBank > 0)
                            {
                                if (isExist == 0)
                                {
                                    var ExistBank = db.tbl_BankFuelRelays.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).FirstOrDefault();
                                    fuelrelay.model = ExistBank.model;
                                    fuelrelay.constractor = ExistBank.constractor;
                                    fuelrelay.serialNumber = ExistBank.serialNumber.ToUpper();
                                    fuelrelay.status = "تایید شده";
                                    fuelrelay.workshop = Workshop;//Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                    fuelrelay.productDate = ExistBank.productDate;
                                    fuelrelay.expireDate = ExistBank.expireDate;
                                    fuelrelay.CreateDate = DateTime.Now;
                                    fuelrelay.Creator = User.Identity.Name;
                                    fuelrelay.MaterailName = "رله سوخت";

                                    db.tbl_FuelRelays.Add(fuelrelay);
                                    db.SaveChanges();
                                }
                            }
                            else
                            {
                                if (isExist == 0)
                                {

                                    fuelrelay.serialNumber = serialNumber[i].ToUpper();
                                    fuelrelay.status = "نیاز به بررسی";
                                    fuelrelay.workshop = Workshop;//Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                    fuelrelay.productDate = "";
                                    fuelrelay.expireDate = "";
                                    fuelrelay.CreateDate = DateTime.Now;
                                    fuelrelay.Creator = User.Identity.Name;
                                    fuelrelay.MaterailName = "رله سوخت";

                                    db.tbl_FuelRelays.Add(fuelrelay);
                                    db.SaveChanges();
                                    //countRow += 1;
                                }
                            }
                        }
                        else
                        {
                            //Message = "سریال وارد شده صحیح نمی باشد!";
                        }
                    }
                    return RedirectToAction("Index");
                    //countRow += 1;
                    //Message = "تعداد " + countRow + "با موفقیت ثبت شد";
                }

            }

            // برای ذخیره اطلاعات قطعات از نوع رله سوخت
            if (MaterialName.Equals("Gas ECU"))
            {
                //    Message = "حتما یک ردیف باید پر شود!";
                if (serialNumber != null)
                {
                    //foreach (var item in serialNumber)
                    for (int i = 0; i < serialNumber.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(serialNumber[i]))
                        {
                            var serialId = serialNumber[i].Replace(" ", "");
                            //برای مواردی که در بانک اطلاعاتی سریال وجود دارد
                            var isExist = db.tbl_GasECU.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).Count();
                            var isExistBank = db.tbl_BankGasECU.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).Count();

                            if (isExistBank > 0)
                            {
                                if (isExist == 0)
                                {
                                    var ExistBank = db.tbl_BankGasECU.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).FirstOrDefault();
                                    gasecu.model = ExistBank.model;
                                    gasecu.constractor = ExistBank.constractor;
                                    gasecu.serialNumber = ExistBank.serialNumber.ToUpper();
                                    gasecu.status = "تایید شده";
                                    gasecu.workshop = Workshop;//Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                    gasecu.productDate = ExistBank.productDate;
                                    gasecu.expireDate = ExistBank.expireDate;
                                    gasecu.CreateDate = DateTime.Now;
                                    gasecu.Creator = User.Identity.Name;
                                    gasecu.MaterailName = "Gas ECU";

                                    db.tbl_GasECU.Add(gasecu);
                                    db.SaveChanges();
                                }
                            }
                            else
                            {
                                if (isExist == 0)
                                {

                                    gasecu.serialNumber = serialNumber[i].ToUpper();
                                    gasecu.status = "نیاز به بررسی";
                                    gasecu.workshop = Workshop;//Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                    gasecu.productDate = "";
                                    gasecu.expireDate = "";
                                    gasecu.CreateDate = DateTime.Now;
                                    gasecu.Creator = User.Identity.Name;
                                    gasecu.MaterailName = "Gas ECU";

                                    db.tbl_GasECU.Add(gasecu);
                                    db.SaveChanges();
                                    //countRow += 1;
                                }
                            }
                        }
                        else
                        {
                            //Message = "سریال وارد شده صحیح نمی باشد!";
                        }
                    }
                    return RedirectToAction("Index");
                    //countRow += 1;
                    //Message = "تعداد " + countRow + "با موفقیت ثبت شد";
                }

            }


            return View();
        }

        //
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //
    }
}