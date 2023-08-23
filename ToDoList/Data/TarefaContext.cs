using Microsoft.EntityFrameworkCore;
using ToDoList.Models.DbModels;

namespace ToDoList.Data
{
    public class TarefaContext : DbContext
    {
        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public TarefaContext(DbContextOptions<TarefaContext> options) : base(options) { }
    }
}
