﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityPass.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityPass.Pages.HumanResources
{
    [Authorize(Policy = Policies.OnlyAdminHR)]
    public class ManageModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}