using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_TestApp.Models
{
    public class Bet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public double Amount { get; set; }
        public double Gain { get; set; }
        public DateTime Date { get; set; }
        public DateTime SettlementDate { get; set; }
    }
}
