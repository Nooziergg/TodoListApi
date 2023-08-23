﻿namespace ToDoList.Models.DTOs
{
    public class TarefaUpdateDTO
    {
        public int Id { get; set; } 
        public string Nome { get; set; }
        public decimal Custo { get; set; }
        public DateTime DataLimite { get; set; }
        public int OrdemApresentacao { get; set; }
    }
}
