using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Repository.Interfaces;

namespace ToDoList.Repository.Implementations
{
    public class ErrorRepository : IErrorRepository
    {
        private readonly TarefaContext _context;

        public ErrorRepository(TarefaContext context)
        {
            _context = context;
        }

        public void LogError(Exception exception)
        {
            var errorLog = new ErrorLog
            {
                ErrorMessage = exception.Message,
                StackTrace = exception.StackTrace,
                DateOccurred = DateTime.UtcNow
            };

            _context.ErrorLogs.Add(errorLog);
            _context.SaveChanges();
        }
    }

}
