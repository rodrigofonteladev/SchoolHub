using Microsoft.EntityFrameworkCore;
using SchoolHub.Common;
using SchoolHub.Data;
using SchoolHub.Interfaces;
using SchoolHub.Models.Entities;
using SchoolHub.Models.ViewModels.Tutor;

namespace SchoolHub.Services
{
    public class TutorService(AppDbContext context) : ITutorService
    {
        public async Task<List<ListTutorViewModel>> GetAllAsync() => await context.Tutors.AsNoTracking().OrderBy(item => item.PaternalSurname).Select(item => new ListTutorViewModel
        { Id = item.Id, FullName = item.FullName, PaternalSurname = item.PaternalSurname, MaternalSurname = item.MaternalSurname, DocumentTypeName = item.DocumentType.Name, DocumentNumber = item.DocumentNumber, StudentName = $"{item.Student.FullName} {item.Student.PaternalSurname}", Relationship = item.Relationship, Email = item.Email }).ToListAsync();

        public async Task<Result> CreateAsync(RegisterTutorViewModel model)
        {
            if (!model.StudentId.HasValue || !model.DocumentTypeId.HasValue || !model.Relationship.HasValue || !await context.Students.AnyAsync(item => item.Id == model.StudentId.Value) || !await context.DocumentTypes.AnyAsync(item => item.Id == model.DocumentTypeId.Value)) return Result.Failure("The selected student or document type does not exist.");
            if (await context.Tutors.AnyAsync(item => item.DocumentNumber == model.DocumentNumber)) return Result.Failure("A tutor with that document number already exists.");
            await context.Tutors.AddAsync(new Tutor { FullName = model.FullName, PaternalSurname = model.PaternalSurname, MaternalSurname = model.MaternalSurname, DocumentNumber = model.DocumentNumber, Email = model.Email, Address = model.Address, Relationship = model.Relationship.Value, StudentId = model.StudentId.Value, DocumentTypeId = model.DocumentTypeId.Value });
            await context.SaveChangesAsync(); return Result.Success();
        }

        public async Task<RegisterTutorViewModel?> GetForEditAsync(int id) => await context.Tutors.AsNoTracking().Where(item => item.Id == id).Select(item => new RegisterTutorViewModel { FullName = item.FullName, PaternalSurname = item.PaternalSurname, MaternalSurname = item.MaternalSurname, DocumentNumber = item.DocumentNumber, Email = item.Email, Address = item.Address, Relationship = item.Relationship, StudentId = item.StudentId, DocumentTypeId = item.DocumentTypeId }).FirstOrDefaultAsync();

        public async Task<Result> UpdateAsync(int id, RegisterTutorViewModel model)
        {
            var entity = await context.Tutors.FindAsync(id);
            if (entity is null) return Result.Failure("The tutor does not exist.");
            if (!model.StudentId.HasValue || !model.DocumentTypeId.HasValue || !model.Relationship.HasValue || !await context.Students.AnyAsync(item => item.Id == model.StudentId.Value) || !await context.DocumentTypes.AnyAsync(item => item.Id == model.DocumentTypeId.Value)) return Result.Failure("The selected student or document type does not exist.");
            if (await context.Tutors.AnyAsync(item => item.Id != id && item.DocumentNumber == model.DocumentNumber)) return Result.Failure("A tutor with that document number already exists.");
            entity.FullName = model.FullName; entity.PaternalSurname = model.PaternalSurname; entity.MaternalSurname = model.MaternalSurname; entity.DocumentNumber = model.DocumentNumber; entity.Email = model.Email; entity.Address = model.Address; entity.Relationship = model.Relationship.Value; entity.StudentId = model.StudentId.Value; entity.DocumentTypeId = model.DocumentTypeId.Value; await context.SaveChangesAsync(); return Result.Success();
        }
    }
}
