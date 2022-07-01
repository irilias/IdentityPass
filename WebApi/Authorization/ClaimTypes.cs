using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Authorization
{
    public static partial class ClaimTypes
    {
        public const string IsAdmin = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/IsAdmin";
        public const string IsHR = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/IsHR";
        public const string EmployementDate = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/EmployementDate";
    }
}
