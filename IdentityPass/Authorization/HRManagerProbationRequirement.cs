using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityPass.Authorization
{
    public class HRManagerProbationRequirement : IAuthorizationRequirement
    {

        public HRManagerProbationRequirement(int nbrOfProbationMonths)
        {
            this.NbrOfProbationMonths = nbrOfProbationMonths;
        }

        public int NbrOfProbationMonths { get; private set; }
    }


    public class HRManagerProbationRequirementHandler : AuthorizationHandler<HRManagerProbationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context
            , HRManagerProbationRequirement requirement)
        {
            var hasEmployementDateClaim = context.User.HasClaim(x => x.Type == ClaimTypes.EmployementDate);
            if (!hasEmployementDateClaim) return Task.CompletedTask;
            var employementDateClaim = context.User.FindFirst(ClaimTypes.EmployementDate);
            if (employementDateClaim != null)
            {
                var period = DateTime.Now - DateTime.Parse(employementDateClaim.Value);
                if (period.TotalDays > requirement.NbrOfProbationMonths * 30)
                    context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
