using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Models;
using PersonalBlog.ViewModels;

namespace PersonalBlog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Route("/users")]
        public IActionResult Index()
        {
            return View(_userManager.Users.ToList());
        }

        [Route("/edit-user")]
        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.Title = "Редактирование пользователя";
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel viewModel = new EditUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DayOfBirth
            };
            return View(viewModel);
        }
        [Route("/edit-user")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)

        {
            ViewBag.Title = "Редактирование пользователя";
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    return NotFound();
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.DayOfBirth = model.DateOfBirth;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction(nameof(Index));
        }

        [Route("user-roles")]
        public async Task<IActionResult> EditRoles(string userId)
        {
            ViewBag.Title = "Изменение ролей ползователя";
            User user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.ToList();

            ChangeRoleViewModel model = new ChangeRoleViewModel
            {
                UserId = userId,
                UserEmail = user.Email,
                UserRoles = (List<string>)userRoles,
                AllRoles = allRoles,
            };
            return View(model);
        }
        [Route("user-roles")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditRoles(string userId, List<string> roles)
        {
            ViewBag.Title = "Изменение ролей ползователя";
            User user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var addedRoles = roles.Except(userRoles);
            var removedRoles = userRoles.Except(roles);

            await _userManager.AddToRolesAsync(user, addedRoles);
            await _userManager.RemoveFromRolesAsync(user, removedRoles);

            return RedirectToAction(nameof(Index));
        }
    }
}
