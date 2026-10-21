using Microsoft.EntityFrameworkCore;
using SchoolHub.Data;
using SchoolHub.Interfaces;
using SchoolHub.Models.ViewModels.Teacher;

namespace SchoolHub.Services
{
    public class TeacherService(AppDbContext context) : ITeacherService
    {
        public async Task<List<ListTeacherViewModel>> GetAllAsync()
        {
            return await context.Teachers
                .AsNoTracking()
                .OrderBy(teacher => teacher.PaternalSurname)
                .ThenBy(teacher => teacher.MaternalSurname)
                .ThenBy(teacher => teacher.FullName)
                .Select(teacher => new ListTeacherViewModel
                {
                    Id = teacher.Id,
                    FullName = teacher.FullName,
                    PaternalSurname = teacher.PaternalSurname,
                    MaternalSurname = teacher.MaternalSurname,
                    DocumentNumber = teacher.DocumentNumber,
                    DocumentTypeName = teacher.DocumentType.Name,
                    Email = teacher.Email,
                    IsActive = teacher.IsActive,
                    ClassAssignmentCount = teacher.ClassAsignments.Count
                })
                .ToListAsync();
        }
    }
}