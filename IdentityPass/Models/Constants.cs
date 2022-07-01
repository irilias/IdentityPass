using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityPass.Models
{
    public class Constants
    {
        public const string CookieAuthScheme = "CookieAuth";
        public const string AuthorizationConfigSection = "Authorization";
        public const string HRManagersProbationPeriodInMonths = "HRManagersProbationPeriodInMonths";
        public const string WebAPILogicalName = "WeatherApi";
        public const string WebAPIAuthUri = "Authentication";
        public const string WebAPIJwtBearerScheme = "Bearer";
        public const string UsersJsonPasswordKey = "Password";
        public const string UsersJsonUsernameKey = "Username";
        public const string UsersJsonFilePath = "users.json";

    }
}
