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

namespace WebApplication10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        public IConfiguration _configuration;
        public SqlConnection sqlConnection;

        public SuppliersController(IConfiguration configuration)
        {
            _configuration = configuration;
            sqlConnection = new SqlConnection(_configuration.GetSection("Data").GetSection("ConnectionString").Value);
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Supplier>> Get()
        {
            var suppliers = sqlConnection.Query<Supplier>("exec SP_Retrieve_Supplier").ToList();
            return suppliers;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Supplier> Get(int id)
        {
            var supplier = sqlConnection.Query<Supplier>("exec SP_Retrieve_Supplier_By_Id @Id", new { Id = id }).SingleOrDefault();
            return supplier;
        }

        // POST api/values
        [HttpPost]
        public int Post([FromBody] Supplier supplier)
        {
            var affectedRow = sqlConnection.Execute("exec SP_Insert_Supplier @Name", new { Name = supplier.Name });
            return affectedRow;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public int Put(int id, [FromBody] Supplier supplier)
        {
            var affectedRow = sqlConnection.Execute("exec SP_Update_Supplier @Id @Name", new { Id = id, Name = supplier.Name });
            return affectedRow;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public int Delete(int id)
        {
            var affectedRows = sqlConnection.Execute("exec SP_Delete_Supplier @Id", new { id = id });
            return affectedRows;
        }
    }
}