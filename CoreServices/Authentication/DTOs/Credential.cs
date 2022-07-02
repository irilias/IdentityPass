using System;
using System.Collections.Generic;
using System.Text;

namespace CoreServices.Authentication.DTOs
{
    public class Credential
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsHR { get; set; }
        public DateTime EmploymentDate { get; set; }
    }
}
