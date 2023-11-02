
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Interface;
using PersonalBlog.Models;
using PersonalBlog.ViewModels;

namespace PersonalBlog.Controllers
{
    [Authorize]
    public class AcountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AcountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            User user = await _userManager.FindByEmailAsync(loginViewModel.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Ползователь с таким email не найдено");
                return View(loginViewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, false, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("Password", "Неправельный пароль");
            return View(loginViewModel);

        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Edit(string email)
        {
            ViewBag.Title = "Редактирование пользователя";
            User user = await _userManager.FindByEmailAsync(email);
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
                    return RedirectToAction("Index", "Home");
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

        public async Task<IActionResult> EditPassword(string email)
        {
            ViewBag.Title = "Изменение пароля";
            User user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel changePasswordViewModel = new ChangePasswordViewModel
            {
                Id = user.Id,
                Email = user.Email
            };
            return View(changePasswordViewModel);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditPassword(ChangePasswordViewModel changePasswordViewModel)
        {
            ViewBag.Title = "Изменение пароля";
            if (!ModelState.IsValid)
            {
                return View(changePasswordViewModel);
            }
            User user = await _userManager.FindByIdAsync(changePasswordViewModel.Id);
            if (user == null)
            {
                return NotFound();
            }

            var result  = await _userManager
                .ChangePasswordAsync(
                user, 
                changePasswordViewModel.OldPassword, 
                changePasswordViewModel.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("Index","Home");
            }
            foreach (var item in result.Errors)
            {
                if (item.Description.Equals("Incorrect password."))
                {
                    ModelState.AddModelError("OldPassword", "Incorrect password.");
                    continue;
                }
                ModelState.AddModelError("",item.Description);
            }
            return View(changePasswordViewModel);
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpGet]
        public async Task<IActionResult> Register([FromServices] IMembership membership, string? code)
        {
            ViewBag.Title = "Регистрация";
            if (!User.Identity.IsAuthenticated || code != null)
            {
                if (await membership.ExistsMembershipAsync(code) && await membership.EnableMembershipAsync(code))
                {
                    return View(new RegisterViewModel { Code = code });
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register([FromServices] IMembership membership, RegisterViewModel rvm)
        {
            ViewBag.Title = "Регистрация";
            if (ModelState.IsValid)
            {
                var usercheck = await _userManager.FindByIdAsync(rvm.Email);
                if (usercheck != null)
                {
                    ModelState.AddModelError("Email","Такой email уже есть");
                    return View(rvm);
                }
                var user = new User { 
                    UserName = rvm.Email, 
                    Email = rvm.Email ,
                    FirstName = rvm.FirstName,
                    LastName = rvm.LastName,
                    DayOfBirth = rvm.DateOfBirth,
                    PhoneNumber = rvm.PhoneNumber
                };
                var result = await _userManager.CreateAsync(user,rvm.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Editor");
                    await membership.DisableMembershipAsync(rvm.Code);
                    if (!User.Identity.IsAuthenticated)
                    {
                        await _signInManager.SignInAsync(user, true);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(rvm);
        }
    }
}
