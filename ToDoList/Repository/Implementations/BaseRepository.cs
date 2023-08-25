using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ToDoList.Repository.Implementations
{
    public abstract class BaseRepository<TContext> where TContext : DbContext
    {
        protected readonly TContext _context;
        private readonly string _connectionString;

        protected BaseRepository(TContext context, string connectionString)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        // Manage Connections
        // Create a new connection for Dapper operations
        protected IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        // EF-based generic methods
        protected void Add<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        protected T Update<T>(T entity) where T : class
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
            return entity;
        }

        protected void Remove<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }
      
        protected IEnumerable<T> Query<T>(string sql, object parameters = null)
        {            
            using var connection = CreateConnection();
            connection.Open();
            var result = connection.Query<T>(sql, parameters);
            connection.Close();
            return result;
        }

        protected T QuerySingle<T>(string sql, object parameters = null)
        {
            using var connection = CreateConnection();
            connection.Open();
            var result = connection.QuerySingleOrDefault<T>(sql, parameters);
            connection.Close();
            return result;
        }



    }







}


