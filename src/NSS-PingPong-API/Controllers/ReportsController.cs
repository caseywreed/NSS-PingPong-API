using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSS_PingPong_API.Data;
using NSS_PingPong_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;

namespace NSS_PingPong_API.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowDevelopmentEnvironment")]
    public class ReportsController : Controller
    {

        private NSSPingPongContext context;

        public ReportsController(NSSPingPongContext ctx)
        {
            context = ctx;
        }

        // Two Player Game Report
        // POST api/Reports

        [HttpPost]
        [Route("TwoPlayerGame")]
        public IActionResult Post([FromBody]TwoPlayerGame model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Game game = new Game();

            var gamePlayers = new GamePlayer[]
            {
                new GamePlayer(model.playerOneId, 1, game.GameId, true),
                new GamePlayer(model.playerTwoId, 2, game.GameId, true)
            };

            foreach (GamePlayer g in gamePlayers)
            {
                context.GamePlayer.Add(g);
            }

            game.GamePlayers = gamePlayers.ToList();
            game.TeamOneScore = model.teamOneScore;
            game.TeamTwoScore = model.teamTwoScore;

            //Check scores against each other to calculate Point Differential
            //and decide who won

            if (model.teamOneScore > model.teamTwoScore)
            {
                //Modify the GamePlayer instances
                gamePlayers[0].Won = true;
                gamePlayers[0].PointDiff = (model.teamOneScore - model.teamTwoScore);
                gamePlayers[1].Won = false;
                gamePlayers[1].PointDiff = (model.teamTwoScore - model.teamOneScore);

                //Modify the Stats classes for each player
                var playerOneStats = context.Stats.Single(s => s.PlayerId == model.playerOneId);
                var playerTwoStats = context.Stats.Single(s => s.PlayerId == model.playerTwoId);
                playerOneStats.AddWin();
                playerTwoStats.AddLoss();
            }
            else
            {
                gamePlayers[0].Won = false;
                gamePlayers[0].PointDiff = (model.teamOneScore - model.teamTwoScore);
                gamePlayers[1].Won = true;
                gamePlayers[1].PointDiff = (model.teamTwoScore - model.teamOneScore);

                //Modify the Stats classes for each player
                var playerOneStats = context.Stats.Single(s => s.PlayerId == model.playerOneId);
                var playerTwoStats = context.Stats.Single(s => s.PlayerId == model.playerTwoId);
                playerTwoStats.AddWin();
                playerOneStats.AddLoss();
            }

            context.Game.Add(game);
            context.SaveChanges();
            return Ok(game);
        }

        [HttpPost]
        [Route("FourPlayerGame")]
        public IActionResult Post([FromBody]FourPlayerGame model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Game game = new Game();

            var gamePlayers = new GamePlayer[]
            {
                new GamePlayer(model.playerOneId, 1, game.GameId, false),
                new GamePlayer(model.playerTwoId, 1, game.GameId, false),
                new GamePlayer(model.playerThreeId, 2, game.GameId, false),
                new GamePlayer(model.playerFourId, 2, game.GameId, false)
            };

            foreach (GamePlayer g in gamePlayers)
            {
                context.GamePlayer.Add(g);
            }

            game.GamePlayers = gamePlayers.ToList();
            game.TeamOneScore = model.teamOneScore;
            game.TeamTwoScore = model.teamTwoScore;

            //Check scores against each other to calculate Point Differential
            //and decide who won

            if (model.teamOneScore > model.teamTwoScore)
            {
                //Team One Win
                gamePlayers[0].Won = true;
                gamePlayers[0].PointDiff = (model.teamOneScore - model.teamTwoScore);
                gamePlayers[1].Won = true;
                gamePlayers[1].PointDiff = (model.teamOneScore - model.teamTwoScore);
                //Team Two Loss
                gamePlayers[2].Won = false;
                gamePlayers[2].PointDiff = (model.teamTwoScore - model.teamOneScore);
                gamePlayers[3].Won = false;
                gamePlayers[3].PointDiff = (model.teamTwoScore - model.teamOneScore);

                //Modify the Stats classes for each player
                var playerOneStats = context.Stats.Single(s => s.PlayerId == model.playerOneId);
                var playerTwoStats = context.Stats.Single(s => s.PlayerId == model.playerTwoId);
                playerOneStats.AddWin();
                playerTwoStats.AddWin();
                var playerThreeStats = context.Stats.Single(s => s.PlayerId == model.playerThreeId);
                var playerFourStats = context.Stats.Single(s => s.PlayerId == model.playerFourId);
                playerThreeStats.AddWin();
                playerFourStats.AddWin();

            }
            else
            {
                //Team One Loss
                gamePlayers[0].Won = false;
                gamePlayers[0].PointDiff = (model.teamOneScore - model.teamTwoScore);
                gamePlayers[1].Won = false;
                gamePlayers[1].PointDiff = (model.teamOneScore - model.teamTwoScore);
                //Team Two Win
                gamePlayers[2].Won = true;
                gamePlayers[2].PointDiff = (model.teamTwoScore - model.teamOneScore);
                gamePlayers[3].Won = true;
                gamePlayers[3].PointDiff = (model.teamTwoScore - model.teamOneScore);
            }

            context.Game.Add(game);
            context.SaveChanges();
            return Ok(game);
        }

        [HttpGet]
        [Route("GetAverageStats")]
        public IActionResult Get()
        {
            var statsList = context.Stats.ToList();
            var statsCount = statsList.Count();

            var singlesGames = context.GamePlayer.Where(gp => gp.Singles == true).ToList().Count();
            var singlesWins = context.GamePlayer.Where(gp => gp.Singles == true && gp.Won == true).ToList().Count();
            var doublesGames = context.GamePlayer.Where(gp => gp.Singles == false).ToList().Count();
            var doublesWins = context.GamePlayer.Where(gp => gp.Singles == false && gp.Won == true).ToList().Count();

            double winPercentCounter = 0;
            double singlesWinPercentCounter = 0;
            double doublesWinPercentCounter = 0;
            double pointDiffCounter = 0;
            double winCounter = 0;
            double lossCounter = 0;
            double ratingCounter = 0; 

            foreach (Stats s in statsList)
            {
                s.CalculateStats(context);
                if ( Double.IsNaN((double)s.WinPercentage) )
                {
                    s.WinPercentage = 0;
                }
                if (s.SinglesWinPercentage == null)
                {
                    s.SinglesWinPercentage = 0;
                }
                if (s.DoublesWinPercentage == null)
                {
                    s.DoublesWinPercentage = 0;
                }
                if (Double.IsNaN((double)s.AvgPointDiff))
                {
                    s.AvgPointDiff = 0;
                }
                if (Double.IsNaN((double)s.Rating))
                {
                    s.Rating = 0;
                }

                winCounter = winCounter + s.Wins;
                lossCounter = lossCounter + s.Losses;
                singlesWinPercentCounter = singlesWinPercentCounter + (double)s.SinglesWinPercentage;
                doublesWinPercentCounter = doublesWinPercentCounter + (double)s.DoublesWinPercentage;
                winPercentCounter = winPercentCounter + (double)s.WinPercentage;
                winPercentCounter = winPercentCounter + (double)s.WinPercentage;
                pointDiffCounter = pointDiffCounter + (double)s.AvgPointDiff;
                ratingCounter = ratingCounter + (double)s.Rating;
            }

            var averageStats = new AverageStats()
            {
                Wins = winCounter / statsCount,
                Losses = lossCounter / statsCount,
                WinPercentage = winPercentCounter / statsCount,
                SinglesWinPercentage = singlesWinPercentCounter / statsCount,
                DoublesWinPercentage = doublesWinPercentCounter / statsCount,
                AvgPointDiff = pointDiffCounter / statsCount,
                Rating = ratingCounter / statsCount
            };

            return Ok(averageStats);
        }
    }
}



