using Ex04.Models;
using Ex04.Models.BaseEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ex04.Data
{
    public class NewsContext : IdentityDbContext
    {
        public NewsContext(DbContextOptions<NewsContext> opt) : base(opt)
        {

        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Rate> Rate { get; set; }

        public DbSet<ImageCategory> ImageCategories { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<@int> ImageAndCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var adminId = "8967c0da-1606-447b-b91b-10c9f7e87418";
            var adminRoleId = "56016200-6e5a-41ae-83ba-a9759ac9e6b5";
            var userRoleId = "99e62bd0-505e-4c7c-9533-3d0177220cec";

            builder.Entity<IdentityRole>().HasData(
                    new IdentityRole
                    {
                        Id = adminRoleId,
                        Name = "Admin",
                        NormalizedName = "Admin"
                    },
                    new IdentityRole
                    {
                        Id = userRoleId,
                        Name = "User",
                        NormalizedName = "User"
                    }
                );
            var hasher = new PasswordHasher<User>();
            builder.Entity<User>().HasData(
                    new User
                    {
                        Id = adminId,
                        UserName = "admin",
                        Email = "admin@mail.com",
                        NormalizedUserName = "admin",
                        PasswordHash = hasher.HashPassword(null, "1234")
                    }
                );

            builder.Entity<IdentityUserRole<string>>().HasData(
                    new IdentityUserRole<string>
                    {
                        RoleId = adminRoleId,
                        UserId = adminId
                    }
                );

            builder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Description = "",
                    Title = "Khoa học",
                    CreatedAt = DateTime.Now,
                    IsDeleted = false,
                },
                new Category
                {
                    Id = 2,
                    Description = "",
                    Title = "Xe",
                    CreatedAt = DateTime.Now,
                    IsDeleted = false,
                },
                new Category
                {
                    Id = 3,
                    Description = "",
                    Title = "Thể thao",
                    CreatedAt = DateTime.Now,
                    IsDeleted = false,
                }
                );

            builder.Entity<Post>().HasData(
                new Post
                {
                    Id = 1,
                    Description = "MC Piers Morgan thúc giục đội bóng yêu thích Arsenal ký hợp đồng với siêu sao Cristiano Ronaldo, sau khi anh rời Man Utd để thành cầu thủ tự do.",
                    Title = "Piers Morgan muốn Arsenal tuyển mộ Ronaldo",
                    Views = 300,
                    Content = "\"Đến lúc bắt đầu giai đoạn hai\", Morgan đăng trên Twitter sau khi Man Utd thông báo chấm dứt hợp đồng với Ronaldo trên cơ sở đồng thuận tối 22/11. Ông đính kèm hình ảnh siêu sao 37 tuổi giơ cao chiếc áo Arsenal số 7 in chữ Ronaldo, trong cuộc phỏng vấn trên chương trình Piers Morgan Uncensored tối 16/11 và 17/11.",
                    CreatedAt = DateTime.Now,
                    IsDeleted = false,
                    CategoryId = 3,
                    Rate = 0
                },
                new Post
                {
                    Id = 2,
                    Description = "Robot Perseverance ghi lại khoảnh khắc mặt trăng Phobos bay qua phía trước Mặt Trời, tạo thành \"nhật thực méo\" khác với trên Trái Đất.",
                    Title = "Robot NASA ghi hình nhật thực trên sao Hỏa",
                    Views = 400,
                    Content = "Nhật thực trên Trái Đất rất tròn trịa với Mặt Trời và Mặt Trăng đều tròn. Nhưng trên sao Hỏa, hiện tượng này trông hơi khác. Perseverance, robot đang hoạt động trên bề mặt sao Hỏa của NASA, ghi hình nhật thực khi mặt trăng Phobos với hình dạng giống củ khoai tây đi qua phía trước và che khuất một phần đĩa Mặt Trời hôm 18/11. Nhà khoa học hành tinh Paul Byrne so sánh cảnh tượng này giống với một \"con mắt hoạt hình\". Chuyên gia xử lý hình ảnh Kevin Gill tổng hợp các ảnh chụp của Perseverance thành một video ngắn cho thấy Phobos di chuyển ngang qua Mặt Trời, giúp người xem cảm nhận rõ ràng hơn về nhật thực khi quan sát từ bề mặt hành tinh đỏ. Phobos là một trong hai mặt trăng của sao Hỏa, trên bề mặt có nhiều hố trũng và rãnh. Điểm rộng nhất của thiên thể này là 27 km. Phobos chỉ nhỏ bằng 1/157 lần Mặt Trăng của Trái Đất. Vệ tinh tự nhiên còn lại của sao Hỏa, Deimos, còn có kích thước khiêm tốn hơn. Phobos di chuyển khá gần sao Hỏa, thậm chí có thể đâm vào hành tinh đỏ trong vài chục triệu năm nữa.",
                    CreatedAt = DateTime.Now,
                    Rate = 0,
                    IsDeleted = false,
                    CategoryId = 1
                }, new Post
                {
                    Id = 3,
                    Description = "Hãng Drako Motors mang đến triển lãm Los Angeles mẫu xe thứ hai, với khung gầm bằng carbon, mạnh 2.000 mã lực và tăng tốc 0-97 km/h trong 1,9 giây.",
                    Title = "Drako Dragon - SUV điện tăng tốc nhanh như siêu xe",
                    Views = 200,
                    Content = "Drako Motors là startup thành lập năm 2013, có trụ sở ở San Jose, bang California. Năm 2019, hãng ra mắt mẫu xe đầu tiên, chiếc sedan GTE công suất 1.200 mã lực. Năm nay, Drako trình làng Dragon thuộc dòng SUV với sức mạnh 2.000 mã lực. Cũng giống GTE, Dragon là xe thuần điện. Bộ khung gầm bằng sợi carbon giúp giảm 50% trọng lượng so với kết cấu SUV truyền thống, theo Drako. Ngoài ra, xe không có cột B vì sử dụng kiểu cửa cánh chim. Cửa mở điện và tạo ra không gian thoải mái để ra, vào cả hai hàng ghế. Thiết kế nội thất tối giản, màn hình thông tin giải trí 17,1 inch để điều khiển hệ thống điều hòa cùng nhiều tính năng khác. Hệ thống treo thiết lập ba cấp có thể thay đổi khoảng sáng gầm xe giữa 162-314 mm tùy thuộc chế độ.",
                    CreatedAt = DateTime.Now,
                    IsDeleted = false,
                    CategoryId = 2,
                    Rate = 0
                }
                );

            builder.Entity<ImageCategory>().HasData(
                new ImageCategory
                {
                    Id = 1,
                    Name = "News Image",
                    Description = "",
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                },
                new ImageCategory
                {
                    Id = 2,
                    Name = "Decor Image",
                    Description = "",
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                },
                new ImageCategory
                {
                    Id = 3,
                    Name = "Background Image",
                    Description = "",
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                },
                new ImageCategory
                {
                    Id = 4,
                    Name = "Banner Image",
                    Description = "",
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                }
            );
        }

        public override int SaveChanges()
        {
            BeforeSaveChanges();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            BeforeSaveChanges();
            return await base.SaveChangesAsync();
        }

        private void BeforeSaveChanges()
        {
            var entities = ChangeTracker.Entries();
            foreach (var entry in entities)
            {
                if (entry.Entity is IBaseEntity entityBase)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified: entityBase.UpdatedAt = DateTime.Now; break;
                        case EntityState.Added:
                            entityBase.UpdatedAt = DateTime.Now;
                            entityBase.CreatedAt = DateTime.Now;
                            break;
                    }
                }
            }
        }
    }
}
