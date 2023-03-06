﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using BookStoreWebApp.Models;
using BookStoreWebApp.Models.DTO;
using BookStoreWebApp.Supporters.Constants;

using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;

namespace BookStoreWebApp.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if(User != null && User.Identity.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            User? user = null;
            if (username != null)
            {
                user = await _userManager.FindByNameAsync(username);
            }

            if(user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);

                if(result.Succeeded)
                {
                    TempData["User"] = user.DisplayName;
                    var roles = await _userManager.GetRolesAsync(user);
                    if(roles.FirstOrDefault().Equals(RoleName.ADMIN))
                    {
                        return RedirectToAction("Index", "Product");
                    }    
                    return RedirectToAction("Index", "Home");
                }
            }
            TempData["err"] = "Incorrect username or password";
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO request)
        {
            if(request.Password != request.ConfirmPassword)
            {
                TempData["confirmPasswordError"] = "Password not match";
                return View(request);
            }    
            if(ModelState.IsValid)
            {
                var user = new User
                {
                    Mail = request.Mail,
                    Password = request.Password,
                    Address= request.Address,
                    Avatar= request.Avatar,
                    DisplayName = request.DisplayName,
                    Phone = request.Phone
                };
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    var IR = await _userManager.AddToRoleAsync(await _userManager.FindByIdAsync(user.Id), RoleName.USER);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                var errorString = "";
                foreach(var error in result.Errors.ToList())
                {
                    errorString += error.Description;
                }
                TempData["generalError"] = errorString;
            }
            else
            {
                TempData["generalError"] = "Your input is not valid, please fill all require field";
            }
            return RedirectToAction("Register", "Account");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
