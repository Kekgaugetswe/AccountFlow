using System;
using AccountFlow.Web.Domain.Accounts.Models;
using AccountFlow.Web.Domain.Persons.Models;
using AccountFlow.Web.Domain.Transactions.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountFlow.Web.DataAccess;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options): base(options)
    {
        
    }

    public DbSet<Person> Persons {get; set;}
    public DbSet<Account> Accounts {get; set;}
    public DbSet<Transaction> Transactions { get; set; }

}
