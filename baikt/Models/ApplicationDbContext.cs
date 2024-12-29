using baikt.Models;
using Microsoft.EntityFrameworkCore;

namespace baikt.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<NganhHoc> NganhHoc { get; set; }
        public DbSet<SinhVien> SinhVien { get; set; }
        public DbSet<HocPhan> HocPhan { get; set; }
        public DbSet<DangKy> DangKy { get; set; }
        public DbSet<ChiTietDangKy> ChiTietDangKy { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình bảng NganhHoc
            modelBuilder.Entity<NganhHoc>()
                .HasKey(nh => nh.MaNganh); // Định nghĩa khóa chính

            modelBuilder.Entity<NganhHoc>()
                .Property(nh => nh.TenNganh)
                .HasMaxLength(30)
                .IsRequired(); // Giới hạn độ dài và bắt buộc

            // Cấu hình bảng SinhVien
            modelBuilder.Entity<SinhVien>()
                .HasKey(sv => sv.MaSV); // Định nghĩa khóa chính

            modelBuilder.Entity<SinhVien>()
                .Property(sv => sv.HoTen)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<SinhVien>()
                .HasOne(sv => sv.NganhHoc)
                .WithMany(nh => nh.SinhViens)
                .HasForeignKey(sv => sv.MaNganh)
                .OnDelete(DeleteBehavior.Restrict); // Không xóa liên quan

            // Cấu hình bảng HocPhan
            modelBuilder.Entity<HocPhan>()
                .HasKey(hp => hp.MaHP); // Định nghĩa khóa chính

            modelBuilder.Entity<HocPhan>()
                .Property(hp => hp.TenHP)
                .HasMaxLength(30)
                .IsRequired();

            modelBuilder.Entity<HocPhan>()
                .Property(hp => hp.SoTinChi)
                .IsRequired();

            // Cấu hình bảng DangKy
            modelBuilder.Entity<DangKy>()
                .HasKey(dk => dk.MaDK); // Định nghĩa khóa chính

            modelBuilder.Entity<DangKy>()
                .Property(dk => dk.NgayDK)
                .IsRequired();

            modelBuilder.Entity<DangKy>()
                .HasOne(dk => dk.SinhVien)
                .WithMany(sv => sv.DangKys)
                .HasForeignKey(dk => dk.MaSV)
                .OnDelete(DeleteBehavior.Cascade); // Xóa liên quan

            // Cấu hình bảng ChiTietDangKy
            modelBuilder.Entity<ChiTietDangKy>()
                .HasKey(ct => new { ct.MaDK, ct.MaHP }); // Định nghĩa khóa chính tổng hợp

            modelBuilder.Entity<ChiTietDangKy>()
                .HasOne(ct => ct.DangKy)
                .WithMany(dk => dk.ChiTietDangKys)
                .HasForeignKey(ct => ct.MaDK)
                .OnDelete(DeleteBehavior.Cascade); // Xóa liên quan

            modelBuilder.Entity<ChiTietDangKy>()
                .HasOne(ct => ct.HocPhan)
                .WithMany(hp => hp.ChiTietDangKys)
                .HasForeignKey(ct => ct.MaHP)
                .OnDelete(DeleteBehavior.Restrict); // Không xóa liên quan
        }
    }
}