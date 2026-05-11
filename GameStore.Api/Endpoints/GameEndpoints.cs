using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints
{
    public static class GameEndpoints
    {
        const string GetGameEndpointName = "GetGameById";

        public static void MapGameEndpoints (this WebApplication app)
        {
            var group = app.MapGroup("/games");
            // GET /games - return all the games in the store
            group.MapGet("/", async (GameStoreContext dbContext) =>
                await dbContext.Games
                    .Include(game => game.Genre) // Include the Genre navigation property to access the genre name
                    .Select(game => new GameSummaryDto(
                        game.Id,
                        game.Name,
                        game.Genre!.Name,
                        game.Price,
                        game.ReleaseDate))
                    .AsNoTracking()
                    .ToListAsync());

            // GET /games/{id} - return a game by ID
            group.MapGet("/{id}", async (int id, GameStoreContext dbContext) => {
                var game = await dbContext.Games.FindAsync(id);

                return game is not null ? Results.Ok(
                    new GameDetailsDto(
                        game.Id,
                        game.Name,
                        game.GenreId,
                        game.Price,
                        game.ReleaseDate
                    )) : Results.NotFound();

            }).WithName(GetGameEndpointName); //instancing the route for get game by id and  assigning a name for this route

            // POST /games - create a new game
            group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
            {
                Game game = new()
                {
                    Name = newGame.Name,
                    GenreId = newGame.GenreId,
                    Price = newGame.Price,
                    ReleaseDate = newGame.ReleaseDate
                };

                dbContext.Games.Add(game);
                await dbContext.SaveChangesAsync();

                GameDetailsDto gameDetails = new(
                    game.Id,
                    game.Name,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate
                );
                return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, gameDetails); //Returns the endpoint for consult this data and the body of created object
            })
            ;

            //PUT /games/{id} - edit game by id
            group.MapPut("/{id}", async (int id, UpdateGameDto gameToUpdate, GameStoreContext dbContext) =>
            {
                var existingGame = await dbContext.Games.FindAsync(id);

                if (existingGame is null)
                {
                    return Results.NotFound();
                }

                existingGame.Name = gameToUpdate.Name;
                existingGame.GenreId = gameToUpdate.GenreId;
                existingGame.Price = gameToUpdate.Price;
                existingGame.ReleaseDate = gameToUpdate.ReleaseDate;

                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            });

            // DELETE /games/{id} - delete a game by ID
            group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
            {
                int affectedRows = await dbContext.Games
                    .Where(game => game.Id == id)
                    .ExecuteDeleteAsync();

                return affectedRows == 0
                    ? Results.NotFound()
                    : Results.NoContent();
            });
        }
    }
}
