using ToDoList.Models.DbModels;

namespace ToDoList.Repository.Interfaces
{
    public interface ITarefaRepository
    {
        IEnumerable<Tarefa> GetAll();
        Tarefa GetById(int id);
        void Add(Tarefa tarefa);
        void Update(Tarefa tarefa);
        void Delete(int id);
        Tarefa GetByName(string name);
        Tarefa GetByOrder(int order);
        void ShiftOrderFrom(int order);
        int GetMaxOrdemApresentacao();

    }
}
