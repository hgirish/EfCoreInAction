using Microsoft.AspNetCore.Http;
using System;

namespace ServiceLayer.CheckoutServices.Concrete
{
    public abstract class CookieTemplate
    {
        private readonly string _cookieName;
        private readonly IRequestCookieCollection _cookiesIn;
        private readonly IResponseCookies _cookiesOut;

        protected CookieTemplate(string cookieName,
            IRequestCookieCollection cookiesIn,
            IResponseCookies cookiesOut = null)
        {
            if (cookiesIn == null)
            {
                throw new ArgumentNullException(nameof(cookiesIn));
            }
            _cookieName = cookieName;
            _cookiesIn = cookiesIn;
            _cookiesOut = cookiesOut;
        }

        protected virtual int ExpireInThisManyDays => 0;

        public void AddOrUpdateCookie(string value)
        {
            if (_cookiesOut == null)
            {
                throw new NullReferenceException("You must supply a IResponseCookies value if you want to use this command.");
            }
            var options = new CookieOptions();
            if (ExpireInThisManyDays > 0)
            {
                options.Expires = DateTime.Now.AddDays(ExpireInThisManyDays);
            }
            _cookiesOut.Append(_cookieName, value, options);
        }
        public bool Exists()
        {
            return _cookiesIn[_cookieName] != null;
        }

        public string GetValue()
        {
            var cookie = _cookiesIn[_cookieName];
            return string.IsNullOrEmpty(cookie) ? null : cookie;
        }

        public void DeleteCookie()
        {
            if (_cookiesOut == null)
            {
                throw new NullReferenceException("You must supply a IResponseCookies value if you want to use this command.");

            }
            if (!Exists())
            {
                return;
            }
            var options = new CookieOptions { Expires = DateTime.Now.AddYears(-1) };
            _cookiesOut.Append(_cookieName, "", options);

        }
    }
}
