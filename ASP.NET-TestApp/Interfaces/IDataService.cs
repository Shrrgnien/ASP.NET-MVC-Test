using ASP.NET_TestApp.Models;
using ASP.NET_TestApp.ViewModels;

namespace ASP.NET_TestApp.Interfaces
{
    public interface IDataService
    {
        void CreatePlayerAsync(Player player);
        Task<List<Player>> GetPlayersAsync();
        void EditPlayer(int id);
        void DeletePlayer(int id);
        Task<IEnumerable<ReportViewModel>> GenerateReportAsync(Status? status, bool isBetHigher = false);
        public Task RecalculateBalanceAsync(int playerId);
        public Task RecalculateBalanceAsync(Transaction transaction, bool revert = false);
        public Task RecalculateBalanceAsync(Bet bet, bool betDeleted = false);
    }
}
