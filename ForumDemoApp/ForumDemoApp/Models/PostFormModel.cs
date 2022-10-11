using System.ComponentModel.DataAnnotations;

namespace ForumDemoApp.Models
{
    //Creating model for "Add Post"
    public class PostFormModel
	{
        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string Title { get; set; }

        [Required]
        [StringLength(1500, MinimumLength = 30)]
        public string Content { get; set; }
    }
}
