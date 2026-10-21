using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolHub.Common.Configuration;
using SchoolHub.Common.Settings;
using SchoolHub.Data;
using SchoolHub.Interfaces;
using SchoolHub.Models.Entities;
using SchoolHub.SeedDatabase;
using SchoolHub.Services;

DotEnvLoader.Load(Path.Combine(Directory.GetCurrentDirectory(), ".env"));

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionSQL")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;

    options.User.RequireUniqueEmail = false;
    // options.User.RequireUniqueEmail = true;

    //options.SignIn.RequireConfirmedAccount = true;
    //options.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "SchoolHub.Auth";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    // options.ExpireTimeSpan = TimeSpan.FromHours(2);
    options.SlidingExpiration = true;
});

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection(SmtpSettings.SectionName));
builder.Services.Configure<MercadoPagoSettings>(builder.Configuration.GetSection(MercadoPagoSettings.SectionName));
builder.Services.Configure<PaymentSettings>(builder.Configuration.GetSection(PaymentSettings.SectionName));

builder.Services.AddScoped<IAssignUserService, AssignUserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<ILevelService, LevelService>();
builder.Services.AddScoped<IGradeService, GradeService>();
builder.Services.AddScoped<ISectionService, SectionService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<IReasonEnrollmentService, ReasonEnrollmentService>();
builder.Services.AddScoped<IDocumentTypeService, DocumentTypeService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddHttpClient<IPaymentGateway, MercadoPagoPaymentGateway>((serviceProvider, client) =>
{
    var settings = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<MercadoPagoSettings>>().Value;
    client.BaseAddress = new Uri(settings.BaseUrl);
    if (!string.IsNullOrWhiteSpace(settings.AccessToken))
    {
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", settings.AccessToken);
    }
});
builder.Services.AddSingleton<MercadoPagoWebhookSignatureValidator>();

var app = builder.Build();

await SeedRole.SeedAsync(app.Services);
await SeedUser.SeedAsync(app.Services, builder.Configuration);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
