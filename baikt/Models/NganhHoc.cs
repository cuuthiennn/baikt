
namespace baikt.Models
{
    public class NganhHoc
    {
        public string MaNganh { get; set; } // Primary Key
        public string TenNganh { get; set; }
        
        // Navigation property
        public ICollection<SinhVien> SinhViens { get; set; }
    }
}