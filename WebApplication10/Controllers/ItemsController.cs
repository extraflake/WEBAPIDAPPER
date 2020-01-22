using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApplication10.Model;
using WebApplication10.ViewModel;

namespace WebApplication10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        public IConfiguration _configuration;
        public SqlConnection sqlConnection;

        public ItemsController(IConfiguration configuration)
        {
            _configuration = configuration;
            sqlConnection = new SqlConnection(_configuration.GetSection("Data").GetSection("ConnectionString").Value);
        }

        [HttpGet]
        public IEnumerable<ItemVM> Get()
        {
            var items = sqlConnection.QueryAsync<ItemVM>("exec SP_Retrieve_Item").Result.ToList();
            return items;
        }

        [HttpGet("{id}")]
        public ItemVM Get(int id)
        {
            var item = sqlConnection.QueryAsync<ItemVM>("exec SP_Retrieve_Item_By_Id @Id", new { Id = id }).Result.SingleOrDefault();
            return item;
        }

        [HttpPost]
        public async Task<int> Insert(Item item)
        {
            var affectedRow = await sqlConnection.ExecuteAsync("exec SP_Insert_Item @Name, @Price, @Stock, @Supplier_Id", new { Name = item.Name, Price = item.Price, Stock = item.Stock, Supplier_Id = item.Supplier_Id });
            return affectedRow;
        }

        [HttpPut("{id}")]
        public async Task<int> Update(int id, Item item)
        {
            var affectedRow = await sqlConnection.ExecuteAsync("exec SP_Update_Item @Id, @Name, @Price, @Stock, @Supplier_Id", new { Id = id, Name = item.Name, Price = item.Price, Stock = item.Stock, Supplier_Id = item.Supplier_Id });
            return affectedRow;
        }

        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            var affectedRow = await sqlConnection.ExecuteAsync("exec SP_Delete_Item @Id", new { Id = id });
            return affectedRow;
        }
    }
}