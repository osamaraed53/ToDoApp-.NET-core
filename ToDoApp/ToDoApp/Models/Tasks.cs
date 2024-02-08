using System.ComponentModel.DataAnnotations;
using ToDoApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace ToDoApp.Data
{

    public class User_Task
    {
        public enum TaskStatus
        {
            Pending,
            Completed,
            InProgress,
            Cancelled
        }

        [Key]
        public int Task_id { get; set; }

        public int User_id { get; set; }

        [Required]
        public string? Task_description { get; set; }

        public DateTime? Due_date { get; set; } = DateTime.Now;

        public string Status { get; set; } = TaskStatus.Pending.ToString();
        public string Created_at { get; set; } = (DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss");
    }

}