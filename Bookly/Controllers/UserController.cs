using AutoMapper;
using Bookly.Business_logic.InterfacesServices;
using Bookly.ViewModels;
using Business_logic.DTOs;
using Business_logic.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

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
                _userService.Register(user);
                _shelfService.CreateDefaultShelf(user.Username);
                return RedirectToAction("LogIn", "User");
            }
            catch (UsernameAlreadyExistsException ex)
            { 
                ViewBag.ErrorMessage = ex.Message;
            }
            catch(EmailAlreadyExistsException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            catch(InvalidBirthdayException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            catch(ServiceValidationException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return View(model);
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

            UserDTO user = _mapper.Map<UserDTO>(model);
            if (_userService.LogIn(user))
            {
                HttpContext.Session.SetString("Username", user.Username);
                return RedirectToAction("Index", "Book");
            }
            ViewBag.ErrorMessage = "Invalid credentials! Please try again!";
            return View(model);
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
            UserDTO? user = _userService.LoadUser();
            ProfileOverviewModel? viewModel = _mapper.Map<ProfileOverviewModel>(user);
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult EditProfile()
        {
            UserDTO? user = _userService.LoadUser();
            EditProfileModel viewModel = _mapper.Map<EditProfileModel>(user);
            return View(viewModel);
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
                UserDTO user = _mapper.Map<UserDTO>(model);
                if (model.Picture != null) _userService.UpdateProfile(user, model.Picture);
                else if (image != null) _userService.UpdateProfile(user, image);
                TempData["ProfileUpdated"] = "Profile updated successfully!";
                return RedirectToAction("ViewProfile", "User");

            }
            catch (ServiceValidationException ex)
            {
                TempData["ProfileError"] = ex.Message;
            }
            catch (UsernameAlreadyExistsException ex)
            {
                TempData["ProfileError"] = ex.Message;
            }
            catch(EmailAlreadyExistsException ex)
            {
                TempData["ProfileError"] = ex.Message;
            }
            catch(InvalidBirthdayException ex)
            {
                TempData["ProfileError"] = ex.Message;
            }
            return RedirectToAction("EditProfile", "User");
        }
    }
}
