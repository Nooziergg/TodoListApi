using ToDoList.Models.DbModels;

namespace ToDoList.Services.Interfaces
{
    public interface IErrorService
    {
        ErrorResponse HandleError(Exception exception, bool isDevelopment);
    }

}
