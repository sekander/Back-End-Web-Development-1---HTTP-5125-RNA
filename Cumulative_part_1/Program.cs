using TeacherApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Connection String 
string cs = @"server=192.168.2.87;userid=n01001831;password=n01001831;database=http_5125";

// Using singleton to ensure there is only one instance of SchoolDb
// Add DatabaseConnection with your connection string
builder.Services.AddSingleton(new SchoolDbContext(cs));

// Add controllers and views
builder.Services.AddControllersWithViews();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHttpsRedirection();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
