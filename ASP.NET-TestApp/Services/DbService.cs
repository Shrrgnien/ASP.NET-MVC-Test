﻿using ASP.NET_TestApp.Interfaces;
using ASP.NET_TestApp.Models;
using ASP.NET_TestApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Diagnostics;
using System.Numerics;

namespace ASP.NET_TestApp.Services
{
    public class DbService : IDataService
    {
        private IConfiguration _configuration;
        public DbService(IConfiguration configuration) 
        { 
            _configuration = configuration;
        }
        public async void CreatePlayer(Player player)
        {
            using (var context = new PariContext(_configuration))
            {
                context.Players.Add(player);
                await context.SaveChangesAsync();
            }
        }

        public void DeletePlayer(int id)
        {
            throw new NotImplementedException();
        }

        public void EditPlayer(int id)
        {
            throw new NotImplementedException();
        }

        public List<Player> GetPlayers()
        {
            using (var context = new PariContext(_configuration))
            {
                var player = context.Players.FirstOrDefault();
                return context.Players.ToList();
            }
        }

        public double RecalculateBalance(IEnumerable<Transaction> transactions)
        {
            var balance = 0;
            return balance;
        }

        public async Task RecalculateBalance(Transaction transaction, bool revert = false)
        {
            using (var context = new PariContext(_configuration))
            {
                var player = await context.Players.FirstOrDefaultAsync(p => p.Id == transaction.PlayerId).ConfigureAwait(false);
                if (player != null)
                {
                    if (!revert)
                    {
                        if (transaction.Type == TransactionType.Deposit)
                        {
                            player.Balance += transaction.Amount;
                        }
                        else
                        {
                            player.Balance -= transaction.Amount;
                        }
                    }
                    else
                    {
                        if (transaction.Type == TransactionType.Deposit)
                        {
                            player.Balance -= transaction.Amount;
                        }
                        else
                        {
                            player.Balance += transaction.Amount;
                        }
                    }

                    context.Players.Update(player);
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
            }
        }


        public async Task RecalculateBalance(Bet bet, bool betDeleted = false)
        {
            using (var context = new PariContext(_configuration))
            {
                var player = await context.Players.FirstOrDefaultAsync(p => p.Id == bet.PlayerId).ConfigureAwait(false);
                { 
                    if(player != null)
                    {
                        if (!betDeleted)
                        {
                            var previousBet = await context.Bets.FindAsync(bet.Id);
                            if (previousBet != null)
                            {
                                if (previousBet.Amount != bet.Amount)
                                {
                                    player.Balance += previousBet.Amount - bet.Amount;
                                }
                                if (previousBet.Gain != bet.Gain)
                                {
                                    player.Balance += bet.Gain - previousBet.Gain;
                                }
                            }
                            else
                            {
                                player.Balance -= bet.Amount;
                                player.Balance += bet.Gain;
                            }
                        }
                        else
                        {
                            player.Balance += bet.Amount;
                            player.Balance -= bet.Gain;
                        }

                        context.Players.Update(player);
                        await context.SaveChangesAsync().ConfigureAwait(false);
                    }
                }
            }
        }

        public async Task RecalculateBalance(int playerId)
        {
            using (var context = new PariContext(_configuration))
            {
                var transactions = context.Transactions
                    .Where(t => t.PlayerId == playerId);
                double amount = 0;
                foreach(var transaction in transactions)
                {
                    if(transaction.Type == TransactionType.Deposit)
                    {
                        amount += transaction.Amount;
                    }
                    else
                    {
                        amount -= transaction.Amount;
                    }
                }

                var player = await context.Players.FirstOrDefaultAsync(p => p.Id == playerId).ConfigureAwait(false);
                if (player != null)
                {
                    player.Balance = amount;
                    context.Players.Update(player);
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }               
            }
        }

        public async Task<IEnumerable<ReportViewModel>> GenerateReport(Status? status, bool betIsHigher = false)
        {
            List<ReportViewModel> report = new List<ReportViewModel>();
            using (var context = new PariContext(_configuration))
            {
                List<Player>? players = null;

                players = await context.Players.ToListAsync().ConfigureAwait(false);
                
                if(players != null)
                {
                    if (status.HasValue)
                    {
                        players = players.Where(p => p.Status == status).ToList();
                    }
                    foreach (var player in players)
                    {
                        ReportViewModel reportRecord = new() { Player = player };
                        double totalDepositeAmount = context.Transactions
                            .Where(t => t.PlayerId == player.Id && t.Type == TransactionType.Deposit)
                            .Sum(t => t.Amount);
                        double totalBetAmount = context.Bets
                            .Where(b => b.PlayerId == player.Id).Sum(b => b.Amount);

                        reportRecord.TotalDepositeAmount = totalDepositeAmount;
                        reportRecord.TotalBetAmount = totalBetAmount;
                        report.Add(reportRecord);
                    }
                    if (betIsHigher)
                    {
                        report = report.Where(r => r.TotalBetAmount > r.TotalDepositeAmount).ToList();
                    }
                }

                return report;
            };
        }

    }
}
