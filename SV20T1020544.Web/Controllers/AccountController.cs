using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020544.BusinessLayers;
using SV20T1020544.DomainModels;

namespace SV20T1020544.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username = "", string password = "")
        {
            ViewBag.Username = username;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("Error", "Phải nhập tên và mặt khẩu!");
                return View();
            }
            var userAccount = UserAccountService.Authorize(username, password);
            if (userAccount == null)
            {
                ModelState.AddModelError("Error", "Đăng nhập thất bài");
                return View();
            }

            //Đăng nhập thành công, tạo dữ liệu để lưu thông tin đăng nhập
            var userData = new WebUserData()
            {
                UserId = userAccount.UserID,
                UserName = userAccount.UserName,
                DisplayName = userAccount.FullName,
                Email = userAccount.Email,
                Photo = userAccount.Photo,
                ClientIP = HttpContext.Connection.RemoteIpAddress?.ToString(),
                SessionId = HttpContext.Session.Id,
                AdditionalData = "",
                //Roles = new List<string> { WebUserRoles.Employee }
                Roles = userAccount.RoleNames.Split(',').ToList(),
            };
            //thiết lập phiên đăng nhập cho tài khoản
            await HttpContext.SignInAsync(userData.CreatePrincipal());
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenined()
        {
            return View();
        }

        public IActionResult AccountDetails()
        {
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AccountDetails(UserAccount data)
        {
            var userData = new WebUserData()
            {
                UserName = data.UserName,
                DisplayName = data.FullName,
                Email = data.Email,
                Photo = data.Photo,
                Roles = data.RoleNames.Split(",").ToList(),
            };
            await HttpContext.SignInAsync(userData.CreatePrincipal());
            return View(data);
        }

        [HttpPost]
        public IActionResult ChangePassword(string userName, string oldPassword, string newPassword, string confirmPassword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
                    ModelState.AddModelError("ChangePassFailed", "Điền đầy đủ để đổi mật khẩu");
                if (confirmPassword == newPassword)
                {
                    var userAccount = UserAccountService.ChangePassword(userName, oldPassword, newPassword);
                    if (!userAccount)
                    {
                        ModelState.AddModelError("oldPassword", "Mật khẩu cũ không đúng");

                    }
                    else return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("ChangePassFailed", "Xác nhận mật khẩu không hợp lệ");

                return View("Index");
            }
            catch
            {
                ModelState.AddModelError("ChangePassFailed", "Đổi mật khẩu không thành công");
                return View("Index");
            }
        }
    }
}
