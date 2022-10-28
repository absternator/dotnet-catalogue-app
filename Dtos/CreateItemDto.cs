using System.ComponentModel.DataAnnotations;

namespace Catalogue.Dtos;

public record CreateItemDto
{
    // * use data annotation throw error if not there!!! data validation!!! awesome!!
    [Required]
    public string Name { get; init; } = String.Empty;
    [Required]
    [Range(1, 100000)]
    public decimal Price { get; init; }
}