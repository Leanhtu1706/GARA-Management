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
    public class RepairsController : Controller
    {
        private readonly GaraContext _context;

        public RepairsController(GaraContext context)
        {
            _context = context;
        }

        // GET: Repairs
        public async Task<IActionResult> Index(int? IdCar, DateTime date, StateType? state, string search)
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
            if (HttpContext.Session.GetString("ErrorMessage") != null)
            {
                ViewBag.ErrorMessage = HttpContext.Session.GetString("ErrorMessage");
                HttpContext.Session.Remove("ErrorMessage");
            }

            if (!string.IsNullOrEmpty(search))
            {
                var garaContext = _context.Repairs.Include(r => r.IdCarNavigation)
                    .ThenInclude(r => r.IdCustomerNavigation)
                    .Include(r => r.IdCarNavigation.IdCarModelNavigation)
                    .Where(r => r.IdCarNavigation.IdCustomerNavigation.Name.Contains(search) || r.IdCarNavigation.IdCarModelNavigation.ModelName.Contains(search) || r.IdCarNavigation.LicensePlates.Contains(search) || r.Id.ToString() == search);
                ViewData["GetTextSearch"] = search;
                return View(await garaContext.ToListAsync());
            }


            if (date.ToString() != "1/1/0001 00:00:00")
            {

                var garaContext = _context.Repairs.Include(r => r.IdCarNavigation)
                    .ThenInclude(r => r.IdCustomerNavigation)
                    .Include(r => r.IdCarNavigation.IdCarModelNavigation)
                    .Where(r =>  r.DateOfFactoryEntry.Month == date.Month && r.DateOfFactoryEntry.Date == date.Date)
                    .OrderByDescending(r => r.DateOfFactoryEntry);
                return View(await garaContext.ToListAsync());
            }
            else if (state != null)
            {
                var garaContext = _context.Repairs.Include(r => r.IdCarNavigation).
                    ThenInclude(r => r.IdCustomerNavigation).
                    Include(r => r.IdCarNavigation.IdCarModelNavigation)
                    .Where(r => r.State == state.Value)
                    .OrderByDescending(r => r.DateOfFactoryEntry);
                return View(await garaContext.ToListAsync());
            }
            else
            {
                var garaContext = _context.Repairs.Include(r => r.IdCarNavigation)
                    .ThenInclude(r => r.IdCustomerNavigation)
                    .Include(r => r.IdCarNavigation.IdCarModelNavigation)
                    .OrderByDescending(r => r.DateOfFactoryEntry);
                return View(await garaContext.ToListAsync());
            }

        }

        // GET: Repairs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetString("SuccessMessage") != null)
            {
                ViewBag.SuccessMessage = HttpContext.Session.GetString("SuccessMessage");
                HttpContext.Session.Remove("SuccessMessage");
            }
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repairs
                .Include(r => r.IdCarNavigation)
                .ThenInclude(r => r.IdCustomerNavigation)
                .Include(r => r.IdCarNavigation.IdCarModelNavigation)
                .Include(r => r.GoodsDeliveryNotes)
                .ThenInclude(r => r.DetailGoodsDeliveryNotes)
                .ThenInclude(r => r.IdMaterialNavigation)
                .Include(r => r.DetailRepairs)
                .ThenInclude(r => r.IdWorkNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (repair == null)
            {
                return NotFound();
            }
            else
            {
                if (repair.GoodsDeliveryNotes.Count == 0)
                {
                    ViewData["checkGoodsDeliveryNotes"] = "Null";
                }
                else
                {
                    ViewData["checkGoodsDeliveryNotes"] = "notNull";
                    var detailDelivery = repair.GoodsDeliveryNotes.Select(g => g.DetailGoodsDeliveryNotes).FirstOrDefault();
                    if (detailDelivery.Count() == 0)
                    {
                        ViewData["checkDetailGoodsDeliveryNotes"] = "Null";
                    }

                }

                return View(repair);
            }
        }

        // GET: Repairs/Create
        public IActionResult Create(int idCar, string layout = "_")
        {
            var existsRepair = _context.Repairs.Where(r => r.IdCar == idCar && r.State != StateType.completed).FirstOrDefault();
            if (existsRepair != null)
            {
                HttpContext.Session.SetString("ErrorMessage", "Xe này đang ở trong xưởng");
                return Json("false");
            }
            ViewData["Layout"] = layout == "_" ? "" : layout;
            ViewData["IdCar"] = idCar;
            return View();
        }

        // POST: Repairs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdCar,DateOfFactoryEntry,DateFinished,State")] Repair repair)
        {
            if (ModelState.IsValid)
            {
                _context.Add(repair);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("SuccessMessage", "Thêm mới thành công");

                //History
                History history = new History();
                history.DateHistory = DateTime.Now;
                history.UserName = HttpContext.Session.GetString("SessionUserName");
                history.Event = "Nhập xưởng xe của khách hàng " + _context.Cars.Include(c => c.IdCustomerNavigation).Where(c => c.Id == repair.IdCar).Select(c => c.IdCustomerNavigation.Name).FirstOrDefault();
                _context.Add(history);
                await _context.SaveChangesAsync();
                //============================

                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCar"] = new SelectList(_context.Cars, "Id", "Id", repair.IdCar);
            return View(repair);
        }

        // GET: Repairs/Edit/5
        public async Task<IActionResult> Edit(int? id, string layout = "_")
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repairs.FindAsync(id);
            if (repair == null)
            {
                return NotFound();
            }
            ViewData["IdCar"] = new SelectList(_context.Cars, "Id", "Id", repair.IdCar);
            ViewData["Layout"] = layout == "_" ? "" : layout;
            return View(repair);
        }

        // POST: Repairs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdCar,DateOfFactoryEntry,DateFinished,State")] Repair repair)
        {
            if (id != repair.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(repair);
                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetString("SuccessMessage", "Cập nhật thành công");

                    //History
                    History history = new History();
                    history.DateHistory = DateTime.Now;
                    history.UserName = HttpContext.Session.GetString("SessionUserName");
                    history.Event = "Cập nhật thông tin nhập xuất xưởng cho xe của khách hàng " + _context.Cars.Include(c => c.IdCustomerNavigation).Where(c => c.Id == repair.IdCar).Select(c => c.IdCustomerNavigation.Name).FirstOrDefault();
                    _context.Add(history);
                    await _context.SaveChangesAsync();
                    //============================

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepairExists(repair.Id))
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
            ViewData["IdCar"] = new SelectList(_context.Cars, "Id", "Id", repair.IdCar);
            return View(repair);
        }

        // GET: Repairs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repairs
                .Include(r => r.IdCarNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (repair == null)
            {
                return NotFound();
            }

            return Json(repair.Id);
        }

        // POST: Repairs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var repair = await _context.Repairs.Include(r => r.IdCarNavigation).ThenInclude(r => r.IdCustomerNavigation).Where(r => r.Id == id).FirstOrDefaultAsync();
            var goodsDeliveryNote = _context.GoodsDeliveryNotes.Where(g => g.IdRepair == id).FirstOrDefault();
            if (goodsDeliveryNote != null)
            {
                var detailGoodsDeliveryNote = _context.DetailGoodsDeliveryNotes.Include(dg => dg.IdGoodsDeliveryNoteNavigation).Where(dg => dg.IdGoodsDeliveryNote == goodsDeliveryNote.Id).ToList();
                if (detailGoodsDeliveryNote.Any())
                {
                    foreach (var itemdg in detailGoodsDeliveryNote)
                    {
                        _context.DetailGoodsDeliveryNotes.Remove(itemdg);
                    }
                }
                _context.GoodsDeliveryNotes.Remove(goodsDeliveryNote);
            }
            var detailRepair = _context.DetailRepairs.Where(dr => dr.IdRepair == id).ToList();
            if (detailRepair.Any())
            {
                foreach (var itemdr in detailRepair)
                {
                    _context.DetailRepairs.Remove(itemdr);
                }
            }

            _context.Repairs.Remove(repair);
            await _context.SaveChangesAsync();
            HttpContext.Session.SetString("SuccessMessage", "Xóa thành công");

            //History
            History history = new History();
            history.DateHistory = DateTime.Now;
            history.UserName = HttpContext.Session.GetString("SessionUserName");
            history.Event = "Xóa xe của khách hàng " + repair.IdCarNavigation.IdCustomerNavigation.Name + " ra khỏi xưởng";
            _context.Add(history);
            await _context.SaveChangesAsync();
            //============================

            return RedirectToAction(nameof(Index));
        }

        private bool RepairExists(int id)
        {
            return _context.Repairs.Any(e => e.Id == id);
        }
        public async Task<IActionResult> BaoGia(int? id)
        {
            var repair = await _context.Repairs
                .Include(r => r.IdCarNavigation)
                .ThenInclude(r => r.IdCustomerNavigation)
                .Include(r => r.GoodsDeliveryNotes)
                .ThenInclude(r => r.DetailGoodsDeliveryNotes)
                .ThenInclude(r => r.IdMaterialNavigation)
                .ThenInclude(r => r.PriceMaterials)
                .Include(r => r.DetailRepairs)
                .ThenInclude(r => r.IdWorkNavigation)
                .Include(r => r.Pays)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (repair.Pays.Any())
            {
                ViewData["exists"] = "exists";
            }
            return View(repair);
        }

        public async Task<IActionResult> Export(int id)
        {
            var repair = await _context.Repairs
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
                var sheet = package.Workbook.Worksheets.Add("Báo giá");
                

                sheet.Cells["C2"].Value = "Báo giá sửa chữa";

                sheet.Cells["A4"].Value = "Tên công ty:";
                sheet.Cells["A5"].Value = "Mã Số thuế:";
                sheet.Cells["A6"].Value = "Mã phiếu sửa:   " + repair.Id;
                sheet.Cells["A7"].Value = "Tên Khách hàng:   "+ repair.IdCarNavigation.IdCustomerNavigation.Name;
                sheet.Cells["A8"].Value = "Số điện thoại:   " + repair.IdCarNavigation.IdCustomerNavigation.Phone; 
                sheet.Cells["A9"].Value = "Địa chỉ:   " + repair.IdCarNavigation.IdCustomerNavigation.Address;
                sheet.Cells["A10"].Value = "Số CMND:   " + repair.IdCarNavigation.IdCustomerNavigation.IdentityCardNumber; 

                sheet.Cells["D6"].Value = "Thời gian: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                sheet.Cells["D7"].Value = "Biển số:"; sheet.Cells["E7"].Value = repair.IdCarNavigation.LicensePlates;
                sheet.Cells["D8"].Value = "Dòng xe:"; sheet.Cells["E8"].Value = repair.IdCarNavigation.IdCarModelNavigation.ModelName;
                sheet.Cells["D9"].Value = "Màu:"; sheet.Cells["E9"].Value = repair.IdCarNavigation.Color;
                sheet.Cells["D10"].Value = "Ghi chú:"; sheet.Cells["E10"].Value = repair.IdCarNavigation.Note;
                sheet.Cells["A11"].Value = "A. Chi tiết tiền vật tư:";
                sheet.Cells["A12"].Value = "Tên vật tư";
                sheet.Cells["B12"].Value = "Đv tính";
                sheet.Cells["C12"].Value = "Số lượng";
                sheet.Cells["D12"].Value = "Đơn giá";
                sheet.Cells["E12"].Value = "thành tiền";
                sheet.Cells[12, 5, 12, 6].Merge = true;

                using (ExcelRange exr = sheet.Cells[7,1,7,6])
                {
                    exr.Style.Border.Top.Style = ExcelBorderStyle.Thin;  
                }
                using (ExcelRange exr = sheet.Cells[10,1,10,6])
                {
                    exr.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;  
                }
                using (ExcelRange exr = sheet.Cells[7,6,10,6])
                {
                    exr.Style.Border.Right.Style = ExcelBorderStyle.Thin;  
                }
                using (ExcelRange exr = sheet.Cells[7,1,10,1])
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
                if(repair.GoodsDeliveryNotes.Count() != 0)
                {
                    foreach (var item in repair.GoodsDeliveryNotes.Where(r => r.IdRepair == id).FirstOrDefault().DetailGoodsDeliveryNotes)
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
                for (var n = rowNumber; n < 25; n++)
                {
                    sheet.Cells[n, 5, n, 6].Merge = true;
                }
                rowNumber = 25;
                using (ExcelRange exr = sheet.Cells[12, 1, rowNumber-1, 6])
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
                sheet.Cells["A"+ rowNumber].Value = "B. Chi tiết tiền công:";
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
  
                foreach (var item in repair.DetailRepairs)
                {
                    sheet.Cells[rowNumberWork, 4, rowNumberWork, 6].Merge = true;
                    sheet.Cells["A" + rowNumberWork].Value = item.IdWorkNavigation.WorkName;
                    sheet.Cells["B" + rowNumberWork].Value = item.Amount;
                    sheet.Cells["C" + rowNumberWork].Value = String.Format(info, "{0:c}", item.IdWorkNavigation.Cost);
                    sheet.Cells["D" + rowNumberWork].Value = String.Format(info, "{0:c}", item.Amount * item.IdWorkNavigation.Cost);
                    tongTienCong += item.Amount * item.IdWorkNavigation.Cost;
                    rowNumberWork++;    
                }
                
                for (var n = rowNumberWork; n < 39 ; n++)
                {
                    sheet.Cells[n, 4, n, 6].Merge = true;
                }
                rowNumberWork = 39;
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
                sheet.Cells["B" + (rowNumberWork + 3)].Value = "Tổng:  " + String.Format(info, "{0:c}", ((tongTienCong + tongTienVatTu) + (((tongTienCong + tongTienVatTu) * 10) / 100)));
                sheet.Cells["B" + (rowNumberWork + 1)].Style.Font.Bold = true;
                sheet.Cells["B" + (rowNumberWork + 2)].Style.Font.Bold = true;
                sheet.Cells["B" + (rowNumberWork + 3)].Style.Font.Bold = true;
                sheet.Cells["A" + (rowNumberWork + 5)].Style.Font.Bold = true;
                sheet.Cells["D" + (rowNumberWork + 5)].Style.Font.Bold = true;

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
            var fileName = $"BaoGia_{DateTime.Now.ToString("yyyyMMddHHmmss")}_"+ id +".xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
