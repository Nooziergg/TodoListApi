using ToDoList.Models.DbModels;
using ToDoList.Models.DTOs;

namespace ToDoList.Repository.Interfaces
{
    public interface ITarefaRepository
    {
        IEnumerable<Tarefa> GetAll(TarefaFilterDTO filter);
        Tarefa GetById(int id);
        void Add(Tarefa tarefa);
        Tarefa Update(Tarefa tarefa);
        void Delete(int id);
        Tarefa GetByName(string name);
        Tarefa GetByOrder(int order);
        void ShiftOrderFrom(int order);
        int GetMaxOrdemApresentacao();

    }
}
