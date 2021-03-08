using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GaraManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace GaraManagement.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly GaraContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public DashBoardController(GaraContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            ViewData["CountCustomer"] = _context.Customers.Count();
            ViewData["CountRepair"] = _context.Repairs.Count();
            return View();
        }

        public IActionResult GetcountData()
        {
            Month countCar = new Month();
            countCar.Thang1 = _context.Repairs.Where(r => r.DateOfFactoryEntry >= DateTime.Parse("01/01/2021") && r.DateOfFactoryEntry < DateTime.Parse("01/02/2021")).Count();
            countCar.Thang2 = _context.Repairs.Where(r => r.DateOfFactoryEntry >= DateTime.Parse("01/02/2021") && r.DateOfFactoryEntry < DateTime.Parse("01/03/2021")).Count();
            countCar.Thang3 = _context.Repairs.Where(r => r.DateOfFactoryEntry >= DateTime.Parse("01/03/2021") && r.DateOfFactoryEntry < DateTime.Parse("01/04/2021")).Count();
            countCar.Thang4 = _context.Repairs.Where(r => r.DateOfFactoryEntry >= DateTime.Parse("01/04/2021") && r.DateOfFactoryEntry < DateTime.Parse("01/05/2021")).Count();
            countCar.Thang5 = _context.Repairs.Where(r => r.DateOfFactoryEntry >= DateTime.Parse("01/05/2021") && r.DateOfFactoryEntry < DateTime.Parse("01/06/2021")).Count();
            countCar.Thang6 = _context.Repairs.Where(r => r.DateOfFactoryEntry >= DateTime.Parse("01/06/2021") && r.DateOfFactoryEntry < DateTime.Parse("01/07/2021")).Count();
            countCar.Thang7 = _context.Repairs.Where(r => r.DateOfFactoryEntry >= DateTime.Parse("01/07/2021") && r.DateOfFactoryEntry < DateTime.Parse("01/08/2021")).Count();
            countCar.Thang8 = _context.Repairs.Where(r => r.DateOfFactoryEntry >= DateTime.Parse("01/08/2021") && r.DateOfFactoryEntry < DateTime.Parse("01/09/2021")).Count();
            countCar.Thang9 = _context.Repairs.Where(r => r.DateOfFactoryEntry >= DateTime.Parse("01/09/2021") && r.DateOfFactoryEntry < DateTime.Parse("01/10/2021")).Count();
            countCar.Thang10 = _context.Repairs.Where(r => r.DateOfFactoryEntry >= DateTime.Parse("01/10/2021") && r.DateOfFactoryEntry < DateTime.Parse("01/11/2021")).Count();
            countCar.Thang11 = _context.Repairs.Where(r => r.DateOfFactoryEntry >= DateTime.Parse("01/11/2021") && r.DateOfFactoryEntry < DateTime.Parse("01/12/2021")).Count();
            countCar.Thang12 = _context.Repairs.Where(r => r.DateOfFactoryEntry >= DateTime.Parse("01/12/2021") && r.DateOfFactoryEntry < DateTime.Parse("31/12/2021")).Count();
            //List<CountCar> listCountCar = new List<CountCar>();
            //for (int i = 1; i <= 12; i++)
            //{
            //    CountCar countCar = new CountCar();

            //    if (i == 12)
            //    {
            //        countCar.("Thang"+i) = _context.Repairs.Where(r => r.DateOfFactoryEntry >= DateTime.Parse("01/" + i + "/2021") && r.DateOfFactoryEntry <= DateTime.Parse("31/" + i + "/2021")).Count();
            //    }
            //    else
            //    {
            //        countCar.y = _context.Repairs.Where(r => r.DateOfFactoryEntry >= DateTime.Parse("01/" + i + "/2021") && r.DateOfFactoryEntry < DateTime.Parse("01/" + (i + 1) + "/2021")).Count();
            //    }

            //    listCountCar.Add(countCar);
            //}
            //var dataJson = JsonConvert.SerializeObject(listCountCar);
            //var dataJson = JsonConvert.SerializeObject(countCar);
            return Json(countCar);
        }

        public IActionResult GetRevenueData()
        {
            // tổng tiền nhập hàng theo tháng
            List<int?> arrayNhapHang = new List<int?>();
            
            for (int i=1; i<=12; i++)
            {
                int? tiennhaphang = 0;
                if (i==12)
                {
                    var nhaphangT12 = _context.GoodsReceivedNotes.Include(a => a.DetailGoodsReceivedNotes).Where(d => d.ImportDate >= DateTime.Parse("01/" + i + "/2021") && d.ImportDate < DateTime.Parse("31/" + i + "/2021")).ToList();
                    foreach (var item in nhaphangT12)
                    {
                        foreach (var item2 in item.DetailGoodsReceivedNotes)
                        {
                            tiennhaphang += (item2.Price * item2.Amount);
                        }
                    }
                }
                else {
                    var nhaphang = _context.GoodsReceivedNotes.Include(a => a.DetailGoodsReceivedNotes).Where(d => d.ImportDate >= DateTime.Parse("01/" + i + "/2021") && d.ImportDate < DateTime.Parse("01/" + (i + 1) + "/2021")).ToList();
                    foreach (var item in nhaphang)
                    {
                        foreach (var item2 in item.DetailGoodsReceivedNotes)
                        {
                            tiennhaphang += (item2.Price * item2.Amount);
                        }
                    }
                }
                arrayNhapHang.Add(tiennhaphang);
                
            }
            //tổng tiền bán hàng theo tháng
            List<int?> arrayBanHang = new List<int?>();

            for (int i = 1; i <= 12; i++)
            {
                int? tienbanhang = 0;
                if (i == 12)
                {
                    var banhangT12 = _context.GoodsDeliveryNotes.Include(a => a.DetailGoodsDeliveryNotes).Where(d => d.ExportDate >= DateTime.Parse("01/" + i + "/2021") && d.ExportDate < DateTime.Parse("31/" + i + "/2021")).ToList();
                    foreach (var item in banhangT12)
                    {
                        foreach (var item2 in item.DetailGoodsDeliveryNotes)
                        {
                            tienbanhang += (item2.Price * item2.Amount);
                        }
                    }
                }
                else
                {
                    var nhaphang = _context.GoodsDeliveryNotes.Include(a => a.DetailGoodsDeliveryNotes).Where(d => d.ExportDate >= DateTime.Parse("01/" + i + "/2021") && d.ExportDate < DateTime.Parse("01/" + (i + 1) + "/2021")).ToList();
                    foreach (var item in nhaphang)
                    {
                        foreach (var item2 in item.DetailGoodsDeliveryNotes)
                        {
                            tienbanhang += (item2.Price * item2.Amount);
                        }
                    }
                }
                arrayBanHang.Add(tienbanhang);
            }

            arrayBanHang.ToArray();
            arrayNhapHang.ToArray();
            //tổng tiền doanh thu bán hàng theo tháng
            List<int?> arrayDoanhThuBanHang = new List<int?>();
            
            for(int i = 0; i <= 11; i++)
            {
                arrayDoanhThuBanHang.Add(arrayBanHang[i] - arrayNhapHang[i]);
            }

            //tổng tiền chi phí thu được theo tháng
            List<int?> arraychiphi = new List<int?>();
            for (var i = 1; i<=12; i++)
            {
                int? chiphisua = 0;
                if(i == 12)
                {
                    var repairCost = _context.DetailRepairs.Include(dt => dt.IdRepairNavigation).ThenInclude(dt => dt.Pays).Where(dt => dt.IdRepairNavigation.Pays.First().DateOfPayment >= DateTime.Parse("01/" + i + "/2021") && dt.IdRepairNavigation.Pays.First().DateOfPayment < DateTime.Parse("31/" + i + "/2021"));
                    foreach (var item in repairCost)
                    {
                        chiphisua += item.Amount * item.IdWorkNavigation.Cost;
                    }
                }
                else
                {
                    var repairCost = _context.DetailRepairs.Include(dt => dt.IdWorkNavigation).Include(dt => dt.IdRepairNavigation).ThenInclude(dt => dt.Pays).Where(dt => dt.IdRepairNavigation.Pays.First().DateOfPayment >= DateTime.Parse("01/" + i + "/2021") && dt.IdRepairNavigation.Pays.First().DateOfPayment < DateTime.Parse("01/" + (i + 1) + "/2021"));
                    foreach(var item in repairCost)
                    {
                        chiphisua += item.Amount * item.IdWorkNavigation.Cost;
                    }    
                }

                arraychiphi.Add(chiphisua);
            }


            arraychiphi.ToArray();

            List<int?> arrayTongDoanhThu = new List<int?>();

            for (int i = 0; i <= 11; i++)
            {
                arrayTongDoanhThu.Add((arrayBanHang[i] + arraychiphi[i]));
            }
            List<int?> arrayLoiNhuanRong = new List<int?>();

            for (int i = 0; i <= 11; i++)
            {
                arrayLoiNhuanRong.Add((arrayTongDoanhThu[i] - arrayNhapHang[i]));
            }

            
            arrayLoiNhuanRong.ToArray();

            List<int?> arrayTongNo = new List<int?>();

            

            Doanhthu doanhThu = new Doanhthu();
            doanhThu.nhaphang = arrayNhapHang;
            doanhThu.banhang = arrayBanHang;
            doanhThu.doanhthuvattu = arrayDoanhThuBanHang;
            doanhThu.phisuachua = arraychiphi;
            doanhThu.tongdoanhthu = arrayTongDoanhThu;
            doanhThu.loinhuanrong = arrayLoiNhuanRong;
            

            return Json(doanhThu);
            
        }
        public IActionResult GetPayData()
        {
            List<int?> arrayPay = new List<int?>();
            for (var i = 1; i <= 12; i++)
            {
                int? pay = 0;
                if (i == 12)
                {
                    var paid = _context.Pays.Where(p => p.DateOfPayment >= DateTime.Parse("01/" + i + "/2021") && p.DateOfPayment < DateTime.Parse("31/" + i + "/2021"));
                    foreach (var item in paid)
                    {
                        pay += item.Paid;
                    }
                }
                else
                {
                    var paid = _context.Pays.Where(p => p.DateOfPayment >= DateTime.Parse("01/" + i + "/2021") && p.DateOfPayment < DateTime.Parse("01/" + (i + 1) + "/2021"));
                    foreach (var item in paid)
                    {
                        pay += item.Paid;
                    }
                }
                arrayPay.Add(pay);
            }
            arrayPay.ToArray();

            List<int?> arraytotal = new List<int?>();
            for (var i = 1; i <= 12; i++)
            {
                int? sumTotal = 0;
                if (i == 12)
                {
                    var paid = _context.Pays.Where(p => p.DateOfPayment >= DateTime.Parse("01/" + i + "/2021") && p.DateOfPayment < DateTime.Parse("31/" + i + "/2021"));
                    foreach (var item in paid)
                    {
                        sumTotal += item.Total;
                    }
                }
                else
                {
                    var paid = _context.Pays.Where(p => p.DateOfPayment >= DateTime.Parse("01/" + i + "/2021") && p.DateOfPayment < DateTime.Parse("01/" + (i + 1) + "/2021"));
                    foreach (var item in paid)
                    {
                        sumTotal += item.Total;
                    }
                }
                arraytotal.Add(sumTotal);
            }
            arraytotal.ToArray();
            List<int?> arrayowe = new List<int?>();
            for (int i = 0; i <= 11; i++)
            {
                arrayowe.Add(arraytotal[i] - arrayPay[i]);
            }
            arrayowe.ToArray();
            thanhtoan thanhtoan = new thanhtoan();
            thanhtoan.paid = arrayPay;
            thanhtoan.owe = arrayowe;
            return Json(thanhtoan);

        }
        public IActionResult GetServiceData()
        {
            //List<string> nameService = new List<string>();
            //var serviceNameData = _context.Services.ToList();
            //foreach(var item in serviceNameData)
            //{
            //    nameService.Add(item.Name);
            //}
            ////nameService.ToArray();

            //List<int?> count = new List<int?>();
            //for (int i = 0; i < nameService.Count(); i++)
            //{
            //    var serviceCountData = _context.DetailRepairs.Where(s => s.IdWorkNavigation.IdServiceNavigation.Name == nameService[i]).Count();
            //    count.Add(serviceCountData);
            //}
            List<Dichvu> dichvuList = new List<Dichvu>();
            List<int> countTyle = new List<int>();
            List<string> nameData = new List<string>();
            var serviceNameData = _context.Services.ToList();

            foreach (var item in serviceNameData)
            {
                var serviceCountData = _context.DetailRepairs.Where(s => s.IdWorkNavigation.IdServiceNavigation.Name == item.Name).Count();
                nameData.Add(item.Name);
                countTyle.Add(serviceCountData);
            }
            countTyle.ToArray();
            var tongTyLe = 0;
            for(var i = 0; i< countTyle.Count(); i++)
            {
                tongTyLe += countTyle[i];
            }

            for(var i = 0; i< countTyle.Count(); i++)
            {
                Dichvu dichvu = new Dichvu();
                dichvu.name = nameData[i];
                dichvu.data = (countTyle[i] /(float) tongTyLe) * 100;
                dichvuList.Add(dichvu);
            }    
            return Json(dichvuList);
        }
        public class thanhtoan
        {
            public IEnumerable<int?> paid { get; set; }
            public IEnumerable<int?> owe { get; set; }
        }
        public class Dichvu
        {
            public string name { get; set; }
            public float data { get; set; } 
        }
        public class Doanhthu
        {
            public IEnumerable<int?> banhang { get; set; }
            public IEnumerable<int?> nhaphang { get; set; }
            public IEnumerable<int?> doanhthuvattu { get; set; }
            public IEnumerable<int?> phisuachua { get; set; }
            public IEnumerable<int?> tongdoanhthu { get; set; }
            public IEnumerable<int?> loinhuanrong { get; set; }

        }
        public class Month
        {
            public int Thang1 { get; set; }
            public int Thang2 { get; set; }
            public int Thang3 { get; set; }
            public int Thang4 { get; set; }
            public int Thang5 { get; set; }
            public int Thang6 { get; set; }
            public int Thang7 { get; set; }
            public int Thang8 { get; set; }
            public int Thang9 { get; set; }
            public int Thang10 { get; set; }
            public int Thang11 { get; set; }
            public int Thang12 { get; set; }
            
        }
    }
}
