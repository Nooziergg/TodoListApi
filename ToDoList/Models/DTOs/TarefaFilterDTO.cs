namespace ToDoList.Models.DTOs
{
    public class TarefaFilterDTO
    {
        public string? Nome { get; set; }
        public decimal? CustoMin { get; set; }
        public decimal? CustoMax { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
