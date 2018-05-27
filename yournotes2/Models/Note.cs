using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace yournotes.Models
{
    public class Note
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
 
        [Required]
        public String Title { get; set; }
        
        [Required]
        public String Text { get; set; }
        
        [Required]
        public User User { get; set; }
    }
}