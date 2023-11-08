using Microsoft.AspNetCore.Mvc.Rendering;
using PersonalBlog.Models;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.ViewModels
{
    public class PublicationViewModel
    {
        [Key]
        public Guid? Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        [Display(Name ="Categories")]
        public IEnumerable<SelectListItem>? SelectListItems { get; set; }
        [Display(Name ="Image")]
        public IFormFile? File { get; set; }
        public string? Image { get; set; }
        public string? ImageFullName { get; set; }
        public string? SeoDescription { get; set; }
        public string? SeoKeywords { get; set; }
    }
}
