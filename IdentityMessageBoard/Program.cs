using IdentityMessageBoard.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using IdentityMessageBoard.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MessageBoardContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("IdentityMessageBoardDb")).UseSnakeCaseNamingConvention());

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MessageBoardContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
