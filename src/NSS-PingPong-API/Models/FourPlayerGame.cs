using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSS_PingPong_API.Models
{
    public class FourPlayerGame
    {
        public int playerOneId { get; set; }
        public int playerTwoId { get; set; }
        public int playerThreeId { get; set; }
        public int playerFourId { get; set; }
        public double teamOneScore { get; set; }
        public double teamTwoScore { get; set; }
    }
}
