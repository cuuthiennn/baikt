namespace baikt.Models
{
    public class SinhVien
    {
        public string MaSV { get; set; } // Primary Key
        public string HoTen { get; set; }
        public string GioiTinh { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Hinh { get; set; }
        public string MaNganh { get; set; } // Foreign Key

        // Navigation properties
        public NganhHoc NganhHoc { get; set; }
        public ICollection<DangKy> DangKys { get; set; }
    }
}