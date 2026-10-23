using Microsoft.EntityFrameworkCore;
using SchoolHub.Common;
using SchoolHub.Data;
using SchoolHub.Interfaces;
using SchoolHub.Models.Entities;
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

        public async Task<Result> CreateAsync(RegisterTeacherViewModel model)
        {
            if (!model.DocumentTypeId.HasValue || !await context.DocumentTypes.AnyAsync(documentType => documentType.Id == model.DocumentTypeId.Value))
            {
                return Result.Failure("The selected document type does not exist.");
            }

            if (await context.Teachers.AnyAsync(teacher => teacher.DocumentNumber == model.DocumentNumber))
            {
                return Result.Failure("A teacher with that document number already exists.");
            }

            await context.Teachers.AddAsync(new Teacher
            {
                FullName = model.FullName,
                PaternalSurname = model.PaternalSurname,
                MaternalSurname = model.MaternalSurname,
                DocumentNumber = model.DocumentNumber,
                Email = model.Email,
                Address = model.Address,
                DocumentTypeId = model.DocumentTypeId.Value
            });
            await context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<RegisterTeacherViewModel?> GetForEditAsync(int id) => await context.Teachers.AsNoTracking().Where(item => item.Id == id).Select(item => new RegisterTeacherViewModel
        { FullName = item.FullName, PaternalSurname = item.PaternalSurname, MaternalSurname = item.MaternalSurname, DocumentNumber = item.DocumentNumber, Email = item.Email, Address = item.Address, DocumentTypeId = item.DocumentTypeId }).FirstOrDefaultAsync();

        public async Task<Result> UpdateAsync(int id, RegisterTeacherViewModel model)
        {
            var entity = await context.Teachers.FindAsync(id);
            if (entity is null) return Result.Failure("The teacher does not exist.");
            if (!await context.DocumentTypes.AnyAsync(item => item.Id == model.DocumentTypeId)) return Result.Failure("The selected document type does not exist.");
            if (await context.Teachers.AnyAsync(item => item.Id != id && item.DocumentNumber == model.DocumentNumber)) return Result.Failure("A teacher with that document number already exists.");
            entity.FullName = model.FullName; entity.PaternalSurname = model.PaternalSurname; entity.MaternalSurname = model.MaternalSurname; entity.DocumentNumber = model.DocumentNumber; entity.Email = model.Email; entity.Address = model.Address; entity.DocumentTypeId = model.DocumentTypeId!.Value;
            await context.SaveChangesAsync(); return Result.Success();
        }
    }
}
