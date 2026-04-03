using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using NPoco;
using SixBeeHealthCare.Web.Data;
using SixBeeHealthCare.Web.Services;
using SixBeeHealthCare.Web.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

//Services

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddScoped<IDatabase>(_ =>
    new Database(new SqlConnection(connectionString), DatabaseType.SqlServer2012));

builder.Services.AddScoped<IAppointmentService, AppointmentService>();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/Login";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });


var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IDatabase>();
    SchemaInitialiser.Initialise(db);
    DbSeeder.Seed(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


public partial class Program { }
