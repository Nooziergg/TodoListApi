using ToDoList.Models.DbModels;
using ToDoList.Models.DTOs;

namespace ToDoList.Services.Interfaces
{
    public interface ITarefaService
    {
        IEnumerable<Tarefa> GetAll(TarefaFilterDTO filter);
        Tarefa GetById(int id);
        void Add(Tarefa tarefa);
        Tarefa Update(Tarefa tarefa);
        void Delete(int id);
        int GetMaxOrdemApresentacao();
    }
}
