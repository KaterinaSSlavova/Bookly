using Interfaces;
using Microsoft.AspNetCore.Http;

namespace Business_logic.Helpers
{
    public class SessionHelper: ISessionHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public SessionHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void SetSession(string key, string value)
        {
            _contextAccessor.HttpContext.Session.SetString(key, value);
        }

        public string GetSession(string key)
        {
            return _contextAccessor.HttpContext.Session.GetString(key);
        }
    }
}
