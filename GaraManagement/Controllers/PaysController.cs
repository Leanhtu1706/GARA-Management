using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GaraManagement.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace GaraManagement.Controllers
{
    public class PaysController : Controller
    {
        private readonly GaraContext _context;

        public PaysController(GaraContext context)
        {
            _context = context;
        }

        // GET: Pays
        public async Task<IActionResult> Index(string search)
        {
            if (HttpContext.Session.GetString("SessionUserName") == null || HttpContext.Session.GetString("PermissionAdmin") != "Yes")
            {
                if (HttpContext.Session.GetString("PermissionCoVan") != "Yes")
                {
                    return RedirectToAction("Index", "Login");
                }
            }

            if (HttpContext.Session.GetString("SuccessMessage") != null)
            {
                ViewBag.SuccessMessage = HttpContext.Session.GetString("SuccessMessage");
                HttpContext.Session.Remove("SuccessMessage");
            }
            ViewData["GetTextSearch"] = search;
            if (!string.IsNullOrEmpty(search))
            {
                var garaContexts = _context.Pays.Include(p => p.IdRepairNavigation).ThenInclude(p => p.IdCarNavigation).ThenInclude(p => p.IdCustomerNavigation).Where(p => p.IdRepair == Convert.ToInt32(search));
                return View(garaContexts.ToList());
            }
            else
            {
                var garaContext = _context.Pays.Include(p => p.IdRepairNavigation);
                return View(await garaContext.ToListAsync());
            }
            
        }

        // GET: Pays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pay = await _context.Pays
                .Include(p => p.IdRepairNavigation)
                .Include(r => r.IdRepairNavigation.IdCarNavigation)
                .ThenInclude(r => r.IdCustomerNavigation)
                .Include(r => r.IdRepairNavigation.GoodsDeliveryNotes)
                .ThenInclude(r => r.DetailGoodsDeliveryNotes)
                .ThenInclude(r => r.IdMaterialNavigation)
                .ThenInclude(r => r.PriceMaterials)
                .Include(r => r.IdRepairNavigation.DetailRepairs)
                .ThenInclude(r => r.IdWorkNavigation)
                .Include(r => r.IdRepairNavigation.IdCarNavigation.IdCarModelNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pay == null)
            {
                return NotFound();
            }

            return View(pay);
        }

        // GET: Pays/Create
        public IActionResult Create(string layout = "_")
        {
            ViewData["IdRepair"] = new SelectList(_context.Repairs, "Id","Id");
            ViewData["Layout"] = layout == "_" ? "" : layout;
            return View();
        }

        // POST: Pays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Pay pay)
        {
            if (ModelState.IsValid)
            {   
                int ? workCost = 0;
                var workCostData = await _context.DetailRepairs.Include(r => r.IdWorkNavigation).Where(r =>r.IdRepair == pay.IdRepair).ToListAsync();
                if (workCostData.Any())
                {

                    foreach (var item in workCostData)
                    {
                        workCost += item.IdWorkNavigation.Cost * item.Amount;

                    }

                }
                int ? materialCost = 0;
                var materialCostData = await _context.GoodsDeliveryNotes.Include(r => r.DetailGoodsDeliveryNotes).Where(r =>r.IdRepair == pay.IdRepair).FirstOrDefaultAsync();
                if (materialCostData != null)
                {

                    foreach (var item in materialCostData.DetailGoodsDeliveryNotes)
                    {
                        materialCost += item.Price * item.Amount;

                    }

                }
                pay.Total = ((workCost + materialCost)+((workCost + materialCost) * 10) / 100);
                _context.Add(pay);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("SuccessMessage", "Thêm mới biên lai thành công");

                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRepair"] = new SelectList(_context.Repairs, "Id", "Id", pay.IdRepair);
            return View(pay);
        }

        // GET: Pays/Edit/5
        public async Task<IActionResult> Edit(int? idRepair, string layout = "_")
        {
            if (idRepair == null)
            {
                return NotFound();
            }

            var pay = await _context.Pays.Where(p => p.IdRepair == idRepair).FirstOrDefaultAsync();
            if (pay == null)
            {
                return NotFound();
            }
            ViewData["Layout"] = layout == "_" ? "" : layout;
            return View(pay);
        }

        // POST: Pays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pay pay)
        {
            if (id != pay.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    pay.Paid += pay.owe;
                    pay.Update_at = DateTime.Now;
                    _context.Update(pay);
                    HttpContext.Session.SetString("SuccessMessage", "Cập nhật thành công");

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PayExists(pay.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRepair"] = new SelectList(_context.Repairs, "Id", "Id", pay.IdRepair);
            return View(pay);
        }

        // GET: Pays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pay = await _context.Pays
                .Include(p => p.IdRepairNavigation)
                .Include(r => r.IdRepairNavigation.IdCarNavigation)
                .ThenInclude(r => r.IdCustomerNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pay == null)
            {
                return NotFound();
            }

            return Json(id);
            
        }

        // POST: Pays/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pay = await _context.Pays.FindAsync(id);
            _context.Pays.Remove(pay);
            await _context.SaveChangesAsync();
            HttpContext.Session.SetString("SuccessMessage", "Xóa thành công");

            return RedirectToAction(nameof(Index));
        }

        private bool PayExists(int id)
        {
            return _context.Pays.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Export(int id)
        {
            var pay = await _context.Repairs
                .Include(r => r.IdCarNavigation)
                .ThenInclude(r => r.IdCustomerNavigation)
                .Include(r => r.GoodsDeliveryNotes)
                .ThenInclude(r => r.DetailGoodsDeliveryNotes)
                .ThenInclude(r => r.IdMaterialNavigation)
                .ThenInclude(r => r.PriceMaterials)
                .Include(r => r.DetailRepairs)
                .ThenInclude(r => r.IdWorkNavigation)
                .Include(r => r.Pays)
                .Include(r => r.IdCarNavigation.IdCarModelNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);


            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var sheet = package.Workbook.Worksheets.Add("Biên lai");


                sheet.Cells["C2"].Value = "Biên lai thu tiền";

                sheet.Cells["A4"].Value = "Tên công ty:";
                sheet.Cells["A5"].Value = "Mã Số thuế:";
                sheet.Cells["A6"].Value = "Mã phiếu sửa:   " + pay.Id;
                sheet.Cells["A7"].Value = "Tên Khách hàng:   " + pay.IdCarNavigation.IdCustomerNavigation.Name;
                sheet.Cells["A8"].Value = "Số điện thoại:   " + pay.IdCarNavigation.IdCustomerNavigation.Phone;
                sheet.Cells["A9"].Value = "Địa chỉ:   " + pay.IdCarNavigation.IdCustomerNavigation.Address;
                sheet.Cells["A10"].Value = "Số CMND:   " + pay.IdCarNavigation.IdCustomerNavigation.IdentityCardNumber;

                sheet.Cells["D6"].Value = "Thời gian: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                sheet.Cells["D7"].Value = "Biển số:"; sheet.Cells["E7"].Value = pay.IdCarNavigation.LicensePlates;
                sheet.Cells["D8"].Value = "Dòng xe:"; sheet.Cells["E8"].Value = pay.IdCarNavigation.IdCarModelNavigation.ModelName;
                sheet.Cells["D9"].Value = "Màu:"; sheet.Cells["E9"].Value = pay.IdCarNavigation.Color;
                sheet.Cells["D10"].Value = "Ghi chú:"; sheet.Cells["E10"].Value = pay.IdCarNavigation.Note;
                sheet.Cells["A11"].Value = "A. Chi tiết tiền vật tư:";
                sheet.Cells["A12"].Value = "Tên vật tư";
                sheet.Cells["B12"].Value = "Đv tính";
                sheet.Cells["C12"].Value = "Số lượng";
                sheet.Cells["D12"].Value = "Đơn giá";
                sheet.Cells["E12"].Value = "thành tiền";
                sheet.Cells[ 12, 5, 12 , 6].Merge = true;

                using (ExcelRange exr = sheet.Cells[7, 1, 7, 6])
                {
                    exr.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                }
                using (ExcelRange exr = sheet.Cells[10, 1, 10, 6])
                {
                    exr.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }
                using (ExcelRange exr = sheet.Cells[7, 6, 10, 6])
                {
                    exr.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                }
                using (ExcelRange exr = sheet.Cells[7, 1, 10, 1])
                {
                    exr.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                }

                //Set style
                sheet.Cells["C2,D6,A4,A5,A6,A11,A12,B12,C12,D12,E12"].Style.Font.Bold = true;
                sheet.Cells["C2"].Style.Font.Size = 14;

                //định dạng money
                var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
                //Show danh sách vật tư
                var rowNumber = 13;
                int? tongTienVatTu = 0;
                if (pay.GoodsDeliveryNotes.Count() != 0)
                {
                    foreach (var item in pay.GoodsDeliveryNotes.Where(r => r.IdRepair == id).FirstOrDefault().DetailGoodsDeliveryNotes)
                    {
                        sheet.Cells[rowNumber, 5, rowNumber, 6].Merge = true;
                        sheet.Cells["A" + rowNumber].Value = item.IdMaterialNavigation.Name;
                        sheet.Cells["B" + rowNumber].Value = item.IdMaterialNavigation.Unit;
                        sheet.Cells["C" + rowNumber].Value = item.Amount;
                        sheet.Cells["D" + rowNumber].Value = String.Format(info, "{0:c}", item.Price);
                        sheet.Cells["E" + rowNumber].Value = String.Format(info, "{0:c}", item.Amount * item.Price);
                        tongTienVatTu += item.Amount * item.Price;
                        rowNumber++;
                    }
                }
                using (ExcelRange exr = sheet.Cells[12, 1, rowNumber - 1, 6])
                {
                    //exr.AutoFitColumns();
                    exr.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    exr.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    exr.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    exr.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                }
                // show danh sách tiền công
                var rowNumberWork = rowNumber + 2;
                int? tongTienCong = 0;
                sheet.Cells["A" + rowNumber].Value = "B. Chi tiết tiền công:";
                sheet.Cells["A" + (rowNumber + 1)].Value = "Tên công việc";
                sheet.Cells["B" + (rowNumber + 1)].Value = "Số lượng";
                sheet.Cells["C" + (rowNumber + 1)].Value = "Chi phí";
                sheet.Cells["D" + (rowNumber + 1)].Value = "thành tiền";
                sheet.Cells[rowNumber + 1, 4, rowNumber + 1, 6].Merge = true;

                sheet.Cells["A" + rowNumber].Style.Font.Bold = true;
                sheet.Cells["A" + (rowNumber + 1)].Style.Font.Bold = true;
                sheet.Cells["B" + (rowNumber + 1)].Style.Font.Bold = true;
                sheet.Cells["C" + (rowNumber + 1)].Style.Font.Bold = true;
                sheet.Cells["D" + (rowNumber + 1)].Style.Font.Bold = true;
                foreach (var item in pay.DetailRepairs)
                {
                    sheet.Cells[rowNumberWork, 4, rowNumberWork, 6].Merge = true;
                    sheet.Cells["A" + rowNumberWork].Value = item.IdWorkNavigation.WorkName;
                    sheet.Cells["B" + rowNumberWork].Value = item.Amount;
                    sheet.Cells["C" + rowNumberWork].Value = String.Format(info, "{0:c}", item.IdWorkNavigation.Cost);
                    sheet.Cells["D" + rowNumberWork].Value = String.Format(info, "{0:c}", item.Amount * item.IdWorkNavigation.Cost);
                    tongTienCong += item.Amount * item.IdWorkNavigation.Cost;
                    rowNumberWork++;
                }
                using (ExcelRange exr = sheet.Cells[rowNumber + 1, 1, rowNumberWork - 1, 6])
                {
                    //exr.AutoFitColumns();
                    exr.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    exr.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    exr.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    exr.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                }

                sheet.Cells["A4:E11"].AutoFitColumns();
                sheet.Cells["A11:E100"].AutoFitColumns();
                sheet.Cells["A11:E100"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                sheet.Cells["A11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                sheet.Cells["A" + rowNumber].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                sheet.Cells["B" + (rowNumberWork + 1)].Value = "Cộng:  " + String.Format(info, "{0:c}", (tongTienCong + tongTienVatTu));
                sheet.Cells["B" + (rowNumberWork + 2)].Value = "Thuế VAT(10%):  " + String.Format(info, "{0:c}", (((tongTienCong + tongTienVatTu) * 10) / 100));
                sheet.Cells["E" + (rowNumberWork + 2)].Value = "Đã thanh toán:  " + String.Format(info, "{0:c}", pay.Pays.FirstOrDefault().Paid);
                sheet.Cells["E" + (rowNumberWork + 3)].Value = "Còn nợ:  " + String.Format(info, "{0:c}", (pay.Pays.FirstOrDefault().Total - pay.Pays.FirstOrDefault().Paid));
                sheet.Cells["B" + (rowNumberWork + 3)].Value = "Tổng:  " + String.Format(info, "{0:c}", ((tongTienCong + tongTienVatTu) + (((tongTienCong + tongTienVatTu) * 10) / 100)));
                sheet.Cells["B" + (rowNumberWork + 1)].Style.Font.Bold = true;
                sheet.Cells["B" + (rowNumberWork + 2)].Style.Font.Bold = true;
                sheet.Cells["B" + (rowNumberWork + 3)].Style.Font.Bold = true;
                sheet.Cells["A" + (rowNumberWork + 5)].Style.Font.Bold = true;
                sheet.Cells["D" + (rowNumberWork + 5)].Style.Font.Bold = true;
                sheet.Cells["E" + (rowNumberWork + 2)].Style.Font.Bold = true;
                sheet.Cells["E" + (rowNumberWork + 3)].Style.Font.Bold = true;

                using (ExcelRange exr = sheet.Cells[rowNumberWork + 5, 1, rowNumberWork + 5, 6])
                {
                    exr.Style.Border.Top.Style = ExcelBorderStyle.Thick;


                }
                sheet.Cells["A" + (rowNumberWork + 5)].Value = "Cố vấn dịch vụ";
                sheet.Cells["D" + (rowNumberWork + 5)].Value = "Khách hàng";
                sheet.Cells["A" + (rowNumberWork + 6)].Value = sheet.Cells["D" + (rowNumberWork + 6)].Value = "(Ký ghi rõ họ tên)";



                package.Save();
            }

            stream.Position = 0;
            var fileName = $"BienLai_{DateTime.Now.ToString("yyyyMMddHHmmss")}_" + id + ".xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
