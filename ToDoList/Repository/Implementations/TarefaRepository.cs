using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Repository.Interfaces;

namespace ToDoList.Repository.Implementations
{
    public class TarefaRepository : BaseRepository<TarefaContext>, ITarefaRepository
    {
        public TarefaRepository(TarefaContext context, string connectionString)
            : base(context, connectionString)
        { }

        // Dapper for reading
        public IEnumerable<Tarefa> GetAll()
        {
            return Query<Tarefa>("SELECT * FROM Tarefas ORDER BY OrdemApresentacao");
        }

        public Tarefa GetById(int id)
        {
            return QuerySingle<Tarefa>("SELECT * FROM Tarefas WHERE Id = @Id", new { Id = id });
        }

        // Entity Framework for writing
        public void Add(Tarefa tarefa)
        {
            base.Add(tarefa);
        }

        public void Update(Tarefa tarefa)
        {
            base.Update(tarefa);
        }

        public void Delete(int id)
        {
            var tarefa = _context.Tarefas.Find(id);
            if (tarefa != null)
            {
                base.Remove(tarefa);
            }
        }
    }
}
