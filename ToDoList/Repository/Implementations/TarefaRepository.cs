using Dapper;
using Microsoft.Data.SqlClient;
using ToDoList.Data;
using ToDoList.Models.DbModels;
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
        public Tarefa GetByName(string name)
        {
            return QuerySingle<Tarefa>("SELECT * FROM Tarefas WHERE Nome = @Nome", new { Nome = name });
        }

        public Tarefa GetByOrder(int order)
        {
            return QuerySingle<Tarefa>("SELECT * FROM Tarefas WHERE OrdemApresentacao = @Ordem", new { Ordem = order });
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

        //Dapper for operations where EF is not a good fit
        public void ShiftOrderFrom(int order)
        {
            using var connection = CreateConnection();
            connection.Open();
            var sql = "UPDATE Tarefas SET OrdemApresentacao = OrdemApresentacao + 1 WHERE OrdemApresentacao >= @Ordem";
            connection.Execute(sql, new { Ordem = order });
            connection.Close();            
        }

        public int GetMaxOrdemApresentacao()
        {
            string sql = "SELECT MAX(OrdemApresentacao) FROM Tarefas";

            using var connection = CreateConnection();
            return connection.ExecuteScalar<int>(sql);
        }



    }
}
