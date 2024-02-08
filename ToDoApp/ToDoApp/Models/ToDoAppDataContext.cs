using Microsoft.EntityFrameworkCore;
using ToDoApp.CustomException;
//using Microsoft.Extensions.Configuration;
//using System.Collections.Generic;
//using ToDoApp.Data;
//using ZstdSharp; 

namespace ToDoApp.Data
{
    public class ToDoAppDataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<User_Task> Tasks { get; set; }


        //public ToDoAppDataContext(DbContextOptions options) : base(options) { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            //optionsBuilder.UseMySQL(configuration.GetConnectionString("DbConnection") ?? throw new DbException("There is something wrong in the Db Connection"));
        }
    }
}

