using AutoMapper;
using Interfaces;
using Bookly.ViewModels;
using Business_logic.DTOs;
using Exceptions;
using Microsoft.AspNetCore.Mvc;
using Bookly.Filters;

namespace Bookly.Bookly.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServices _userService;
        private readonly IShelfServices _shelfService;
        private readonly IMapper _mapper;
        public UserController(IUserServices userServices, IShelfServices shelfService, IMapper mapper)
        {
            _userService = userServices;
            _shelfService = shelfService;
            _mapper = mapper;
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
                TempData["Error"] = ex.Message;
            }
            catch(EmailAlreadyExistsException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch(InvalidBirthdayException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch(NullReferenceException ex)
            {
                TempData["Error"] = ex.Message;
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
                UserDTO? loggedUser = _userService.GetUserByUsername(user.Username);
                HttpContext.Session.SetString("Username", loggedUser.Username);
                HttpContext.Session.SetString("Role", loggedUser.Role.ToString());
                return RedirectToAction("Index", "Book");
            }
            TempData["Error"] = "Invalid credentials! Please try again!";
            return View(model);
        }

        [HttpPost]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LogIn", "User");
        }

        [FilterLoggedUsers]
        [HttpGet]
        public IActionResult ViewProfile()
        {
            UserDTO? user = _userService.LoadUser();
            ProfileOverviewModel? viewModel = _mapper.Map<ProfileOverviewModel>(user);
            return View(viewModel);
        }

        [FilterLoggedUsers]
        [HttpGet]
        public IActionResult EditProfile()
        {
            UserDTO? user = _userService.LoadUser();
            EditProfileModel viewModel = _mapper.Map<EditProfileModel>(user);
            return View(viewModel);
        }

        [FilterLoggedUsers]
        [HttpPost]
        public IActionResult GoToEdit()
        {
            return RedirectToAction("EditProfile", "User");
        }

        [FilterLoggedUsers]
        [HttpPost]
        public IActionResult SaveChanges(EditProfileModel model, string image)
        {
            try
            {
                UserDTO user = _mapper.Map<UserDTO>(model);
                UserDTO oldUser = _userService.LoadUser();
                user.Id = oldUser.Id;
                if (model.Picture != null)
                {
                    user.Picture = _userService.ConvertToString(model.Picture);
                    _userService.UpdateProfile(user);
                }
                else if (image != null)
                {
                    user.Picture = image;
                    _userService.UpdateProfile(user);
                }
                HttpContext.Session.SetString("Username", user.Username);
                TempData["Success"] = "Profile updated successfully!";
                return RedirectToAction("ViewProfile", "User");

            }
            catch (NullReferenceException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (UsernameAlreadyExistsException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch(EmailAlreadyExistsException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch(InvalidBirthdayException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("EditProfile", "User");
        }
    }
}
