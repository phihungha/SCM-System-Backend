using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using ScmssApiServer.Models;
using System.Security.Claims;

namespace ScmssApiServer.Services
{
    /// <summary>
    /// Add custom claims (e.g. Production facility ID) to auth cookies.
    /// </summary>
    public class CustomClaimsTransformation : IClaimsTransformation
    {
        public const string FacilityClaimType = "ProductionFacilityId";

        private UserManager<User> _userManager;

        public CustomClaimsTransformation(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            ClaimsPrincipal newPrincipal = principal.Clone();
            var newIdentity = (ClaimsIdentity)newPrincipal.Identity!;

            string userId = _userManager.GetUserId(newPrincipal)!;
            User user = (await _userManager.FindByIdAsync(userId))!;

            if (user.ProductionFacilityId == null)
            {
                return principal;
            }

            var facilityId = (int)user.ProductionFacilityId;
            var facilityClaim = new Claim(FacilityClaimType, facilityId.ToString());
            newIdentity.AddClaim(facilityClaim);

            return newPrincipal;
        }
    }
}
