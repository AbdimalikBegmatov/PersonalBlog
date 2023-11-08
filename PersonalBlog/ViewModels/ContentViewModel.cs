using PersonalBlog.Models;

namespace PersonalBlog.ViewModels
{
    public class ContentViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Publication> Publications { get; set; }
    }
}
