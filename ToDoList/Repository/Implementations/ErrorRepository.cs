using System;
using ToDoList.Data;
using ToDoList.Models.DbModels;
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
        
            Add(errorLog);
        }
    }
}
