using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolHub.Data;
using SchoolHub.Interfaces;
using SchoolHub.Common.Enums;

namespace SchoolHub.Services
{
    public class CatalogService(AppDbContext context) : ICatalogService
    {
        public async Task<List<SelectListItem>> GetDocumentTypesSelectListAsync()
        {
            return await context.DocumentTypes
            .AsNoTracking()
            .Select(dt => new SelectListItem
            {
                Value = dt.Id.ToString(),
                Text = dt.Name
            }).ToListAsync();
        }

        public async Task<List<SelectListItem>> GetLevelsSelectListAsync()
        {
            return await context.Levels
            .AsNoTracking()
            .Select(l => new SelectListItem
            {
                Value = l.Id.ToString(),
                Text = l.Name
            }).ToListAsync();
        }

        public async Task<List<SelectListItem>> GetGradesSelectListAsync()
        {
            return await context.Grades
            .AsNoTracking()
            .OrderBy(grade => grade.Level.Name)
            .ThenBy(grade => grade.Name)
            .Select(grade => new SelectListItem
            {
                Value = grade.Id.ToString(),
                Text = $"{grade.Name} - {grade.Level.Name}"
            }).ToListAsync();
        }

        public async Task<List<SelectListItem>> GetSectionsSelectListAsync()
        {
            return await context.Sections
            .AsNoTracking()
            .Where(section => section.Enrollments.Count < section.LimitStudents)
            .OrderBy(section => section.Grade.Level.Name)
            .ThenBy(section => section.Grade.Name)
            .Select(section => new SelectListItem
            {
                Value = section.Id.ToString(),
                Text = $"{section.Grade.Name} {section.Name} - {section.Grade.Level.Name}"
            }).ToListAsync();
        }

        public async Task<List<SelectListItem>> GetReasonEnrollmentsSelectListAsync()
        {
            return await context.ReasonEnrollments
            .AsNoTracking()
            .OrderBy(reason => reason.Description)
            .Select(reason => new SelectListItem
            {
                Value = reason.Id.ToString(),
                Text = reason.Description
            }).ToListAsync();
        }

        public async Task<List<SelectListItem>> GetEnrollmentsSelectListAsync()
        {
            return await context.Enrollments
                .AsNoTracking()
                .Where(enrollment => enrollment.Status != EnrollmentStatus.Cancelled)
                .OrderByDescending(enrollment => enrollment.EnrolledYear)
                .ThenBy(enrollment => enrollment.Student.PaternalSurname)
                .ThenBy(enrollment => enrollment.Student.MaternalSurname)
                .Select(enrollment => new SelectListItem
                {
                    Value = enrollment.Id.ToString(),
                    Text = $"{enrollment.Student.FullName} {enrollment.Student.PaternalSurname} {enrollment.Student.MaternalSurname} - {enrollment.EnrolledYear}"
                })
                .ToListAsync();
        }
    }
}