using ASP.NET_TestApp.Models;
using ASP.NET_TestApp.ViewModels;
using Org.BouncyCastle.Asn1.BC;

namespace ASP.NET_TestApp.Interfaces
{
    public interface IDataService
    {
        void CreatePlayer(Player player);
        List<Player> GetPlayers();
        void EditPlayer(int id);
        void DeletePlayer(int id);
        Task<IEnumerable<ReportViewModel>> GenerateReport(Status? status, bool? isBetHigher);
        public Task RecalculateBalance(int playerId);

        public Task RevertTransaction(Transaction transaction);
        public Task RecalculateBalance(Transaction transaction, bool revert = false);
    }
}
