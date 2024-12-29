namespace baikt.Models
{
    public class DangKy
    {
        public int MaDK { get; set; } // Primary Key
        public DateTime NgayDK { get; set; }
        public string MaSV { get; set; } // Foreign Key

        // Navigation properties
        public SinhVien SinhVien { get; set; }
        public ICollection<ChiTietDangKy> ChiTietDangKys { get; set; }
    }
}