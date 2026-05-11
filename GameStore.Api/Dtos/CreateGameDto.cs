using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos
{
    public record CreateGameDto(
        [Required] [StringLength(50)] string Name,
        int GenreId,
        [Range(1,150)] decimal Price,
        [Required] DateOnly ReleaseDate
     );
}
