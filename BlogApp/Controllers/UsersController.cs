using System.Security.Claims;
using BlogApp.Data;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{

    public class UsersController : Controller
    {

        private IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated) //sisteme giris yaptiysa zaten postun index ine yonlendirdik. URL den tekrardan logine yönlenmesin diye.
            {
                return RedirectToAction("Index", "Posts");
            }
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        // [HttpPost]
        // public async Task<IActionResult> Register(RegisterViewModel model)
        // {

        //     if (ModelState.IsValid)
        //     {
        //         var user = await _userRepository.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName || x.Email == model.Email);

        //         if (user == null)
        //         {
        //             _userRepository.CreateUser(new User
        //             {
        //                 UserName = model.UserName,
        //                 Name = model.Name,
        //                 Email = model.Email,
        //                 Password = model.Password,
        //                 Image = "avatar.jpg"
        //             });
        //             return RedirectToAction("Login");
        //         }
        //         else
        //         {
        //             ModelState.AddModelError("", "Username ya da Email kullanımda.");
        //         }
        //     }
        //     return View(model);
        // }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {

                var isUser = await _userRepository.Users.FirstOrDefaultAsync(x => x.Email == model.Email && x.Password == model.Password);

                if (isUser != null)
                {
                    var userClaims = new List<Claim>();
                    //kullanici biliglerini aldim.
                    userClaims.Add(new Claim(ClaimTypes.NameIdentifier, isUser.UserId.ToString()));
                    userClaims.Add(new Claim(ClaimTypes.Name, isUser.UserName ?? ""));
                    userClaims.Add(new Claim(ClaimTypes.GivenName, isUser.Name ?? ""));
                    userClaims.Add(new Claim(ClaimTypes.UserData, isUser.Image ?? "")); //kullanicinin gorsel bilgisidir.
                    //auth islemi
                    if (isUser.Email == "info@ahmetkaya.com")
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                    }
                    //kullanicinin sisteme giris yapmasi icn kisiye ozellikler atamam gerek. Sisteme giris/cikis yapti mi vs.
                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var autProperties = new AuthenticationProperties { IsPersistent = true };
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), autProperties //userCClaimsle aldigim name, image gibi bilgilere autProperties bilgilerini ekle ve sisteme giris yaptir.
                    );
                    return RedirectToAction("Index", "Posts"); //post un indexine yonlendir.
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya parola hatalı");
                }
            }

            return View(model);
        }

        public IActionResult Profile(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }
            var user = _userRepository.Users.Include(x => x.Posts).Include(x => x.Comments).ThenInclude(x => x.Post).FirstOrDefault(x => x.UserName == username);

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
    }
}