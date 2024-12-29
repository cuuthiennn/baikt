using baikt.Data;
using baikt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace baikt.Controllers
{
    public class HocPhanController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly List<HocPhan> _hocPhan;

        public HocPhanController(ApplicationDbContext context, List<HocPhan> hocPhan)
        {
            _context = context;
            _hocPhan = hocPhan;
        }

        public async Task<IActionResult> Index()
        {
            var hocPhans = await _context.HocPhan.ToListAsync();
            return View(hocPhans);
        }

        [HttpPost]
        public async Task<IActionResult> DangKy([FromBody] string maHP)
        {
            if (string.IsNullOrEmpty(maHP))
            {
                return Json(new { success = false, message = "Mã học phần không hợp lệ." });
            }

            var hocPhan = await _context.HocPhan.FindAsync(maHP);
            if (hocPhan == null)
            {
                return Json(new { success = false, message = "Học phần không tồn tại." });
            }

            if (hocPhan.SoLuong <= 0)
            {
                return Json(new { success = false, message = "Học phần đã đầy." });
            }

            string MaSV = HttpContext.Session.GetString("MaSV");
            if (string.IsNullOrEmpty(MaSV))
            {
                return Json(new { success = false, message = "Bạn chưa đăng nhập." });
            }
            var isContain = false;
            foreach (var e in _hocPhan)
            {
                if (e.MaHP.Equals(hocPhan.MaHP))
                {
                    isContain = true;
                    break;
                }
            }
            if (isContain)
            {
                return Json(new { success = false, message = "Bạn đã đăng ký học phần này rồi." });
            }

            hocPhan.SoLuong -= 1;
            _context.HocPhan.Update(hocPhan);
            await _context.SaveChangesAsync();
            
            _hocPhan.Add(hocPhan);

            return Json(new { success = true, message = "Đăng ký thành công." });
        }
    }
}