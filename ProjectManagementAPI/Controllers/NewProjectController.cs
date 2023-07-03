using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementAPI.Controllers;
using ProjectManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectManagementAPI.Controllers
{
    public class NewProjectController : ControllerBase
    {
        private readonly DatabaseContext? _context;

               public NewProjectController(DatabaseContext context)
       {
           _context = context;
       }


        // Getting all the projects
        // GET: /<controller>/
        [HttpGet]
        public async Task<ActionResult<List<ProjectItem>>> GetAllProjects()
        {
            var projects = await _context.Projects.ToListAsync();

            return Ok(projects);
        }

        // Getting a project by id
        // GET: /<controller>/
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectItem>> GetProjectById(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            return Ok(project);
        }


        // Creating a project
        // POST: /<controller>/
        [HttpPost]
        public async Task<ActionResult<List<ProjectItem>>> CreateProject(ProjectItem project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return Ok(await _context.Projects.ToListAsync());
        }

        // Updating a project
        // PUT: /<controller>/
        [HttpPut("{id}")]
        public async Task<ActionResult<List<ProjectItem>>> UpdateProject(int id, ProjectItem project)
        {
            if (id != project.Project_id)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(await _context.Projects.ToListAsync());
        }

        // Deleting a project
        // DELETE: /<controller>/
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<ProjectItem>>> DeleteProject(int id)
        {
            ProjectItem? project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return Ok(await _context.Projects.ToListAsync());
        }









    }
}



//using EntityFramework7Relationships.Data;
//using EntityFramework7Relationships.DTOs;
//using EntityFramework7Relationships.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace EntityFramework7Relationships.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TlouController : ControllerBase
//    {
//        private readonly DataContext _context;

//        public TlouController(DataContext context)
//        {
//            _context = context;
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<Character>> GetCharacterById(int id)
//        {
//            var character = await _context.Characters
//                .Include(c => c.Backpack)
//                .Include(c => c.Weapons)
//                .Include(c => c.Factions)
//                .FirstOrDefaultAsync(c => c.Id == id);

//            return Ok(character);
//        }

//        [HttpPost]
//        public async Task<ActionResult<List<Character>>> CreateCharacter(CharacterCreateDto request)
//        {
//            var newCharacter = new Character
//            {
//                Name = request.Name,
//            };

//            var backpack = new Backpack { Description = request.Backpack.Description, Character = newCharacter };
//            var weapons = request.Weapons.Select(w => new Weapon { Name = w.Name, Character = newCharacter }).ToList();
//            var factions = request.Factions.Select(f => new Faction { Name = f.Name, Characters = new List<Character> { newCharacter } }).ToList();

//            newCharacter.Backpack = backpack;
//            newCharacter.Weapons = weapons;
//            newCharacter.Factions = factions;

//            _context.Characters.Add(newCharacter);
//            await _context.SaveChangesAsync();

//            return Ok(await _context.Characters.Include(c => c.Backpack).Include(c => c.Weapons).ToListAsync());
//        }
//    }
//}
