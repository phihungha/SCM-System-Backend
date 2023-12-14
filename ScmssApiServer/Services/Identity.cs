using Microsoft.AspNetCore.Identity;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.Exceptions;
using ScmssApiServer.Models;
using System.Security.Claims;

namespace ScmssApiServer.Services
{
    /// <summary>
    /// Contains identity information of current user
    /// such as ID, roles, production facility ID,...
    /// </summary>
    public class Identity
    {
        public required string Id { get; set; }

        public bool IsSuperUser => Roles.Contains("Admin") || Roles.Contains("Director");

        public bool IsFinanceUser => Roles.Contains("Finance");

        public bool IsInProductionFacility => ProductionFacilityId != null;

        public bool IsInventoryUser => Roles.Contains("InventorySpecialist") ||
                                       Roles.Contains("InventoryManager");

        public bool IsProductionUser => Roles.Contains("ProductionManager") ||
                                        Roles.Contains("ProductionSpecialist");

        public bool IsPurchaseUser => Roles.Contains("PurchaseManager") ||
                                      Roles.Contains("PurchaseSpecialist");

        public bool IsSalesUser => Roles.Contains("SalesManager") ||
                                   Roles.Contains("SalesSpecialist");

        public int? ProductionFacilityId { get; set; }

        public IList<string> Roles { get; set; } = new List<string>();

        public static Identity FromClaims(ClaimsPrincipal principal, UserManager<User> userManager)
        {
            string? id = userManager.GetUserId(principal);
            if (id == null)
            {
                throw new UnauthenticatedException("Invalid claims.");
            }

            IList<string> roles = principal.Claims.Where(i => i.Type == ClaimTypes.Role)
                                                  .Select(i => i.Value)
                                                  .ToList();

            string? facilityClaim = principal.Claims
                .Where(i => i.Type == CustomClaimsTransformation.FacilityClaimType)
                .Select(i => i.Value).FirstOrDefault();
            int? productionFacilityId = facilityClaim != null ? int.Parse(facilityClaim) : null;

            return new Identity
            {
                Id = id,
                Roles = roles,
                ProductionFacilityId = productionFacilityId,
            };
        }
    }
}
