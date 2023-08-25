using Dapper;
using Microsoft.Data.SqlClient;
using System.Text;
using ToDoList.Data;
using ToDoList.Models.DbModels;
using ToDoList.Models.DTOs;
using ToDoList.Repository.Interfaces;

namespace ToDoList.Repository.Implementations
{
    public class TarefaRepository : BaseRepository<TarefaContext>, ITarefaRepository
    {
        public TarefaRepository(TarefaContext context, string connectionString)
            : base(context, connectionString)
        { }

        // Dapper for reading
        public IEnumerable<Tarefa> GetAll(TarefaFilterDTO filter)
        {
            var conditions = new List<string>();
            var parameters = new DynamicParameters();

            conditions.Add("1=1"); // Always true, a starting point

            if (!string.IsNullOrWhiteSpace(filter.Nome))
            {
                conditions.Add("Nome LIKE @Nome");
                parameters.Add("Nome", $"%{filter.Nome.Trim()}%");
            }

            if (filter.CustoMin.HasValue)
            {
                conditions.Add("Custo >= @CustoMin");
                parameters.Add("CustoMin", filter.CustoMin);
            }

            if (filter.CustoMax.HasValue)
            {
                conditions.Add("Custo <= @CustoMax");
                parameters.Add("CustoMax", filter.CustoMax);
            }

            if (filter.DataLimiteMin.HasValue)
            {
                conditions.Add("DataLimite >= @DataLimiteMin");
                parameters.Add("DataLimiteMin", filter.DataLimiteMin);
            }

            if (filter.DataLimiteMax.HasValue)
            {
                conditions.Add("DataLimite <= @DataLimiteMax");
                parameters.Add("DataLimiteMax", filter.DataLimiteMax);
            }

            string baseQuery = $"SELECT * FROM Tarefas WHERE {string.Join(" AND ", conditions)}";
            string order = "ORDER BY OrdemApresentacao ";
            string pagination = "";
            if (filter.Paginate)
            {

                pagination = " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY ";
                parameters.Add("Offset", (filter.Page - 1) * filter.PageSize);
                parameters.Add("PageSize", filter.PageSize);
            }

            using var connection = CreateConnection();
            connection.Open();
            var result = connection.Query<Tarefa>($"{baseQuery} {order} {pagination}", parameters);
            connection.Close();

            return result;
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

        public Tarefa Update(Tarefa tarefa)
        {
            return base.Update(tarefa);
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
            connection.Open();
            int response = connection.ExecuteScalar<int>(sql);
            connection.Close();
            return response;
        }



    }
}
