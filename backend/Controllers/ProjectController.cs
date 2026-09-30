using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;

namespace backend.Controllers;

/*
1. Modtager en request, fx GET /api/project/3
2. Beder repository'et om at hente, oprette, opdatere eller slette i databasen
3. Sender en response tilbage til appen med en statuskode, fx 200, 201, 204 eller 404, og eventuelt data som JSON
*/

[ApiController]
[Route("api/[controller]")]
public class ProjectController(IProjectRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<Project>> GetAll() => await repository.GetAllAsync(); //henter alle rækker

    [HttpGet("{id}")] //henter en bestemt række med vores ID
    public async Task<ActionResult<Project>> GetById(int id)
    {
        var project = await repository.GetByIdAsync(id); //kører den her SQL
        return project is null ? NotFound() : Ok(project); //ingen række har den ID og derfor returner null
    }

    [HttpPost] //Indsætter data
    public async Task<ActionResult<Project>> Create(Project project)
    {
        project.Id = await repository.CreateAsync(project); // Rækken indsættes, og databasen giver id'et tilbage
        return CreatedAtAction(nameof(GetById), new { id = project.Id }, project); 
    }

    [HttpPut("{id}")] // PUT overskriver kun Title og Description
    public async Task<IActionResult> Update(int id, Project project)
    {
        project.Id = id;
        var updated = await repository.UpdateAsync(project);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")] // sletter rækken med Id
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await repository.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
