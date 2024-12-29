namespace baikt.Models
{
    public class ChiTietDangKy
    {
        public int MaDK { get; set; } // Composite Key
        public string MaHP { get; set; } // Composite Key

        // Navigation properties
        public DangKy DangKy { get; set; }
        public HocPhan HocPhan { get; set; }
    }
}