using Microsoft.AspNetCore.Mvc;
using ToDoList.Models.DbModels;
using ToDoList.Models.DTOs;
using ToDoList.Services.Interfaces;

/// <summary>
/// Controller responsible for CRUD operations related to Tasks.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TarefasController : ControllerBase
{
    private readonly ITarefaService _service;

    public TarefasController(ITarefaService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retrieves a list of Tasks based on provided filters.
    /// </summary>
    /// <param name="filterDto">Filtering criteria for fetching Tasks.</param>
    /// <returns>A list of Tasks.</returns>
    [HttpGet]
    public IActionResult GetAll([FromQuery] TarefaFilterDTO filterDto)
    {
        var tarefas = _service.GetAll(filterDto);
        return Ok(tarefas);
    }

    /// <summary>
    /// Retrieves a specific Task by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Tasks.</param>
    /// <returns>The Tasks with the specified identifier.</returns>
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var tarefa = _service.GetById(id);

        if (tarefa == null)
        {
            return NotFound();
        }

        return Ok(tarefa);
    }

    /// <summary>
    /// Adds a new Task.
    /// </summary>
    /// <param name="tarefaDto">The details of the Tasks to add.</param>
    /// <returns>The created Task.</returns>
    [HttpPost]
    public IActionResult Add(TarefaInsertDTO tarefaDto)
    {

        var tarefa = new Tarefa
        {
            Nome = tarefaDto.Nome,
            Custo = tarefaDto.Custo,
            DataLimite = tarefaDto.DataLimite
        };

        _service.Add(tarefa);
        return CreatedAtAction(nameof(GetById), new { id = tarefa.Id }, tarefa);
    }

    /// <summary>
    /// Updates the details of an existing Task.
    /// </summary>
    /// <param name="id">The unique identifier of the Task to update.</param>
    /// <param name="tarefaDto">The updated details of the Task.</param>
    /// <returns>No content on success, appropriate error response on failure.</returns>
    [HttpPut("{id}")]
    public IActionResult Update(int id, TarefaUpdateDTO tarefaDto)
    {
        if (id != tarefaDto.Id)
        {
            return BadRequest("Ids não conferem");
        }

        var tarefaExistente = _service.GetById(id);
        if (tarefaExistente == null)
        {
            return NotFound();
        }

        tarefaExistente.Nome = tarefaDto.Nome;
        tarefaExistente.Custo = tarefaDto.Custo;
        tarefaExistente.DataLimite = tarefaDto.DataLimite;
        tarefaExistente.OrdemApresentacao = tarefaDto.OrdemApresentacao;

        _service.Update(tarefaExistente);
        return NoContent();
    }

    /// <summary>
    /// Deletes a specific Task.
    /// </summary>
    /// <param name="id">The unique identifier of the Task to delete.</param>
    /// <returns>No content on success, appropriate error response on failure.</returns>
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var tarefa = _service.GetById(id);

        if (tarefa == null)
        {
            return NotFound();
        }

        _service.Delete(id);
        return NoContent();
    }
}
