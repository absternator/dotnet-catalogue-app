using Microsoft.AspNetCore.Mvc;
using Catalogue.Api.Repositories;
using Catalogue.Api.Models;
using Catalogue.Api.Dtos;


namespace Catalogue.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IItemRepository repository;
    private readonly ILogger<ItemsController> logger;

    public ItemsController(IItemRepository repository, ILogger<ItemsController> logger)
    {
        this.repository = repository;
        this.logger = logger;
    }

    /// <summary>
    /// Gets all items
    /// </summary>
    /// <returns>list of items</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemDto>>> GetItems()
    {
        var items = (await repository.GetItemsAsync()).Select(item => item.AsDto());
        logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {items.Count()} items");
        return await Task.FromResult(items.ToList());
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<ItemDto>> GetItem(Guid id)
    {
        var item = await repository.GetItemAsync(id);
        if (item is null)
        {
            return NotFound();
        }
        return item.AsDto();
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> CreateItem(CreateItemDto itemDto)
    {
        Item item = new()
        {
            Id = Guid.NewGuid(),
            Name = itemDto.Name,
            Price = itemDto.Price,
            CreatedDate = DateTimeOffset.UtcNow
        };

        await repository.CreateItemAsync(item);
        // give location where to find in header. eg.  location: https://localhost:7066/api/Items/bc16ffbc-13e6-4058-8f9b-c9b36c6a0508 
        return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item.AsDto());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> updateItem(Guid id, UpdateItemDto itemDto)
    {
        var existingItem = await repository.GetItemAsync(id);

        if (existingItem is null)
        {
            return NotFound();
        }

        // with a thing you can do with record type !! cool!!
        Item updatedItem = existingItem with
        {
            Name = itemDto.Name,
            Price = itemDto.Price
        };
        await repository.UpdateItemAsync(updatedItem);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> deleteItem(Guid id)
    {
        var existingItem = await repository.GetItemAsync(id);

        if (existingItem is null)
        {
            return NotFound();
        }


        await repository.DeleteItemAsync(existingItem);

        return NoContent();
    }
}