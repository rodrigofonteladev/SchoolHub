using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using SchoolHub.Models.Entities;
using SchoolHub.Models.ViewModels.Account;
using System.Text;

namespace SchoolHub.Controllers.Security;

public class AccountController(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    ILogger<AccountController> logger) : Controller
{
    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToLocal(returnUrl);
        }

        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = model.UserNameOrEmail.Contains('@')
            ? await userManager.FindByEmailAsync(model.UserNameOrEmail)
            : await userManager.FindByNameAsync(model.UserNameOrEmail);

        if (user is null || !user.IsActive)
        {
            ModelState.AddModelError(string.Empty, "Las credenciales no son válidas o la cuenta está inactiva.");
            return View(model);
        }

        var result = await signInManager.PasswordSignInAsync(
            user.UserName!, model.Password, model.RememberMe, lockoutOnFailure: true);

        if (result.Succeeded)
        {
            logger.LogInformation("User {UserName} logged in.", user.UserName);
            return RedirectToLocal(model.ReturnUrl);
        }

        if (result.IsLockedOut)
        {
            ModelState.AddModelError(string.Empty, "La cuenta está bloqueada temporalmente.");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Las credenciales no son válidas.");
        }

        return View(model);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied() => View();

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> ResetPassword(string? userId, string? token)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
        {
            return View("ResetPasswordError");
        }

        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return View("ResetPasswordError");
        }

        if (!await IsValidPasswordResetTokenAsync(user, token))
        {
            return View("ResetPasswordError");
        }

        return View(new ResetPasswordViewModel
        {
            UserId = userId,
            Token = token
        });
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await userManager.FindByIdAsync(model.UserId);
        if (user is null)
        {
            return View("ResetPasswordError");
        }

        var decodedToken = await DecodePasswordResetTokenAsync(model.Token);
        if (decodedToken is null || !await userManager.VerifyUserTokenAsync(
                user,
                userManager.Options.Tokens.PasswordResetTokenProvider,
                TokenOptions.DefaultProvider,
                decodedToken))
        {
            return View("ResetPasswordError");
        }

        var result = await userManager.ResetPasswordAsync(user, decodedToken, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        user.IsActive = true;
        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            foreach (var error in updateResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        return View("ResetPasswordSuccess");
    }

    private async Task<bool> IsValidPasswordResetTokenAsync(ApplicationUser user, string token)
    {
        var decodedToken = await DecodePasswordResetTokenAsync(token);
        return decodedToken is not null && await userManager.VerifyUserTokenAsync(
            user,
            userManager.Options.Tokens.PasswordResetTokenProvider,
            TokenOptions.DefaultProvider,
            decodedToken);
    }

    private static Task<string?> DecodePasswordResetTokenAsync(string token)
    {
        try
        {
            return Task.FromResult<string?>(Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token)));
        }
        catch (FormatException)
        {
            return Task.FromResult<string?>(null);
        }
    }

    private IActionResult RedirectToLocal(string? returnUrl) =>
        !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
            ? Redirect(returnUrl)
            : RedirectToAction("Index", "Home")!;
}
