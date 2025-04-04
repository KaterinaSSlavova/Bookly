using Models.Entities;
using ViewModels.Model;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IUserServices
    {
        bool Register(AccountRegister model);
        User? LogIn(AccountLogIn model);
        User? LoadUser();
        bool UpdateProfile(EditProfileModel editModel);
        ProfileOverviewModel LoadProfile();
    }
}
