using Microsoft.AspNetCore.Mvc;
using ToDoList.Models.DbModels;
using ToDoList.Models.DTOs;
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
    public IActionResult GetAll([FromQuery] TarefaFilterDTO filterDto)
    {
        var tarefas = _service.GetAll(filterDto);
        return Ok(tarefas);
    }

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
