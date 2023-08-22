using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Data
{
    public class TarefaContext : DbContext
    {
        public DbSet<Tarefa> Tarefas { get; set; }

        public TarefaContext(DbContextOptions<TarefaContext> options) : base(options) { }
    }
}
