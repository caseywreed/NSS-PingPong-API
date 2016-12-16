using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NSS_PingPong_API.Models;

namespace NSS_PingPong_API.Data
{
    public class NSSPingPongContext : DbContext
    {
        public NSSPingPongContext(DbContextOptions<NSSPingPongContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Player { get; set; }
        public DbSet<Game> Game { get; set; }
        public DbSet<GamePlayer> GamePlayer { get; set; }
        public DbSet<Stats> Stats { get; set; }
    }
}
