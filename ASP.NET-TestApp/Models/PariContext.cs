using Microsoft.EntityFrameworkCore;
using ASP.NET_TestApp.ViewModels;

namespace ASP.NET_TestApp.Models
{
    public class PariContext: DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Bet> Bets { get; set; }


        private string? _connectionString;
        public PariContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("localDb")!;
        }

        //public PariContext(DbContextOptions<PariContext> options) : base(options) 
        //{
        //    Database.EnsureCreated();
        //}
        //public PariContext()
        //{

        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString, null);
            base.OnConfiguring(optionsBuilder);
        }

        //public PariContext(DbContextOptions<PariContext> options) : base(options) 
        //{
        //    Database.EnsureCreated();
        //}
        //public PariContext()
        //{

        //}
    }
}
