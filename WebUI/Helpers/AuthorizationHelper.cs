using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUI.Helpers
{
    public static class AuthorizationHelper
    {
        public const string JWT_ISSUER = "PerfectAspNetCoreApplication"; // издатель токена
        public const string JWT_AUDIENCE = null; // потребитель токена
        const string JWT_KEY = "41B9DF4A217BB3C10B1C339358111B0D";   // ключ для шифрации whosyourdaddy md5
        public static TimeSpan JWT_LIFETIME => TimeSpan.FromDays(1); // время жизни токена - 1 день
        public static SymmetricSecurityKey GetSymmetricJWTSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JWT_KEY));
        }
    }

    
}
