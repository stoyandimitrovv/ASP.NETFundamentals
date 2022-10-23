using System.ComponentModel.DataAnnotations;

namespace Library.Data.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; } 
    }
}
