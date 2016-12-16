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
    public class GamesController : Controller
    {

        private NSSPingPongContext context;

        public GamesController(NSSPingPongContext ctx)
        {
            context = ctx;
        }


        // GET api/games
        [HttpGet]
        public IActionResult Get()
        {
            var games = context.Game.ToList().OrderBy(g => g.DatePlayed);

            if (games == null)
            {
                return NotFound();
            }
            foreach (Game g in games)
            {
                g.GamePlayers = context.GamePlayer.Where(gp => gp.GameId == g.GameId).ToList();
            }

            return Ok(games);
        }

        // GET api/games/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Game game = context.Game.Single(m => m.GameId == id);

                if (game == null)
                {
                    return NotFound();
                }

                game.GamePlayers = context.GamePlayer.Where(gp => gp.GameId == game.GameId).ToList();

                return Ok(game);
            }
            catch (System.InvalidOperationException ex)
            {
                return NotFound(ex);
            }
        }

        // POST api/games
        [HttpPost]
        public IActionResult Post([FromBody]Game game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.Game.Add(game);
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (GameExists(game.GameId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return Ok(game);
        }

        // PUT api/games/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Game game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (game.GameId != id)
            {
                return BadRequest(game);
            }
            try
            {
                context.Game.Update(game);
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }
            return Ok(game);
        }

        // PATCH api/games/5
        [HttpPatch("{id}")]
        public void Patch(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/games/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Game game = context.Game.Single(m => m.GameId == id);

            if (game == null)
            {
                return NotFound();
            }
            else
            {
                context.Game.Remove(game);
                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (GameExists(game.GameId))
                    {
                        return new StatusCodeResult(StatusCodes.Status403Forbidden);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Ok(game);
        }

        private bool GameExists(int id)
        {
            return context.Game.Count(e => e.GameId == id) > 0;
        }
    }
}
