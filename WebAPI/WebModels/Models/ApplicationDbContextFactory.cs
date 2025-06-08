using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WebModels.Models
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Lấy đường dẫn thư mục hiện tại
            var basePath = Directory.GetCurrentDirectory();

            // Load cấu hình appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            // Lấy connection string
            var connectionString = config.GetConnectionString("DefaultConnection");

            // Tạo options builder
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            // Trả về DbContext với options đã config
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
