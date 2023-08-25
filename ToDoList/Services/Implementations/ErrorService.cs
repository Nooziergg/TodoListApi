using ToDoList.Infrastructure.Common.Exceptions;
using ToDoList.Models.DbModels;
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
        public ErrorResponse HandleError(Exception exception, bool isDevelopment)
        {
            //log the error.
            _errorRepository.LogError(exception);

            var code = exception switch
            {
                ArgumentNullException _ or ArgumentOutOfRangeException _ => 400,// Bad Request
                KeyNotFoundException _ => 404,// Not Found
                UnauthorizedAccessException _ => 401,// Unauthorized
                NotImplementedException _ => 501,// Not Implemented
                TimeoutException _ => 408,// Request Timeout
                InvalidOperationException _ => 409,// Conflict
                _ => 500,// Internal Server Error
            };
            
            var error = new ErrorResponse
            {
                StatusCode = code,
                Message = isDevelopment || exception is BusinessException
                   ? exception.Message
                   : "Unexpected error.",
                Details = isDevelopment ? exception.StackTrace : null
            };    
            error.StatusCode = code;
            return error;
        }
    }

}
