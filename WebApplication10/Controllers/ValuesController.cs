using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApplication10.Model;

namespace WebApplication10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public IConfiguration _configuration;

        public ValuesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Supplier>> Get()
        {
            List<Supplier> suppliers = new List<Supplier>();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetSection("Data").GetSection("ConnectionString").Value))
            {
                sqlConnection.Open();
                suppliers = sqlConnection.Query<Supplier>("exec SP_Retrieve_Supplier").ToList();
                sqlConnection.Close();
            }
            return suppliers;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Supplier> Get(int id)
        {
            Supplier supplier = new Supplier();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetSection("Data").GetSection("ConnectionString").Value))
            {
                sqlConnection.Open();
                supplier = sqlConnection.Query<Supplier>("exec SP_Retrieve_Supplier_By_Id @Id", new { Id = supplier.Id }).SingleOrDefault();
                sqlConnection.Close();
            }
            return supplier;
        }

        // POST api/values
        [HttpPost]
        public int Post([FromBody] Supplier supplier)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetSection("Data").GetSection("ConnectionString").Value))
            {
                sqlConnection.Open();
                var affectedRow = sqlConnection.Execute("exec SP_Insert_Supplier @Name", new { Name = supplier.Name });
                sqlConnection.Close();
                return affectedRow;
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public int Put(int id, [FromBody] Supplier supplier)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetSection("Data").GetSection("ConnectionString").Value))
            {
                sqlConnection.Open();
                var affectedRow = sqlConnection.Execute("exec SP_Update_Supplier @Id @Name", new { Id = id, Name = supplier.Name });
                sqlConnection.Close();
                return affectedRow;
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
