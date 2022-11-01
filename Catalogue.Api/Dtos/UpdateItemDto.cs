using System.ComponentModel.DataAnnotations;

namespace Catalogue.Api.Dtos;

public record UpdateItemDto
{
    // * use data annotation throw error if not there!!! data validation!!! awsome!!
    [Required]
    public string Name { get; init; } = String.Empty;
    [Required]
    [Range(1, 100000)]
    public decimal Price { get; init; }
}