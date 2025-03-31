using AccountFlow.Web.DataAccess;
using AccountFlow.Web.Domain.Accounts.Repositories;
using AccountFlow.Web.Domain.Persons.Repositories;
using AccountFlow.Web.Domain.Persons.Services;
using AccountFlow.Web.Domain.Transactions.Repositories;
using AccountFlow.Web.Domain.Transactions.Services;
using AccountFlow.Web.Domain.Usermanagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

builder.Services.AddScoped<IPersonService,PersonService>();
builder.Services.AddScoped<ITransactionService,TransactionService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<MyService>();
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit =  true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

}).AddRoles<IdentityRole>().AddEntityFrameworkStores<DataContext>();

builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath ="/usermanagement/Login";
    options.AccessDeniedPath = "/accounth/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration =true;
});



var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));



   //acountfilow
var app = builder.Build();

// Enable session middleware

// Enable routing
app.UseRouting();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
 
if (app.Environment.IsDevelopment())
{

    app.UseDeveloperExceptionPage();
}
app.MapRazorPages();

//ability to navigate through the pages
app.MapDefaultControllerRoute();

app.Run();
