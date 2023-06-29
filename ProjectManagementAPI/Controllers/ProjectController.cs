using Microsoft.AspNetCore.Mvc;
using ProjectManagementAPI.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : Controller
    {


        // Creating a database object
        private SQLDatabaseContext db = new SQLDatabaseContext();


        // GET api/Project
        [HttpGet]
        public ActionResult<List<ProjectItem>> GetProjectItems()
        {
            // Getting all projects from database
            // An empty project is returned if none exists
            return Ok(db.GetProjects());
        }

        // GET api/Project/{id}
        [HttpGet("{id}")]
        public ActionResult<ProjectItem> GetProjectItem(int id)
        {

            ProjectItem? project = db.GetProjectById(id);

            if (project != null)
            {
                return Ok(project);
            }
            return NotFound($"Project with id {id} does not exist.");

        }

        // POST api/Project
        [HttpPost]
        public ActionResult<ProjectItem> PostProjectItem([FromBody] ProjectItem project)
        {
            if (db.AddProject(project))
            {
                return Ok(project);
            }
            return BadRequest("Make sure the id of the project is unique");

        }

        // PUT api/Project
        [HttpPut]
        public ActionResult<ProjectItem> PutProjectItem(ProjectItem project)
        {
            if (db.UpdateProject(project))
            {
                return Ok(project);
            }
            return BadRequest("Not allowed to update the project. Make sure not to change the id.");

        }

        // DELETE api/Project/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteProjectItem(int id)
        {
            if (db.DeleteProject(id))
            {
                return Ok($"Project {id} deleted successfully");
            }
            return BadRequest("Project does not exist or have epics depending on it.");

        }


    }

}