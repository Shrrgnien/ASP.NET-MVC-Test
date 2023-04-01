using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_TestApp.Models
{
    public enum Status
    {
        New,
        Bad,
        Good
    }
    public class Player
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FullName { get; set; }
        public double Balance { get; set; }
        public DateTime RegistarionDate { get; set; }
        public Status Status { get; set; }
    }
}
