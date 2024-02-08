using System.ComponentModel.DataAnnotations;


namespace ToDoApp.Data
{

    public class User
    {
        [Key]
        public int User_id { get; set; } 

        [Required(ErrorMessage = "User name Required.")]
        [StringLength(50)]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Email is  Required.")]
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is  Required.")]
        [StringLength(255)]
        public string? Hash_password { get; set; }

        public string  Created_at { get; set; } = (DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss");



    }

}
