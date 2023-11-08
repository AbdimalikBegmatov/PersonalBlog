using PersonalBlog.Models;

namespace PersonalBlog.Interface
{
    public interface IPublication
    {
        Task<IEnumerable<Publication>> GetAllPublicationsAsync();
        Task<IEnumerable<Publication>> GetAllPublicationsWithCategoriesAsync();
        Task<Publication> GetPublicationAsync(string id);
        Task<Publication> GetPublicationWithCategoriesAsync(string id);


        Task UpdateViewAsync(string id);

        Task AddPublicationAsync(Publication publication);
        Task DeletePublicationAsync(Publication publication);
        Task UpdatePublicationAsync(Publication publication);   
    }
}
