using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using SchoolHub.Common;
using SchoolHub.Common.User;
using SchoolHub.Interfaces;
using SchoolHub.Models.Entities;
using System.Text;

namespace SchoolHub.Services
{
    public class UserService(
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        ILogger<UserService> logger
    ) : IUserService
    {
        public async Task<Result<CreateUserResult>> CreateUserAsync(string userName, string email, string phoneNumber)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = email,
                PhoneNumber = phoneNumber
            };

            var identityResult = await userManager.CreateAsync(user);

            if (!identityResult.Succeeded)
            {
                string errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                logger.LogWarning("Failed to create identity user for {UserName}. Errores: {Errors}", userName, errors);

                return Result<CreateUserResult>.Failure(errors);
            }

            var createUserResult = new CreateUserResult
            {
                User = user
            };

            return Result<CreateUserResult>.Success(createUserResult);
        }

        public async Task<Result<string>> GeneratePasswordResetLinkAsync(ApplicationUser user)
        {
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext is null)
            {
                logger.LogError("Could not generate a password reset link because there is no active HTTP context.");
                return Result<string>.Failure("The password reset link could not be generated.");
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var resetPasswordPath = "/Account/ResetPassword";
            var resetPasswordUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{resetPasswordPath}";

            var link = QueryHelpers.AddQueryString(resetPasswordUrl, new Dictionary<string, string?>
            {
                ["userId"] = user.Id,
                ["token"] = encodedToken
            });

            return Result<string>.Success(link);
        }
    }
}
