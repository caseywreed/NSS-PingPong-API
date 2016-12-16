using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NSS_PingPong_API.Models
{
    public class Game
    {
        [Key]
        public int GameId { get; set; }

        public DateTime DatePlayed { get; set; }
        public ICollection<GamePlayer> GamePlayers { get; set; }
        public double TeamOneScore { get; set; }
        public double TeamTwoScore { get; set; }

        public Game()
        {
            DatePlayed = DateTime.Now;
        }
    }
}
