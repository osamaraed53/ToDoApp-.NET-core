
using Microsoft.EntityFrameworkCore;
using ToDoApp.EF.Models;

namespace ToDoApp.EF.Data;
public  class AppDbContext(DbContextOptions options) : DbContext(options)
{



    public DbSet<User> Users { get; set; }
    public DbSet<UserTask> UserTasks { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}



