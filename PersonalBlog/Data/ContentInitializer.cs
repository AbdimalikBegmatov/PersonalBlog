using PersonalBlog.Models;

namespace PersonalBlog.Data
{
    public class ContentInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category {Name="Путишествие", Description="Путишествие по всему земному шару." },
                    new Category {Name="История", Description="Рассказанная история-это прожитая жизнь" },
                    new Category {Name="Фильм", Description="Все! Кино не будет! Элекстричества кончилась" }
                    );
                context.SaveChanges();
            }
            if (!context.Publications.Any())
            {
                context.Publications.AddRange(
                    new Publication
                    {
                        Title = "Детройт: хроники мертвого города",
                        Description = "А вам слабо взять и экспромтом поехать в Детройт, штат Мичиган на автосалон, потому что друзья позвали?",
                        Categories = new List<Category>
                        {
                            context.Categories.FirstOrDefault(c=>c.Name.Equals("Путишествие"))
                        },
                        SeoDescription = "Детройт: хроники мертвого города",
                        SeoKeywords = "поездка в детройт"
                    },
                    new Publication
                    {
                        Title = "Достич успеха, меняя образ",
                        Description = "Когда в городе укрепилась высокое мнение добрадетели и познаниях молодого",
                        Categories = new List<Category>
                        {
                            context.Categories.FirstOrDefault(c=>c.Name.Equals("Путишествие")),
                            context.Categories.FirstOrDefault(c=>c.Name.Equals("История"))
                        },
                        SeoDescription = "Достич успеха, меняя образ",
                        SeoKeywords = "Успех в жизни"
                    }
                    );
                context.SaveChanges ();
            }
        }
    }
}
