using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos
{
    public record UpdateGameDto (
        [Required] int Id,
        [Required][StringLength(50)] string Name,
        [Required] int GenreId,
        [Range(1, 150)] decimal Price,
        [Required] DateOnly ReleaseDate
    );
}
