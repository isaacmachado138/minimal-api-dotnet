using GameStore.Api.Dtos;

namespace GameStore.Api.Routes
{
    public static class GameRoutes
    {
        const string GetGameEndpointName = "GetGameById";

        private static readonly List<GameDto> games = [
            new (1, "The Witcher 3", "RPG", 39.99m, new DateOnly(2015, 5, 19)),
            new (2, "Cyberpunk 2077", "RPG", 59.99m, new DateOnly(2020, 12, 10)),
            new (3, "Ages of Mythology", "Strategy", 19.99m, new DateOnly(2002, 10, 31)),
        ];
        
        public static void MapGameEndpoints (this WebApplication app)
        {
            var group = app.MapGroup("/games");
            // GET /games - return all the games in the store
            group.MapGet("/", () => games);

            // GET /games/{id} - return a game by ID
            group.MapGet("/{id}", (int id) => {
                var game = games.Find(game => game.Id == id);
                return game is not null ? Results.Ok(game) : Results.NotFound();
            }).WithName(GetGameEndpointName); //instancing the route for get game by id and  assigning a name for this route

            // POST /games - create a new game
            group.MapPost("/", (CreateGameDto newGame) =>
            {
                GameDto game = new GameDto(
                    games.Count + 1,
                    newGame.Name,
                    newGame.Genre,
                    newGame.Price,
                    newGame.ReleaseDate
                );

                games.Add(game);

                return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game); //Returns the endpoint for consult this data and the body of created object
            })
            ;

            //PUT /games/{id} - edit game by id
            group.MapPut("/{id}", (int id, UpdateGameDto gameToUpdate) =>
            {
                var index = games.FindIndex(game => game.Id == id);

                if (index == -1)
                {
                    return Results.NotFound();
                }
                else
                {
                    games[index] = new GameDto(
                        id,
                        gameToUpdate.Name,
                        gameToUpdate.Genre,
                        gameToUpdate.Price,
                        gameToUpdate.ReleaseDate
                    );
                }

                return Results.NoContent();
            });

            // DELETE /games/{id} - delete a game by ID
            group.MapDelete("/{id}", (int id) =>
            {
                games.RemoveAll(game => game.Id == id);

                return Results.NoContent();
            });
        }
    }
}
