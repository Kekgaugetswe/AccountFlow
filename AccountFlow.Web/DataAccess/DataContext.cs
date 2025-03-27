using System;
using AccountFlow.Web.Domain.Accounts.Models;
using AccountFlow.Web.Domain.Persons.Models;
using AccountFlow.Web.Domain.Transactions.Models;
using Humanizer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AccountFlow.Web.DataAccess;

public class DataContext : IdentityDbContext<ApplicationUser> 
{
    public DataContext(DbContextOptions<DataContext> options): base(options)
    {
        
    }

    public DbSet<Person> Persons {get; set;}
    public DbSet<Account> Accounts {get; set;}
    public DbSet<Transaction> Transactions { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>()
                .HasOne(u => u.Person)
                .WithOne(p => p.ApplicationUser)
                .HasForeignKey<ApplicationUser>(u=> u.PersonCode)
                .OnDelete(DeleteBehavior.Restrict);
    }

}
