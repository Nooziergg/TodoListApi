using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ToDoList.Models
{
    public class Tarefa
    {
        public int? Id { get; set; }
        public string Nome { get; set; }
        public decimal Custo { get; set; }    
        public DateTime DataLimite { get; set; }

        public int OrdemApresentacao { get; set; }
    }
}
