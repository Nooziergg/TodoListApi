using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using ToDoList.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class TarefasController : ControllerBase
{
    private readonly ITarefaService _service;

    public TarefasController(ITarefaService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_service.GetAll());
    }

    // Outros métodos HTTP
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

    [HttpPost]
    public IActionResult Add(Tarefa tarefa)
    {
        _service.Add(tarefa);

        return CreatedAtAction(nameof(GetById), new { id = tarefa.Id }, tarefa);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Tarefa tarefa)
    {
        if (id != tarefa.Id)
        {
            return BadRequest("Ids não conferem");
        }

        var tarefaExistente = _service.GetById(id);

        if (tarefaExistente == null)
        {
            return NotFound();
        }

        _service.Update(tarefa);

        return NoContent();
    }

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
