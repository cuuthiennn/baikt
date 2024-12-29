using baikt.Data;
using baikt.Models;
using baikt.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace baikt.Controllers
{
    public class DangKyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly List<HocPhan> _hocPhan;
        public DangKyController(ApplicationDbContext context, List<HocPhan> hocPhan)
        {
            _context = context;
            _hocPhan = hocPhan;
        }

        public async Task<IActionResult> Index()
        {
            string MaSV = HttpContext.Session.GetString("MaSV");
            if (string.IsNullOrEmpty(MaSV))
            {
                return RedirectToAction("DangNhap", "Auth");
            }

            var hocPhan = _hocPhan;

            return View(hocPhan);
        }

        [HttpPost]
        public async Task<IActionResult> DoXacNhanDangKy()
        {
            var maSV = HttpContext.Session.GetString("MaSV");
            if (string.IsNullOrEmpty(maSV))
            {
                return RedirectToAction("Index", new { error = "Session expired or MaSV is missing" });
            }

            DangKy dangKy = new DangKy
            {
                MaSV = maSV,
                NgayDK = DateTime.Now
            };

            await _context.DangKy.AddAsync(dangKy);
            await _context.SaveChangesAsync();

            if (_hocPhan == null || !_hocPhan.Any())
            {
                return RedirectToAction("Index", new { error = "No courses selected" });
            }

            foreach (var hocPhan in _hocPhan)
            {
                ChiTietDangKy hocDangKy = new ChiTietDangKy
                {
                    MaDK = dangKy.MaDK,
                    MaHP = hocPhan.MaHP
                };
                await _context.ChiTietDangKy.AddAsync(hocDangKy);
            }

            await _context.SaveChangesAsync();
            
            _hocPhan.Clear();

            return RedirectToAction("Index", new { success = "Registration completed successfully" });
        }


        
        public async Task<IActionResult> XacNhanDangKy()
        {
            string MaSV = HttpContext.Session.GetString("MaSV");
            var sinhVien = await _context.SinhVien.FindAsync(MaSV);
            if (sinhVien == null)
            {
                return RedirectToAction("Index");
            }
            DangKyInfoViewModel model = new DangKyInfoViewModel();
            model.SinhVien = sinhVien;
            model.THocPhans = _hocPhan.ToList();
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> XoaDangKy(string maHP)
        {
            int count = 0;
            foreach (var e in _hocPhan)
            {
                if (e.MaHP.Equals(maHP))
                {
                    break;
                }

                count++;
            }
            
            _hocPhan.RemoveAt(count);
            
            var hocPhan = await _context.HocPhan.FindAsync(maHP);
            hocPhan.SoLuong += 1;
            _context.HocPhan.Update(hocPhan);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> XoaTatCaDangKy()
        {
            _hocPhan.Clear();
            return RedirectToAction("Index");
        }

    }
}