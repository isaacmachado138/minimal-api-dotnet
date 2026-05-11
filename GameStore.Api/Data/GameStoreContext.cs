using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data
{
    public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options)
    {
        public DbSet<Game> Games { get; set; }

        public DbSet<Genre> Genres { get; set; }
    }
}
 