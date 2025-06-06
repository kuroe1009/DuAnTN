﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebModels.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<GioHang> GioHangs { get; set; }
        public DbSet<GioHangCT> GioHangChiTiets { get; set; }
        public DbSet<DiaChiNhanHang> DiaChiNhanHangs { get; set; }
        public DbSet<HinhThucTT> HinhThucTTs { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<HoaDonCT> HoaDonChiTiets { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<PhanLoai> PhanLoais { get; set; }
        public DbSet<SanPhamGG> SanPhamGiamGias { get; set; }
        public DbSet<GiamGia> GiamGias { get; set; }
        public DbSet<SanPhamCT> SanPhamChiTiets { get; set; }
        public DbSet<MauSac> MauSacs { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<CoAo> CoAos { get; set; }
        public DbSet<TaAo> TaAos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Cần thiết cho Identity

            // Quan hệ ApplicationUser - ApplicationRole (IDRole nullable)
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.IDRole)
                .OnDelete(DeleteBehavior.Restrict);

            // 1-1: User - GioHang
            builder.Entity<GioHang>()
                .HasOne(g => g.User)
                .WithOne(u => u.GioHang)
                .HasForeignKey<GioHang>(g => g.IDUser)
                .OnDelete(DeleteBehavior.Cascade);

            // 1-n: GioHang - GioHangChiTiet
            builder.Entity<GioHangCT>()
                .HasOne(ct => ct.GioHang)
                .WithMany(g => g.GioHangChiTiets)
                .HasForeignKey(ct => ct.IDGioHang)
                .OnDelete(DeleteBehavior.Cascade);

            // 1-n: HinhThucTT - HoaDon
            builder.Entity<HoaDon>()
                .HasOne(hd => hd.HinhThucTT)
                .WithMany(ht => ht.HoaDons)
                .HasForeignKey(hd => hd.IDHinhThucTT)
                .OnDelete(DeleteBehavior.Restrict);

            // 1-n: DiaChiNhanHang - HoaDon
            builder.Entity<HoaDon>()
                .HasOne(hd => hd.DiaChiNhanHang)
                .WithMany(dc => dc.HoaDons)
                .HasForeignKey(hd => hd.IDDiaChiNhanHang)
                .OnDelete(DeleteBehavior.Restrict); // tránh multiple cascade paths

            // 1-n: ApplicationUser - HoaDon
            builder.Entity<HoaDon>()
                .HasOne(hd => hd.User)
                .WithMany(u => u.HoaDons)
                .HasForeignKey(hd => hd.IDUser)
                .OnDelete(DeleteBehavior.Restrict);

            // 1-n: HoaDon - HoaDonChiTiet
            builder.Entity<HoaDonCT>()
                .HasOne(ct => ct.HoaDon)
                .WithMany(hd => hd.HoaDonChiTiets)
                .HasForeignKey(ct => ct.IDHoaDon)
                .OnDelete(DeleteBehavior.Cascade);

            // 1-n: PhanLoai - SanPham
            builder.Entity<SanPham>()
                .HasOne(sp => sp.PhanLoai)
                .WithMany(pl => pl.SanPhams)
                .HasForeignKey(sp => sp.IDPhanLoai)
                .OnDelete(DeleteBehavior.Restrict);

            // 1-n: SanPham - SanPhamChiTiet
            builder.Entity<SanPhamCT>()
                .HasOne(spct => spct.SanPham)
                .WithMany(sp => sp.SanPhamChiTiets)
                .HasForeignKey(spct => spct.IDSanPham)
                .OnDelete(DeleteBehavior.Cascade);

            // 1-n: MauSac - SanPhamChiTiet
            builder.Entity<SanPhamCT>()
                .HasOne(spct => spct.MauSac)
                .WithMany(ms => ms.SanPhamChiTiets)
                .HasForeignKey(spct => spct.IDMauSac)
                .OnDelete(DeleteBehavior.Restrict);

            // 1-n: Size - SanPhamChiTiet
            builder.Entity<SanPhamCT>()
                .HasOne(spct => spct.Size)
                .WithMany(sz => sz.SanPhamChiTiets)
                .HasForeignKey(spct => spct.IDSize)
                .OnDelete(DeleteBehavior.Restrict);

            // 1-n: CoAo - SanPhamChiTiet
            builder.Entity<SanPhamCT>()
                .HasOne(spct => spct.CoAo)
                .WithMany(ca => ca.SanPhamChiTiets)
                .HasForeignKey(spct => spct.IDCoAo)
                .OnDelete(DeleteBehavior.Restrict);

            // 1-n: TaAo - SanPhamChiTiet
            builder.Entity<SanPhamCT>()
                .HasOne(spct => spct.TaAo)
                .WithMany(ta => ta.SanPhamChiTiets)
                .HasForeignKey(spct => spct.IDTaAo)
                .OnDelete(DeleteBehavior.Restrict);

            // Kiểu decimal mặc định là (18,2)
            foreach (var property in builder.Model.GetEntityTypes()
                         .SelectMany(t => t.GetProperties())
                         .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                if (string.IsNullOrEmpty(property.GetColumnType()))
                {
                    property.SetColumnType("decimal(18,2)");
                }
            }

        }
        // Ví dụ trong DbContext
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is SanPham && // Hoặc một base entity nếu có
                            (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((SanPham)entityEntry.Entity).NgaySua = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    ((SanPham)entityEntry.Entity).NgayTao = DateTime.UtcNow;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
