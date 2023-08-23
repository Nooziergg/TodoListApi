namespace ToDoList.Models.DbModels
{
    public class ErrorLog
    {
        public int? Id { get; set; }
        public string? ErrorMessage { get; set; }
        public string? StackTrace { get; set; }
        public DateTime DateOccurred { get; set; }
    }

}
