using Microsoft.AspNetCore.Mvc;
using baikt.Models;
using baikt.Data;

namespace baikt.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DangNhap(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sinhVien = await _context.SinhVien.FindAsync(model.MaSV);

                if (sinhVien != null)
                {
                    HttpContext.Session.SetString("MaSV", model.MaSV); // Ví dụ dùng Session
                    return RedirectToAction("Index", "SinhVien");
                }
                else
                {
                    ModelState.AddModelError("", "Mã Sinh Viên không tồn tại.");
                }
            }
            return View(model);
        }

        public IActionResult DangXuat()
        {
            HttpContext.Session.Remove("MaSV");
            return RedirectToAction("DangNhap", "Auth");
        }
    }
}