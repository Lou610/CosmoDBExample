using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using GeekGroveAPI.Models;
using System.ComponentModel;
using Microsoft.AspNetCore.Cors;

namespace GeekGroveAPI.Controllers
{
    [EnableCors("HomeLab")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class CharacterController : ControllerBase
    {
        public string ServerURL =  "https://geekgrove.documents.azure.com:443/";
        public string PrimaryKey = "JPSOst1DDAo2BNZ2S1e3ifQUW9RxEV90G7hbMwaKtI6TWN5eOm3e5zeqQfIrXUJ2lHOvK5ic6VaHACDbHXlQZw==";
        public string Db = "KronoStudio";
        public string Container = "GeekGrove";


        [HttpPost("CreateCharacter")]
        public async Task CreateCharacter(Character entity)
        {
            var cosmosClient = new CosmosClient(ServerURL, PrimaryKey);
            var database = cosmosClient.GetDatabase(Db);
            var container = database.GetContainer(Container);

            Guid newGuid = Guid.NewGuid();

            entity.Id = newGuid.ToString();
            entity.Type = "Character";


            await container.CreateItemAsync(entity, new PartitionKey(entity.Type));
        }


        [HttpGet("GetCharacter")]
        public async Task<Character> GetCharacter(string id, string type)
        {
            var cosmosClient = new CosmosClient("https://geekgrove.documents.azure.com:443/", "JPSOst1DDAo2BNZ2S1e3ifQUW9RxEV90G7hbMwaKtI6TWN5eOm3e5zeqQfIrXUJ2lHOvK5ic6VaHACDbHXlQZw==");
            var database = cosmosClient.GetDatabase("KronoStudio");
            var container = database.GetContainer("GeekGrove");

            try
            {
                var response = await container.ReadItemAsync<Character>(id, new PartitionKey(type));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        [HttpPut("UpdateCharacter")]
        public async Task UpdateCharacter(Character entity)
        {
            var cosmosClient = new CosmosClient("https://geekgrove.documents.azure.com:443/", "JPSOst1DDAo2BNZ2S1e3ifQUW9RxEV90G7hbMwaKtI6TWN5eOm3e5zeqQfIrXUJ2lHOvK5ic6VaHACDbHXlQZw==");
            var database = cosmosClient.GetDatabase("KronoStudio");
            var container = database.GetContainer("GeekGrove");

            await container.ReplaceItemAsync(entity, entity.Id, new PartitionKey(entity.Type));
        }

        [HttpDelete("DeleteCharacter")]
        public async Task DeleteCharacter(string id, string type)
        {
            var cosmosClient = new CosmosClient("https://geekgrove.documents.azure.com:443/", "JPSOst1DDAo2BNZ2S1e3ifQUW9RxEV90G7hbMwaKtI6TWN5eOm3e5zeqQfIrXUJ2lHOvK5ic6VaHACDbHXlQZw==");
            var database = cosmosClient.GetDatabase("KronoStudio");
            var container = database.GetContainer("GeekGrove");

            await container.DeleteItemAsync<Character>(id, new PartitionKey(type));
        }

    }
}
