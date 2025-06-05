using System.ComponentModel.DataAnnotations;

namespace WebModels.Models
{
    public class PhanLoai
    {
        [Key]
        public Guid IDPhanLoai { get; set; }

        [Required(ErrorMessage = "Tên phân loại không được để trống.")]
        [MaxLength(100, ErrorMessage = "Tên phân loại không được vượt quá 100 ký tự.")]
        public string TenPhanLoai { get; set; } = null!;

        public DateTime NgayTao { get; set; } = DateTime.UtcNow;
        public DateTime NgaySua { get; set; } = DateTime.UtcNow;
        public bool TrangThai { get; set; } = true;

        // Navigation property - 1 PhanLoai có nhiều SanPham
        public virtual ICollection<SanPham> SanPhams { get; set; }

        public PhanLoai()
        {
            SanPhams = new HashSet<SanPham>();
        }
    }
}
