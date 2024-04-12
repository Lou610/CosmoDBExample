using GeekGroveAPI.Models;
using Microsoft.Azure.Cosmos;

namespace GeekGroveAPI.Repositry
{
    public class CharacterRepository
    {
        


        public async Task CreateGeekGroveEntityAsync(Character entity)
        {
            var cosmosClient = new CosmosClient("https://geekgrove.documents.azure.com:443/", "JPSOst1DDAo2BNZ2S1e3ifQUW9RxEV90G7hbMwaKtI6TWN5eOm3e5zeqQfIrXUJ2lHOvK5ic6VaHACDbHXlQZw==");
            var database = cosmosClient.GetDatabase("KronoStudio");
            var container = database.GetContainer("GeekGrove");


            await container.CreateItemAsync(entity, new PartitionKey(entity.Type));
        }
    }
}
