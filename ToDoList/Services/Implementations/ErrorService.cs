using ToDoList.Models;
using ToDoList.Repository.Interfaces;
using ToDoList.Services.Interfaces;

namespace ToDoList.Services.Implementations
{
    public class ErrorService : IErrorService
    {
        private readonly IErrorRepository _errorRepository;

        public ErrorService(IErrorRepository errorRepository)
        {
            _errorRepository = errorRepository;
        }
        //tratamento de erro centralizado, pois é mais rápido de fazer, mas não é interessante em um projeto grande
        //, pois é mais complicado de tratar erros com mais detalhes e não é possível tratar erros de forma específica para cada controller ou método sem fazer gambiarra
        public ErrorResponse HandleError(Exception exception, bool isDevelopment)
        {
            // Optionally, log the error.
            _errorRepository.LogError(exception);

            var code = 500;
            var error = new ErrorResponse
            {
                StatusCode = code,
                Message = isDevelopment ? exception.Message : "Ocorreu um erro inesperado.",
                Details = isDevelopment ? exception.StackTrace : null
            };

            code = exception switch
            {
                ArgumentNullException _ or ArgumentOutOfRangeException _ => 400,// Bad Request
                KeyNotFoundException _ => 404,// Not Found
                UnauthorizedAccessException _ => 401,// Unauthorized
                NotImplementedException _ => 501,// Not Implemented
                TimeoutException _ => 408,// Request Timeout
                InvalidOperationException _ => 409,// Conflict
                _ => 500,// Internal Server Error
            };
            error.StatusCode = code;
            return error;
        }
    }

}
