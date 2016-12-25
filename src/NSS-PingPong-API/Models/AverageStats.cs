using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NSS_PingPong_API.Models
{
    public class AverageStats
    {
        //Core Stats
        public double Wins { get; set; }
        public double Losses { get; set; }

        //Computed Stats
        public double WinPercentage { get; set; }
        public double SinglesWinPercentage { get; set; }
        public double DoublesWinPercentage { get; set; }
        public double AvgPointDiff { get; set; }
        public double Rating { get; set; }
    }
}
