using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolHub.Models.Entities;

namespace SchoolHub.Data.Configurations
{
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.Property(col => col.StartTime).IsRequired();
            builder.Property(col => col.EndTime).IsRequired();
            builder.HasIndex(col => new { col.ShiftId, col.DayWeekId, col.StartTime, col.EndTime }).IsUnique();
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Schedule_StartTime", "StartTime < EndTime");
            });

            builder
                .HasOne(s => s.Shift)
                .WithMany(sh => sh.Schedules)
                .HasForeignKey(s => s.ShiftId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(s => s.DayWeek)
                .WithMany(dw => dw.Schedules)
                .HasForeignKey(s => s.DayWeekId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
