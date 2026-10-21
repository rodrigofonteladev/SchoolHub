using Microsoft.EntityFrameworkCore;
using SchoolHub.Common;
using SchoolHub.Data;
using SchoolHub.Interfaces;
using SchoolHub.Models.Entities;
using SchoolHub.Models.ViewModels.Section;

namespace SchoolHub.Services
{
    public class SectionService(AppDbContext context) : ISectionService
    {
        public async Task<List<ListSectionViewModel>> GetAllAsync()
        {
            return await context.Sections
                .AsNoTracking()
                .OrderBy(section => section.Grade.Level.Name)
                .ThenBy(section => section.Grade.Name)
                .ThenBy(section => section.Name)
                .Select(section => new ListSectionViewModel
                {
                    Id = section.Id,
                    Name = section.Name,
                    GradeName = section.Grade.Name,
                    LevelName = section.Grade.Level.Name,
                    LimitQuantity = section.LimitStudents,
                    EnrollmentCount = section.Enrollments.Count
                })
                .ToListAsync();
        }

        public async Task<Result> CreateAsync(string name, int gradeId)
        {
            if (!await context.Grades.AnyAsync(grade => grade.Id == gradeId))
            {
                return Result.Failure("The selected grade does not exist.");
            }

            if (await context.Sections.AnyAsync(section => section.GradeId == gradeId && section.Name == name))
            {
                return Result.Failure("A section with that name already exists in the selected grade.");
            }

            await context.Sections.AddAsync(new Section { Name = name, GradeId = gradeId });
            await context.SaveChangesAsync();

            return Result.Success();
        }
    }
}