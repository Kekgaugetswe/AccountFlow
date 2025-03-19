using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccountFlow.Web.Domain.Accounts.Models;

namespace AccountFlow.Web.Domain.Transactions.Models;

public class Transaction
{

    [Key]
    public int Code { get; set; }

    [Column("account_code")]
    public int AccountCode { get; set; }

    [Column("transaction_date")]
    public DateTime TransactionDate { get; set; }

    [Column("capture_date")]
    public DateTime CaptureDate { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    [ForeignKey("AccountCode")]
    public Account Account { get; set; }

}
