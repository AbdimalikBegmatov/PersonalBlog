using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.ViewModels
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public  string Name { get; set; }
        public string Description { get; set; }
    }
}
