using ASP.NET_TestApp.Models;

namespace ASP.NET_TestApp.ViewModels
{
    public class ReportViewModel
    {
        public Player Player { get; set; }
        public double TotalDepositeAmount { get; set; }
        public double TotalBetAmount { get; set; }
        public Status Status { get; set; }
    }
}
