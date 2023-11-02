using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Interface;
using PersonalBlog.Models;
using PersonalBlog.ViewModels;

namespace PersonalBlog.Controllers
{
    public class MembershipController : Controller
    {
        private readonly IMembership _membership;

        public MembershipController(IMembership membership)
        {
            _membership = membership;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Генерация прилашений";
            return View(await _membership.GetAllMembershipsAsync());
        }
        public async Task<IActionResult> Generate([FromServices] IHttpContextAccessor httpContextAccessor)
        {
            string code = Guid.NewGuid().ToString();
            string link = HttpContext.Request.Scheme + "://" + httpContextAccessor.HttpContext.Request.Host.Value + "/register?code=" + code;
            var membership = new Membership()
            {
                IsEnable = true,
                Code = code,
                Link = link
            };
            await _membership.AddMembershipAsync(membership);
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int membershipId)
        {
            var currentMembership = await _membership.GetMembershipAsync(membershipId);
            if (currentMembership != null)
            {
                await _membership.DeleteMembershipAsync(currentMembership);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
