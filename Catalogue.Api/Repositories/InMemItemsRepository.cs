// using Catalogue.Api.Models;

// namespace Catalogue.Api.Repositories;


// public class InMemoryItemRepository : IItemRepository
// {
//     private readonly List<Item> items = new()
//         {
//             new Item {Id = Guid.NewGuid(), Name = "Potion", Price = 10, CreatedDate = DateTimeOffset.UtcNow },
//             new Item {Id = Guid.NewGuid(), Name = "Sword", Price = 22, CreatedDate = DateTimeOffset.UtcNow },
//             new Item {Id = Guid.NewGuid(), Name = "Sheild", Price = 15, CreatedDate = DateTimeOffset.UtcNow },

//         };
//     public IEnumerable<Item> GetItemsAsync()
//     {
//         return items;
//     }

//     public Item? GetItemAsync(Guid id)
//     {
//         // use find in this case usually
//         return items.Where(item => item.Id == id).SingleOrDefault();
//     }

//     public void CreateItemAsync(Item item)
//     {
//         items.Add(item);
//     }

//     public void UpdateItemAsync(Item item)
//     {
//         int index = items.FindIndex(i => i.Id == item.Id);
//         items[index] = item;
//     }

//     public void DeleteItemAsync(Item item)
//     {
//         items.RemoveAll(i => i.Id == item.Id);

//     }
// }
