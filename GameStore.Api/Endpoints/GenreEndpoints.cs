using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints
{
    public static class GenreEndpoints
    {
        public static void MapGenreEndpoints(this WebApplication app)
        {

            var group = app.MapGroup("/genres");

            group.MapGet("/", async (GameStoreContext dbContext) =>
                await dbContext.Genres
                    .Select(genre => new GenreDto(genre.Id, genre.Name))
                    .AsNoTracking()
                    .ToListAsync());

            group.MapPost("/", (GameStoreContext dbContext, CreateGenreDto createGenreDto) =>
            {
                var genre = new Genre
                {
                    Name = createGenreDto.Name
                };

                dbContext.Genres.Add(genre);
                dbContext.SaveChanges();

                return Results.Created($"/genres/{genre.Id}", new GenreDto(genre.Id, genre.Name));
            });
        }
    }
}
