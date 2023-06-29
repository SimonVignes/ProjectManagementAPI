using Microsoft.AspNetCore.Mvc;
using ProjectManagementAPI.Models;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TaskController : Controller
{

    // Creating database object
    private SQLDatabaseContext db = new SQLDatabaseContext();

    // GET api/Task
    [HttpGet]
    public ActionResult<List<SubTaskItem>> GetSubTaskItems()
    {
        // Getting all subtasks from database
        return Ok(db.GetSubTasks());

    }

    // GET api/Task/{id}
    [HttpGet("{id}")]
    public ActionResult<SubTaskItem> GetSubTaskItem(int id)
    {
        // Getting subtask by id from database
        SubTaskItem? subTask = db.GetSubTaskById(id);
        if (subTask != null)
        {
            return Ok(subTask);
        }
        return NotFound($"Subtask with id {id} was not found");

    }

    // POST api/Task
    [HttpPost]
    public ActionResult<List<SubTaskItem>> AddTask(SubTaskItem subTask)
    {
        // Adding subtask to database
        if (db.AddSubTask(subTask))
        {
            return Ok(subTask);
        }
        return BadRequest("Subtask was not added");
    }

    // PUT api/Task
    [HttpPut]
    public ActionResult<List<SubTaskItem>> UpdateTask(SubTaskItem subTask)
    {
        // Updating subtask in database
        if (db.UpdateSubTask(subTask))
        {
            return Ok(db.GetSubTasks());
        }
        return NotFound(subTask);

    }

    // DELETE api/Task/{id}
    [HttpDelete("{id}")]
    public ActionResult<List<SubTaskItem>> Delete(int id)
    {
        // Deleting subtask from database
        if (db.DeleteSubTask(id))
        {
            return Ok(db.GetSubTasks());
        }
        return NotFound();
    }
}
