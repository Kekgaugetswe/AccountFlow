using AccountFlow.Web.DataAccess;
using AccountFlow.Web.Domain.Accounts.Repositories;
using AccountFlow.Web.Domain.Persons.Repositories;
using AccountFlow.Web.Domain.Transactions.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));


   //acountfilow
var app = builder.Build();

app.UseStaticFiles();
 
if (app.Environment.IsDevelopment())
{

    app.UseDeveloperExceptionPage();
}

//ability to navigate through the pages
app.MapDefaultControllerRoute();

app.Run();
