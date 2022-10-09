using System.ComponentModel.DataAnnotations;
using static ForumDemoApp.Data.DataConstants.Post;

namespace ForumDemoApp.Data.Entities
{
    //First create entity Post with attributes
    public class Post
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }
    }
}
