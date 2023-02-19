using Microsoft.EntityFrameworkCore;
using ToDoListApi.Models;

namespace ToDoListApi.Data
{
    public class DataContext : DbContext
    {
        public DbSet<TodoItem> TodoItems { get; set; } = null!;
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }      
    }
}