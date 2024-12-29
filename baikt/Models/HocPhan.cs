namespace baikt.Models
{
    public class HocPhan
    {
        public string MaHP { get; set; } // Primary Key
        public string TenHP { get; set; }
        public int SoTinChi { get; set; }
        
        public int SoLuong { get; set; }

        // Navigation property
        public ICollection<ChiTietDangKy> ChiTietDangKys { get; set; }
    }
}