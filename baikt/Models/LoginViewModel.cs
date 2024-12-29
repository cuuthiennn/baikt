using System.ComponentModel.DataAnnotations;

namespace baikt.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập Mã Sinh Viên")]
        [Display(Name = "MaSV")] // Để hiển thị "MaSV" trên label
        public string MaSV { get; set; }
    }
}