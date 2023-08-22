using ToDoList.Models;
using ToDoList.Repository.Interfaces;
using ToDoList.Services.Interfaces;

namespace ToDoList.Services.Implementations
{
    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _repository;

        public TarefaService(ITarefaRepository repository)
        {
            _repository = repository;
        }

        // Chamadas para o repositório
        public IEnumerable<Tarefa> GetAll()
        {
            return _repository.GetAll();
        }

        public Tarefa GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(Tarefa tarefa)
        {
            _repository.Add(tarefa);
        }

        public void Update(Tarefa tarefa)
        {
            _repository.Update(tarefa);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }        

    }
}
