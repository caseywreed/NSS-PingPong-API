using NSS_PingPong_API.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace NSS_PingPong_API.Models
{
    public class Stats
    {
        [Key]
        public int StatsId { get; set; }
        public int PlayerId { get; set; }

        //Core Stats
        public double Wins { get; set; }
        public double Losses { get; set; }
        public double Games { get; set; }

        //Computed Stats
        public double? WinPercentage { get; set; }
        public double? SinglesWinPercentage { get; set; }
        public double? DoublesWinPercentage { get; set; }
        public double? AvgPointDiff { get; set; }
        public double? Rating { get; set; }

        public void CalculateStats(NSSPingPongContext context)
        {
            Games = Wins + Losses;
            WinPercentage = (Wins / Games) * 100;

            var gps = context.GamePlayer.Where(g => g.PlayerId == PlayerId);

            Double pointDiffCounter = 0;
            Double numOfGames = gps.Count();

            foreach (GamePlayer gp in gps)
            {
                pointDiffCounter = pointDiffCounter + (double)gp.PointDiff;
            }

            AvgPointDiff = (pointDiffCounter / numOfGames);

            var gpSinglesGames = context.GamePlayer.Where(gp => gp.PlayerId == PlayerId && gp.Singles == true).ToList().Count();
            var gpSinglesWins = context.GamePlayer.Where(gp => gp.PlayerId == PlayerId && gp.Singles == true && gp.Won == true).ToList().Count();
            var gpDoublesGames = context.GamePlayer.Where(gp => gp.PlayerId == PlayerId && gp.Singles == false).ToList().Count();
            var gpDoublesWins = context.GamePlayer.Where(gp => gp.PlayerId == PlayerId && gp.Singles == false && gp.Won == true).ToList().Count();

            if (gpSinglesGames == 0 || gpDoublesGames == 0)
            {
                Rating = 0;
            }
            else
            {

                //Problem with Rating being 10,000 more than needed
                SinglesWinPercentage = (double)gpSinglesWins / (double)gpSinglesGames;
                DoublesWinPercentage = (double)gpDoublesWins / (double)gpDoublesGames;
                Rating = ( ( (SinglesWinPercentage * .45) + (DoublesWinPercentage * .35) + (WinPercentage * .2) ) * 10 );
                SinglesWinPercentage = SinglesWinPercentage * 100;
                DoublesWinPercentage = DoublesWinPercentage * 100;
            }
        }

        public void AddWin()
        {
            Wins = Wins + 1;
        }

        public void AddLoss()
        {
            Losses = Losses + 1;
        }

        public Stats()
        {
            //Save this for later, just in case
            //var rnd = new Random();
            Wins = 0;
            Losses = 0;
            Games = Wins + Losses;
        }
    }
}
