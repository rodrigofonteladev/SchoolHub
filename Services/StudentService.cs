using SchoolHub.Common;
using SchoolHub.Common.Enums;
using SchoolHub.Data;
using SchoolHub.Helpers;
using SchoolHub.Interfaces;
using SchoolHub.Models.Entities;
using SchoolHub.Models.ViewModels.Student;
using Microsoft.EntityFrameworkCore;

namespace SchoolHub.Services
{
    public class StudentService(
        AppDbContext context,
        IUserService userService,
        IRoleService roleService,
        IAssignUserService assignUserService,
        IEmailService emailService,
        IEnrollmentService enrollmentService,
        ILogger<StudentService> logger
    ) : IStudentService
    {
        public async Task<List<ListStudentViewModel>> GetAllAsync()
        {
            return await context.Students
                .AsNoTracking()
                .OrderBy(student => student.PaternalSurname)
                .ThenBy(student => student.MaternalSurname)
                .ThenBy(student => student.FullName)
                .Select(student => new ListStudentViewModel
                {
                    Id = student.Id,
                    FullName = student.FullName,
                    PaternalSurname = student.PaternalSurname,
                    MaternalSurname = student.MaternalSurname,
                    DocumentNumber = student.DocumentNumber,
                    DocumentTypeName = student.DocumentType.Name,
                    Email = student.Email,
                    Birthday = student.Birthday,
                    PointsBalance = student.PointsBalance,
                    IsActive = student.IsActive,
                    TutorCount = student.Tutors.Count
                })
                .ToListAsync();
        }

        public async Task<Result> CreateAsync(RegisterStudentViewModel model)
        {
            await using var ts = await context.Database.BeginTransactionAsync();

            try
            {
                // Crear estudiante
                var student = new Student
                {
                    FullName = model.FullName,
                    PaternalSurname = model.PaternalSurname,
                    MaternalSurname = model.MaternalSurname,
                    DocumentNumber = model.DocumentNumber,
                    Email = model.Email,
                    Address = model.Address,
                    Birthday = model.Birthday!.Value,
                    DocumentTypeId = model.DocumentTypeId!.Value
                };

                // Crear tutores
                foreach (var tutorModel in model.Tutors)
                {
                    student.Tutors.Add(new Tutor
                    {
                        FullName = tutorModel.FullName,
                        PaternalSurname = tutorModel.PaternalSurname,
                        MaternalSurname = tutorModel.MaternalSurname,
                        DocumentNumber = tutorModel.DocumentNumber,
                        Email = tutorModel.Email,
                        Address = tutorModel.Address,
                        Relationship = tutorModel.Relationship!.Value,
                        DocumentTypeId = tutorModel.DocumentTypeId!.Value
                    });
                }

                await context.Students.AddAsync(student);
                await context.SaveChangesAsync();

                string userName = UserUtility.GenerateUserName(student.Id, student.PaternalSurname, student.MaternalSurname);
                string email = $"{userName}@schoolhub.edu.pe";

                // Crear matricula (internamente genera el pago de matricula)
                var enrollmentResult = await enrollmentService.CreateEnrollmentAsync(student.Id, model.SectionId!.Value, model.ReasonEnrollmentId!.Value);
                if (!enrollmentResult.Succeeded) return Result.Failure(enrollmentResult.Error!);

                // Crear usuario
                var userResult = await userService.CreateUserAsync(userName, email, model.PhoneNumber);
                if (!userResult.Succeeded) return Result.Failure(userResult.Error!);

                // Asignar rol
                var roleResult = await roleService.AssignRoleAsync(userResult.Value!.User!, Role.Student.ToString());
                if (!roleResult.Succeeded) return Result.Failure(roleResult.Error!);

                // Crear asignacion de usuario (user accounts)
                await assignUserService.CreateAssignUserAccountAsync($"S-{student.Id}", student.Id, userResult.Value?.User?.Id!);

                // Generar enlace de configuración de contraseña
                var passwordResetLinkResult = await userService.GeneratePasswordResetLinkAsync(userResult.Value!.User!);
                if (!passwordResetLinkResult.Succeeded)
                {
                    return Result.Failure(passwordResetLinkResult.Error!);
                }

                // Confirmar transaccion
                await ts.CommitAsync();

                logger.LogInformation($"Student successfully registered. DocumentNumber: {model.DocumentNumber}");

                var userEmail = userResult.Value?.User?.Email!;

                // Enviar enlace para configurar la contraseña al correo del estudiante
                var sendEmail = await emailService.SendStudentPasswordResetLinkAsync(
                    model.Email,
                    model.FullName,
                    userName,
                    userEmail,
                    passwordResetLinkResult.Value!);
                if (!sendEmail.Succeeded) logger.LogWarning($"The student was registered, but the email could not be sent: {sendEmail.Error}");

                return Result.Success();
            }
            catch (Exception ex)
            {
                await ts.RollbackAsync();

                logger.LogError(
                    ex,
                    $"A critical error ocurred while registering the student with document number: {model.DocumentNumber}\n Message: {ex.Message}"
                );

                return Result.Failure("An error occurred during registration.");
            }
        }

        public async Task<EditStudentViewModel?> GetForEditAsync(int id) => await context.Students.AsNoTracking().Where(item => item.Id == id).Select(item => new EditStudentViewModel
        { FullName = item.FullName, PaternalSurname = item.PaternalSurname, MaternalSurname = item.MaternalSurname, DocumentNumber = item.DocumentNumber, Email = item.Email, Address = item.Address, Birthday = item.Birthday, DocumentTypeId = item.DocumentTypeId }).FirstOrDefaultAsync();

        public async Task<Result> UpdateAsync(int id, EditStudentViewModel model)
        {
            var entity = await context.Students.FindAsync(id);
            if (entity is null) return Result.Failure("The student does not exist.");
            if (!await context.DocumentTypes.AnyAsync(item => item.Id == model.DocumentTypeId)) return Result.Failure("The selected document type does not exist.");
            if (await context.Students.AnyAsync(item => item.Id != id && item.DocumentNumber == model.DocumentNumber)) return Result.Failure("A student with that document number already exists.");
            entity.FullName = model.FullName; entity.PaternalSurname = model.PaternalSurname; entity.MaternalSurname = model.MaternalSurname; entity.DocumentNumber = model.DocumentNumber; entity.Email = model.Email; entity.Address = model.Address; entity.Birthday = model.Birthday!.Value; entity.DocumentTypeId = model.DocumentTypeId!.Value;
            await context.SaveChangesAsync(); return Result.Success();
        }


    }
}
