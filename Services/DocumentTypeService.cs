using Microsoft.EntityFrameworkCore;
using SchoolHub.Common;
using SchoolHub.Data;
using SchoolHub.Interfaces;
using SchoolHub.Models.Entities;
using SchoolHub.Models.ViewModels.DocumentType;

namespace SchoolHub.Services
{
    public class DocumentTypeService(AppDbContext context) : IDocumentTypeService
    {
        public async Task<List<ListDocumentTypeViewModel>> GetAllAsync()
        {
            return await context.DocumentTypes
                .AsNoTracking()
                .OrderBy(documentType => documentType.Name)
                .Select(documentType => new ListDocumentTypeViewModel
                {
                    Id = documentType.Id,
                    Name = documentType.Name,
                    IsActive = documentType.IsActive,
                    StudentCount = documentType.Students.Count,
                    TeacherCount = documentType.Teachers.Count,
                    TutorCount = documentType.Tutors.Count
                })
                .ToListAsync();
        }

        public async Task<Result> CreateAsync(string name, bool isActive)
        {
            if (await context.DocumentTypes.AnyAsync(documentType => documentType.Name == name))
            {
                return Result.Failure("A document type with that name already exists.");
            }

            await context.DocumentTypes.AddAsync(new DocumentType
            {
                Name = name,
                IsActive = isActive
            });
            await context.SaveChangesAsync();

            return Result.Success();
        }
    }
}