using ToDoList.Models;

namespace ToDoList.Services.Interfaces
{
    public interface ITarefaService
    {
        IEnumerable<Tarefa> GetAll();
        Tarefa GetById(int id);
        void Add(Tarefa tarefa);
        void Update(Tarefa tarefa);
        void Delete(int id);
    }
}
