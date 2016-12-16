using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSS_PingPong_API.Models
{
    public class TwoPlayerGame
    {
        public int playerOneId { get; set; }
        public int playerTwoId { get; set; }
        public double teamOneScore { get; set; }
        public double teamTwoScore { get; set; }
    }
}
