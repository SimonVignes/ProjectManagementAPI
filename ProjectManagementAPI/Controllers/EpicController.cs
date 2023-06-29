using Microsoft.AspNetCore.Mvc;
using ProjectManagementAPI.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EpicController : Controller
    {

        // Creating a database object
        private SQLDatabaseContext db = new SQLDatabaseContext();

        // GET: api/Epic
        [HttpGet]
        public ActionResult<List<EpicItem>> GetEpicItems()
        {
            return Ok(db.GetEpics());
        }

        // GET api/Epic/{id}
        [HttpGet("{id}")]
        public ActionResult<EpicItem> GetEpicItem(int id)
        {
            EpicItem? epic = db.GetEpicById(id);
            if (epic == null)
            {
                return NotFound($"Project with id {id} was not found");
            }
            return Ok(epic);
        }

        // POST api/Epic
        [HttpPost]
        public ActionResult<List<EpicItem>> AddEpic(EpicItem epic)
        {
            if (db.AddEpic(epic))
            {
                return Ok(epic);
            }
            return BadRequest("Epic was not added");
        }

        // PUT api/Epic
        [HttpPut]
        public ActionResult<List<EpicItem>> UpdateEpic(EpicItem epic)
        {
            // Only return ok if epic is updated
            if (db.UpdateEpic(epic))
            {
                return NotFound(epic);
            }

            return Ok(epic);
        }

        // DELETE api/Epic/{id}
        [HttpDelete("{id}")]
        public ActionResult<List<EpicItem>> DeleteEpic(int id)
        {
            if (db.DeleteEpic(id))
            {
                return Ok($"Epic with id {id} was deleted");
            }
            return NotFound($"Epic with id {id} was not found");
        }
    }
}
