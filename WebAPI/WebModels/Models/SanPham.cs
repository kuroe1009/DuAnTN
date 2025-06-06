﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebModels.Models
{
    public class SanPham
    {
        [Key]
        public Guid IDSanPham { get; set; }

        // FK - sản phẩm phải thuộc phân loại
        [Required]
        [ForeignKey("PhanLoai")]
        public Guid IDPhanLoai { get; set; }
        public virtual PhanLoai PhanLoai { get; set; }

        [Required]
        [MaxLength(255)]
        public string TenSanPham { get; set; }

        public bool? GioiTinh { get; set; }  // Ví dụ: true=Nam, false=Nữ, null=unisex

        public double? TrongLuong { get; set; }

        [MaxLength(2000)]
        public string? MoTa { get; set; }

        [MaxLength(500)]
        public string? HinhAnh { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.UtcNow;

        public DateTime NgaySua { get; set; } = DateTime.UtcNow;

        public bool TrangThai { get; set; } = true;

        // 1 sản phẩm có thể có nhiều chi tiết (ví dụ size, màu)
        public virtual ICollection<SanPhamCT> SanPhamChiTiets { get; set; } = new List<SanPhamCT>();

        // 1 sản phẩm có thể có nhiều record giảm giá khác nhau (theo thời gian hoặc sự kiện)
        public virtual ICollection<SanPhamGG> SanPhamGiamGias { get; set; } = new List<SanPhamGG>();

        // 1 sản phẩm có thể xuất hiện nhiều lần trong hóa đơn chi tiết
        public virtual ICollection<HoaDonCT> HoaDonChiTiets { get; set; } = new List<HoaDonCT>();
        public virtual ICollection<GioHangCT> GioHangChiTiets { get; set; } = new List<GioHangCT>();

    }
}
