using Catalogue.Api.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalogue.Api.Repositories
{
    public class MongoDbItemsRepository : IItemRepository
    {
        private const string databaseName = "catalogue";
        private const string collectionName = "items";
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        private readonly IMongoCollection<Item> itemsCollection;
        public MongoDbItemsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            itemsCollection = database.GetCollection<Item>(collectionName);
        }

        public async Task CreateItemAsync(Item item)
        {
            await itemsCollection.InsertOneAsync(item);
        }

        public async Task DeleteItemAsync(Item item)
        {
            var filter = filterBuilder.Eq(exisitngItem => exisitngItem.Id, item.Id);
            await itemsCollection.DeleteOneAsync(filter);
        }

        public async Task<Item?> GetItemAsync(Guid id)
        {
            var filter = filterBuilder.Eq(item => item.Id, id);
            return await itemsCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await itemsCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdateItemAsync(Item item)
        {
            var filter = filterBuilder.Eq(exisitngItem => exisitngItem.Id, item.Id);
            await itemsCollection.ReplaceOneAsync(filter, item);
        }
    }
}
