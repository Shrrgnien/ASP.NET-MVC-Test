using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_TestApp.Models
{
    public enum TransactionType
    {
        Deposit,
        Withdrawal
    }
    public class Transaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public TransactionType Type {  get; set; }
    }
}
