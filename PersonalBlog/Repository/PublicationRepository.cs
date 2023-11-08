using Microsoft.EntityFrameworkCore;
using PersonalBlog.Data;
using PersonalBlog.Interface;
using PersonalBlog.Models;

namespace PersonalBlog.Repository
{
    public class PublicationRepository:IPublication
    {
        private readonly ApplicationDbContext _context;

        public PublicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddPublicationAsync(Publication publication)
        {
            if (publication.Categories.Count>0)
            {
                var categoryId = publication.Categories.Select(c=>c.Id).ToArray();
                var allCategories = _context.Categories.Where(c=>categoryId.Contains(c.Id)).ToList();
                publication.Categories = allCategories;
            }
            _context.Publications.Add(publication);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePublicationAsync(Publication publication)
        {
            _context.Publications.Remove(publication);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Publication>> GetAllPublicationsAsync()
        {
            return await _context.Publications.ToListAsync();
        }

        public async Task<IEnumerable<Publication>> GetAllPublicationsWithCategoriesAsync()
        {
            return await _context.Publications.Include(p=>p.Categories).ToListAsync();
        }

        public async Task<Publication> GetPublicationAsync(string id)
        {
            return await _context.Publications.FirstOrDefaultAsync(p => p.Id.ToString().Equals(id));
        }

        public async Task<Publication> GetPublicationWithCategoriesAsync(string id)
        {
            return await _context.Publications.Include(p => p.Categories).FirstOrDefaultAsync(p => p.Id.ToString().Equals(id));
        }

        public async Task UpdatePublicationAsync(Publication publication)
        {
            var categoryId = publication.Categories.Select(c => c.Id).ToArray();
            var allCategory = _context.Categories.Where(c=>categoryId.Contains(c.Id)).ToList();

            var currentPublication = await GetPublicationWithCategoriesAsync(publication.Id.ToString());

            currentPublication.Title = publication.Title;
            currentPublication.Description = publication.Description;
            currentPublication.SeoKeywords = publication.SeoKeywords;
            currentPublication.SeoDescription = publication.SeoDescription;
            currentPublication.FullImageName = publication.FullImageName;
            currentPublication.Image = publication.Image;
            currentPublication.Categories = allCategory;

            _context.Publications.Update(currentPublication);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateViewAsync(string id)
        {
            var result =  _context.Publications.FirstOrDefault(p => p.Id.ToString().Equals(id));
            result.TotalViews += 1;
            await _context.SaveChangesAsync();
        }
    }
}
