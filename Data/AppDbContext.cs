using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolHub.Models.Entities;

namespace SchoolHub.Data
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public DbSet<CategoryCourse> CategoryCourses { get; set; }
        public DbSet<ClassAsignment> ClassAsignments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<DayWeek> DayWeeks { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<PointConversionRate> PointConversionRates { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseDetail> PurchaseDetails { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<ReasonEnrollment> ReasonEnrollments { get; set; }
        public DbSet<Recharge> Recharges { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Tutor> Tutors { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
    }
}
