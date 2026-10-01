using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using EtteremAPI.Models;

namespace EtteremAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RendelesController : ControllerBase
    {
        private readonly string connectionString;

        public RendelesController(IConfiguration configuration)
        {
            connectionString =
                configuration.GetConnectionString("EtteremDb");
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            List<Rendeles> rendelesek = new List<Rendeles>();

            using MySqlConnection connection =
                new MySqlConnection(connectionString);

            connection.Open();

            string sql = @"
                SELECT id, dish, description,
                       orderTime, updateTime, vendegId
                FROM rendeles
                ORDER BY id";

            using MySqlCommand command =
                new MySqlCommand(sql, connection);

            using MySqlDataReader reader =
                command.ExecuteReader();

            while (reader.Read())
            {
                rendelesek.Add(new Rendeles
                {
                    Id = reader.GetInt32("id"),

                    Dish = reader.GetString("dish"),

                    Description = reader.IsDBNull(
                        reader.GetOrdinal("description"))
                        ? null
                        : reader.GetString("description"),

                    OrderTime = reader.GetDateTime("orderTime"),

                    UpdateTime = reader.GetDateTime("updateTime"),

                    VendegId = reader.GetInt32("vendegId")
                });
            }

            return Ok(rendelesek);
        }


    

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            using MySqlConnection connection =
                new MySqlConnection(connectionString);

            connection.Open();

            string sql = @"
                SELECT id, dish, description,
                       orderTime, updateTime, vendegId
                FROM rendeles
                WHERE id = @id";

            using MySqlCommand command =
                new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue("@id", id);

            using MySqlDataReader reader =
                command.ExecuteReader();

            if (!reader.Read())
            {
                return NotFound();
            }

            Rendeles rendeles = new Rendeles
            {
                Id = reader.GetInt32("id"),

                Dish = reader.GetString("dish"),

                Description = reader.IsDBNull(
                    reader.GetOrdinal("description"))
                    ? null
                    : reader.GetString("description"),

                OrderTime = reader.GetDateTime("orderTime"),

                UpdateTime = reader.GetDateTime("updateTime"),

                VendegId = reader.GetInt32("vendegId")
            };

            return Ok(rendeles);
        }


        [HttpPost]
        public IActionResult Create(Rendeles rendeles)
        {
            if (rendeles == null)
            {
                return BadRequest();
            }

            DateTime now = DateTime.Now;

            using MySqlConnection connection =
                new MySqlConnection(connectionString);

            connection.Open();

            string sql = @"
                INSERT INTO rendeles
                (
                    dish,
                    description,
                    orderTime,
                    updateTime,
                    vendegId
                )
                VALUES
                (
                    @dish,
                    @description,
                    @orderTime,
                    @updateTime,
                    @vendegId
                );

                SELECT LAST_INSERT_ID();";

            using MySqlCommand command =
                new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue(
                "@dish",
                rendeles.Dish);

            command.Parameters.AddWithValue(
                "@description",
                (object?)rendeles.Description ?? DBNull.Value);

            command.Parameters.AddWithValue(
                "@orderTime",
                now);

            command.Parameters.AddWithValue(
                "@updateTime",
                now);

            command.Parameters.AddWithValue(
                "@vendegId",
                rendeles.VendegId);

            int id = Convert.ToInt32(
                command.ExecuteScalar());

            rendeles.Id = id;
            rendeles.OrderTime = now;
            rendeles.UpdateTime = now;

            return CreatedAtAction(
                nameof(GetById),
                new { id = id },
                rendeles);
        }


        [HttpPut("{id}")]
        public IActionResult Update(
            int id,
            Rendeles rendeles)
        {
            if (rendeles == null)
            {
                return BadRequest();
            }

            using MySqlConnection connection =
                new MySqlConnection(connectionString);

            connection.Open();

            string checkSql = @"
                SELECT COUNT(*)
                FROM rendeles
                WHERE id = @id";

            using MySqlCommand checkCommand =
                new MySqlCommand(checkSql, connection);

            checkCommand.Parameters.AddWithValue(
                "@id",
                id);

            int count = Convert.ToInt32(
                checkCommand.ExecuteScalar());

            if (count == 0)
            {
                return NotFound();
            }

            DateTime now = DateTime.Now;

            string sql = @"
                UPDATE rendeles
                SET
                    dish = @dish,
                    description = @description,
                    vendegId = @vendegId,
                    updateTime = @updateTime
                WHERE id = @id";

            using MySqlCommand command =
                new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue(
                "@dish",
                rendeles.Dish);

            command.Parameters.AddWithValue(
                "@description",
                (object?)rendeles.Description ?? DBNull.Value);

            command.Parameters.AddWithValue(
                "@vendegId",
                rendeles.VendegId);

            command.Parameters.AddWithValue(
                "@updateTime",
                now);

            command.Parameters.AddWithValue(
                "@id",
                id);

            command.ExecuteNonQuery();

            rendeles.Id = id;
            rendeles.UpdateTime = now;

            return Ok(rendeles);
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using MySqlConnection connection =
                new MySqlConnection(connectionString);

            connection.Open();

            string sql = @"
                DELETE FROM rendeles
                WHERE id = @id";

            using MySqlCommand command =
                new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue(
                "@id",
                id);

            int affectedRows =
                command.ExecuteNonQuery();

            if (affectedRows == 0)
            {
                return NotFound();
            }

            return Ok("A rendelés törölve.");
        }


        [HttpGet("vendeg/{id}")]
        public IActionResult GetVendeg(int id)
        {
            using MySqlConnection connection =
                new MySqlConnection(connectionString);

            connection.Open();

            string sql = @"
                SELECT name, email
                FROM vendeg
                WHERE id = @id";

            using MySqlCommand command =
                new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue(
                "@id",
                id);

            using MySqlDataReader reader =
                command.ExecuteReader();

            if (!reader.Read())
            {
                return NotFound();
            }

            var result = new
            {
                Name = reader.GetString("name"),
                Email = reader.GetString("email")
            };

            return Ok(result);
        }



        [HttpGet("vendeg/{id}/rendelesek")]
        public IActionResult GetVendegRendelesek(int id)
        {
            using MySqlConnection connection =
                new MySqlConnection(connectionString);

            connection.Open();

            string sql = @"
                SELECT
                    v.name,
                    r.dish,
                    r.description
                FROM vendeg v
                LEFT JOIN rendeles r
                    ON v.id = r.vendegId
                WHERE v.id = @id";

            using MySqlCommand command =
                new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue(
                "@id",
                id);

            using MySqlDataReader reader =
                command.ExecuteReader();

            List<object> result =
                new List<object>();

            while (reader.Read())
            {
                result.Add(new
                {
                    Name = reader.GetString("name"),

                    Dish = reader.IsDBNull(
                        reader.GetOrdinal("dish"))
                        ? null
                        : reader.GetString("dish"),

                    Description = reader.IsDBNull(
                        reader.GetOrdinal("description"))
                        ? null
                        : reader.GetString("description")
                });
            }

            if (result.Count == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }




        [HttpGet("count")]
        public IActionResult GetCount()
        {
            using MySqlConnection connection =
                new MySqlConnection(connectionString);

            connection.Open();

            string sql =
                "SELECT COUNT(*) FROM rendeles";

            using MySqlCommand command =
                new MySqlCommand(sql, connection);

            int count = Convert.ToInt32(
                command.ExecuteScalar());

            return Ok(new
            {
                Count = count
            });
        }


   
        [HttpGet("vendeg/{id}/count")]
        public IActionResult GetVendegCount(int id)
        {
            using MySqlConnection connection =
                new MySqlConnection(connectionString);

            connection.Open();

            string sql = @"
                SELECT COUNT(*)
                FROM rendeles
                WHERE vendegId = @id";

            using MySqlCommand command =
                new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue(
                "@id",
                id);

            int count = Convert.ToInt32(
                command.ExecuteScalar());

            return Ok(new
            {
                VendegId = id,
                Count = count
            });
        }
    }
}