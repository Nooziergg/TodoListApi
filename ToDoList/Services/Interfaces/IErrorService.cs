using ToDoList.Models;

namespace ToDoList.Services.Interfaces
{
    public interface IErrorService
    {
        ErrorResponse HandleError(Exception exception, bool isDevelopment);
    }

}
