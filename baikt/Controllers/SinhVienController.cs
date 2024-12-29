using baikt.Data;
using baikt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace baikt.Controllers
{
    public class SinhVienController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SinhVienController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            string MaSV = HttpContext.Session.GetString("MaSV");
            if (string.IsNullOrEmpty(MaSV))
            {
                return RedirectToAction("DangNhap", "Auth");
            }

            var students = await _context.SinhVien.ToListAsync();
            return View(students);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SinhVien sinhVien, IFormFile hinh)
        {
            if (hinh != null)
            {
                if (!hinh.ContentType.StartsWith("image/"))
                {
                    ModelState.AddModelError("Hinh", "Please upload a valid image.");
                    return View(sinhVien);
                }

                if (hinh.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("Hinh", "File size exceeds the limit of 5MB.");
                    return View(sinhVien);
                }

                var uploadDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                var fileName = Path.GetFileName(hinh.FileName);
                var filePath = Path.Combine(uploadDirectory, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await hinh.CopyToAsync(stream);
                }

                sinhVien.Hinh = "/images/" + fileName;
            }

            await _context.SinhVien.AddAsync(sinhVien);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "SinhVien");
        }


        public async Task<IActionResult> Edit(string id)
        {
            var student = await _context.SinhVien.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SinhVien sinhVien, IFormFile hinh)
        {
            if (hinh != null)
            {
                if (!hinh.ContentType.StartsWith("image/"))
                {
                    ModelState.AddModelError("Hinh", "Please upload a valid image.");
                    return View(sinhVien);
                }

                if (hinh.Length > 5 * 1024 * 1024) // 5MB
                {
                    ModelState.AddModelError("Hinh", "File size exceeds the limit of 5MB.");
                    return View(sinhVien);
                }

                var uploadDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                var fileName = Path.GetFileName(hinh.FileName);
                var filePath = Path.Combine(uploadDirectory, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await hinh.CopyToAsync(stream);
                }

                sinhVien.Hinh = "/images/" + fileName;
            }
            else
            {
                var existingStudent = await _context.SinhVien.AsNoTracking()
                    .FirstOrDefaultAsync(s => s.MaSV == sinhVien.MaSV);
                if (existingStudent != null)
                {
                    sinhVien.Hinh = existingStudent.Hinh;
                }
            }

            try
            {
                _context.Entry(sinhVien).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "SinhVien");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.SinhVien.Any(s => s.MaSV == sinhVien.MaSV))
                {
                    return NotFound();
                }
                throw;
            }
        }


        public async Task<IActionResult> Delete(string id)
        {
            var student = await _context.SinhVien.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var entity = await _context.SinhVien.FindAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Sinh viên với ID {id} không tồn tại.");
            }

            var dangKyList = await _context.DangKy.Where(dk => dk.MaSV == id).ToListAsync();
            foreach (var dangKy in dangKyList)
            {
                var chiTietList = await _context.ChiTietDangKy.Where(ct => ct.MaDK == dangKy.MaDK).ToListAsync();
                _context.ChiTietDangKy.RemoveRange(chiTietList);
                _context.DangKy.Remove(dangKy);
            }

            _context.SinhVien.Remove(entity);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Detail(string id)
        {
            var student = await _context.SinhVien.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }
    }
}
