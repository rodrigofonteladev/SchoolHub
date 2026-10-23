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

        public async Task<List<SelectListItem>> GetCategoryCoursesSelectListAsync()
        {
            return await context.CategoryCourses.AsNoTracking().OrderBy(category => category.Name)
                .Select(category => new SelectListItem { Value = category.Id.ToString(), Text = category.Name })
                .ToListAsync();
        }

        public async Task<List<SelectListItem>> GetTeachersSelectListAsync() => await context.Teachers.AsNoTracking()
            .OrderBy(item => item.PaternalSurname).Select(item => new SelectListItem
            { Value = item.Id.ToString(), Text = $"{item.FullName} {item.PaternalSurname} {item.MaternalSurname}" }).ToListAsync();

        public async Task<List<SelectListItem>> GetCoursesSelectListAsync() => await context.Courses.AsNoTracking()
            .OrderBy(item => item.Name).Select(item => new SelectListItem
            { Value = item.Id.ToString(), Text = item.Name }).ToListAsync();

        public async Task<List<SelectListItem>> GetSchedulesSelectListAsync()
        {
            var schedules = await context.Schedules.AsNoTracking()
                .OrderBy(item => item.DayWeekId).ThenBy(item => item.StartTime)
                .Select(item => new { item.Id, Day = item.DayWeek.Name, item.StartTime, item.EndTime, Shift = item.Shift.Name })
                .ToListAsync();

            return schedules.Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = $"{item.Day} {item.StartTime:HH\\:mm}-{item.EndTime:HH\\:mm} ({item.Shift})"
            }).ToList();
        }

        public async Task<List<SelectListItem>> GetStudentsSelectListAsync() => await context.Students.AsNoTracking()
            .OrderBy(item => item.PaternalSurname).Select(item => new SelectListItem
            { Value = item.Id.ToString(), Text = $"{item.FullName} {item.PaternalSurname} {item.MaternalSurname}" }).ToListAsync();

        public async Task<List<SelectListItem>> GetClassAsignmentsSelectListAsync() => await context.ClassAsignments.AsNoTracking()
            .OrderByDescending(item => item.AssignedYear).Select(item => new SelectListItem
            { Value = item.Id.ToString(), Text = $"{item.Course.Name} - {item.Teacher.FullName} - {item.AssignedYear}" }).ToListAsync();

        public async Task<List<SelectListItem>> GetShiftsSelectListAsync() => await context.Shifts.AsNoTracking().OrderBy(item => item.Name)
            .Select(item => new SelectListItem { Value = item.Id.ToString(), Text = item.Name }).ToListAsync();

        public async Task<List<SelectListItem>> GetDaysSelectListAsync() => await context.DayWeeks.AsNoTracking().OrderBy(item => item.Id)
            .Select(item => new SelectListItem { Value = item.Id.ToString(), Text = item.Name }).ToListAsync();

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
