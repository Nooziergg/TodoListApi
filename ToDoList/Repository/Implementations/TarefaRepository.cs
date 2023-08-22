using Dapper;
using System.Data;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Repository.Interfaces;

namespace ToDoList.Repository.Implementations
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly TarefaContext _context;
        private readonly IDbConnection _dbConnection;

        public TarefaRepository(TarefaContext context, IDbConnection dbConnection)
        {
            _context = context;
            _dbConnection = dbConnection;
        }

        // Dapper para leitura
        public IEnumerable<Tarefa> GetAll()
        {            
            return _dbConnection.Query<Tarefa>("SELECT * FROM Tarefas ORDER BY OrdemApresentacao");
        }

        public Tarefa GetById(int id)
        {
            return _dbConnection.QuerySingleOrDefault<Tarefa>("SELECT * FROM Tarefas WHERE Id = @Id", new { Id = id });
        }

        // Entity Framework para escrita
        public void Add(Tarefa tarefa)
        {            
            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();
        }

        public void Update(Tarefa tarefa)
        {
            _context.Tarefas.Update(tarefa);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var tarefa = _context.Tarefas.Find(id);
            //check if tarefa has been found
            if (tarefa != null)
            {
                _context.Tarefas.Remove(tarefa);
                _context.SaveChanges();
            }
            
        }
    }

}
