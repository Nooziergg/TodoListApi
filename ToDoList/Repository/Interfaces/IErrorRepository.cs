namespace ToDoList.Repository.Interfaces
{
    public interface IErrorRepository
    {
        void LogError(Exception exception);
    }

}
