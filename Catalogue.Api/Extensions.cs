using Catalogue.Api.Dtos;
using Catalogue.Api.Models;

namespace Catalogue.Api;

public static class Extensions
{
    public static ItemDto AsDto(this Item item)
    {
        return new ItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Price = item.Price,
            CreatedDate = item.CreatedDate
        };
    }
}