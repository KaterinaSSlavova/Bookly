using AutoMapper;
using Bookly.Business_logic.InterfacesServices;
using Business_logic.DTOs;
using Microsoft.AspNetCore.Mvc;
using Bookly.ViewModels;

namespace Bookly.Bookly.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserServices _userService;
        private readonly IShelfServices _shelfService;
        private readonly IMapper _mapper;
        public UserController(IUserServices userServices, IShelfServices shelfService, IMapper mapper, ILogger<UserController> logger)
        {
            _userService = userServices;
            _shelfService = shelfService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(AccountRegister model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                UserDTO user = _mapper.Map<UserDTO>(model);
                if (_userService.Register(user))
                {
                    _shelfService.CreateDefaultShelf(user.Username);
                    return RedirectToAction("LogIn", "User");
                }
                ViewBag.ErrorMessage = "There was an error while registering. Email and username must be unique.";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to create an account: {ErrorMessage}", ex.Message);
                TempData["Error"] = "An unexpected error occurred! Please try again later!";
                return View();
            }
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(AccountLogIn model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                UserDTO user = _mapper.Map<UserDTO>(model);
                if (_userService.LogIn(user))
                {
                    HttpContext.Session.SetString("Username", user.Username);
                    return RedirectToAction("Index", "Book");
                }
                ViewBag.ErrorMessage = "Invalid username or password! Please try again!";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to log into an account: {ErrorMessage}", ex.Message);
                TempData["Error"] = "An unexpected error occurred! Please try again later!";
                return View();
            }
        }

        [HttpPost]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LogIn", "User");
        }

        [HttpGet]
        public IActionResult ViewProfile()
        {
            try
            {
                UserDTO? user = _userService.LoadUser();
                ProfileOverviewModel? viewModel = _mapper.Map<ProfileOverviewModel>(user);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to load account overview page: {ErrorMessage}", ex.Message);
                TempData["ProfileError"] = "An unexpected error occurred! Please try again later!";
                return View();
            }
        }

        [HttpGet]
        public IActionResult EditProfile()
        {
            try
            {
                UserDTO? user = _userService.LoadUser();
                EditProfileModel viewModel = _mapper.Map<EditProfileModel>(user);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to load edit profile page: {ErrorMessage}", ex.Message);
                TempData["ProfileError"] = "An unexpected error occurred! Please try again later!";
                return View();
            }
        }

        [HttpPost]
        public IActionResult GoToEdit()
        {
            return RedirectToAction("EditProfile", "User");
        }

        [HttpPost]
        public IActionResult SaveChanges(EditProfileModel model, string image)
        {
            try
            {
                bool isSaved = false;
                UserDTO user = _mapper.Map<UserDTO>(model);
                if (model.Picture != null) isSaved = _userService.UpdateProfile(user, model.Picture);
                else if (image != null) isSaved = _userService.UpdateProfile(user, image);
                else
                {
                    TempData["ProfileError"] = "Invalid data! Please upload a photo!";
                    return RedirectToAction("EditProfile", "User");
                }

                if (isSaved)
                {
                    TempData["ProfileUpdated"] = "Profile updated successfully!";
                    return RedirectToAction("ViewProfile", "User");
                }
                else
                {
                    TempData["ProfileError"] = "Invalid data!";
                    return RedirectToAction("EditProfile", "User");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to edit an account: {ErrorMessage}", ex.Message);
                TempData["ProfileError"] = "An unexpected error occurred! Please try again later!";
                return RedirectToAction("EditProfile", "User");
            }
        }
    }
}
