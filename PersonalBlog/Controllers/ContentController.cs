using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PersonalBlog.Interface;
using PersonalBlog.Models;
using PersonalBlog.ViewModels;

namespace PersonalBlog.Controllers
{
    public class ContentController : Controller
    {
        private readonly ICategory _categories;
        private readonly IPublication _publications;
        private readonly IWebHostEnvironment _appEnviroment;

        public ContentController(ICategory categories, IPublication publications, IWebHostEnvironment appEnviroment)
        {
            _categories = categories;
            _publications = publications;
            _appEnviroment = appEnviroment;
        }


        [Route("/content")]
        public async Task<IActionResult> Index()
        {
            return View(new ContentViewModel
            {
                Categories = await _categories.GetAllCategoriesAsync(),
                Publications = await _publications.GetAllPublicationsWithCategoriesAsync(),
            });
        }

        #region Categories

        [Route("/create-category")]
        public IActionResult CreateCategory()
        {
            ViewBag.Title = "Добавление категории";
            return View();
        }

        [Route("/create-category")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateCategory(CategoryViewModel cvm)
        {
            ViewBag.Title = "Добавление категории";
            if (ModelState.IsValid)
            {
                await _categories.AddCategoryAsync(new Category
                {
                    Name = cvm.Name,
                    Description = cvm.Description,
                });
                return RedirectToAction(nameof(Index));
            }
            return View(cvm);
        }

        [Route("edit-category")]
        public async Task<ActionResult> EditCategoryAsync(string id)
        {
            ViewBag.Title = "Редактирование категории";
            var currentCategory = await _categories.GetCategoryAsync(id);
            if (currentCategory is not null)
            {
                return View(new CategoryViewModel
                {
                    Id = currentCategory.Id,
                    Name = currentCategory.Name,
                    Description = currentCategory.Description
                });
            }
            return NoContent();
        }


        [Route("/edti-category")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditCategory(CategoryViewModel cvm)
        {
            ViewBag.Title = "Редактирование категории";
            if (ModelState.IsValid)
            {
                await _categories.UpdateCategoryAsync(new Category
                {
                    Id = cvm.Id,
                    Name = cvm.Name,
                    Description = cvm.Description
                });
                return RedirectToAction(nameof(Index));
            }
            return View(cvm);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<ActionResult> DeleteCategory(string id)
        {
            var currentcategory = await _categories.GetCategoryAsync(id);
            if (currentcategory is not null)
            {
                await _categories.DeleteCategoryAsync(currentcategory);
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Publication

        [Route("/create-publication")]
        public async Task<IActionResult> CreatePublication()
        {
            ViewBag.Title = "Добавление публикации";
            var allCategories = await _categories.GetAllCategoriesAsync();
            var categoriesList = allCategories.Select(c=>new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
            });
            return View(new PublicationViewModel
            {
                SelectListItems = categoriesList,
            });
        }

        [Route("/create-publication")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreatePublication(PublicationViewModel pvm, string[] categories)
        {
            ViewBag.Title = "Добавление публикации";
            if (ModelState.IsValid)
            {
                string? fileImageName = null, imagePath = null;
                if (pvm.File!=null)
                {
                    fileImageName = pvm.File.FileName;
                    if (fileImageName.Contains("\\"))
                    {
                        fileImageName = fileImageName.Substring(fileImageName.LastIndexOf('\\') + 1);
                    }
                    imagePath = "/publicationImages/" + Guid.NewGuid() + fileImageName;
                    using (var filestream = new FileStream(_appEnviroment.WebRootPath+imagePath,FileMode.Create))
                    {
                        await pvm.File.CopyToAsync(filestream);
                    }
                }
                await _publications.AddPublicationAsync(new Publication
                {
                    Title = pvm.Title,
                    Description = pvm.Description,
                    SeoDescription = pvm.SeoDescription,
                    SeoKeywords = pvm.SeoKeywords,
                    FullImageName = fileImageName,
                    Image = imagePath,
                    Categories = categories.Select(c=> new Category
                    {
                        Id = new Guid(c)
                    }).ToList()
                });
                return RedirectToAction(nameof(Index));
            }
            var allCategories = await _categories.GetAllCategoriesAsync();
            pvm.SelectListItems = allCategories.Select(c=> new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
            return View(pvm);
        }

        #endregion
    }
}
