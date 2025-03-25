using AccountFlow.Web.DataAccess;
using AccountFlow.Web.Domain.Accounts.Repositories;
using AccountFlow.Web.Domain.Persons.Repositories;
using AccountFlow.Web.Domain.Persons.Services;
using AccountFlow.Web.Domain.Transactions.Repositories;
using AccountFlow.Web.Domain.Transactions.Services;
using AccountFlow.Web.Domain.Usermanagement;
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



var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout duration
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



   //acountfilow
var app = builder.Build();

// Enable session middleware
app.UseSession();

// Enable routing
app.UseStaticFiles();
app.UseRouting();
app.UseStaticFiles();
 
if (app.Environment.IsDevelopment())
{

    app.UseDeveloperExceptionPage();
}

//ability to navigate through the pages
app.MapDefaultControllerRoute();

app.Run();
