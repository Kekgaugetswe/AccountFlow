using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccountFlow.Web.Domain.Persons.Models;
using AccountFlow.Web.Domain.Transactions.Models;

namespace AccountFlow.Web.Domain.Accounts.Models;

public class Account 
{
    [Key]
    public int Code { get; set; }
    [Column("person_code")]
    public int  PersonCode { get; set; }
    [Column("account_number")]
    public string AccountNumber { get; set; } = string.Empty;

    [Column("outstanding_balance")]
    public decimal OutstandingBalance { get; set; }

    [ForeignKey("PersonCode")]
    public Person? Person { get; set; }
    public ICollection<Transaction>? Transactions { get; set; }

}
