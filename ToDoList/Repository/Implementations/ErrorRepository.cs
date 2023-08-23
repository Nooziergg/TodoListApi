using System;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Repository.Interfaces;

namespace ToDoList.Repository.Implementations
{
    public class ErrorRepository : BaseRepository<TarefaContext>, IErrorRepository
    {
        public ErrorRepository(TarefaContext context, string connectionString)
            : base(context, connectionString)
        {
        }

        public void LogError(Exception exception)
        {
            var errorLog = new ErrorLog
            {
                ErrorMessage = exception.Message,
                StackTrace = exception.StackTrace,
                DateOccurred = DateTime.UtcNow
            };

            // Use the Add method from BaseRepository
            Add(errorLog);
        }
    }
}
