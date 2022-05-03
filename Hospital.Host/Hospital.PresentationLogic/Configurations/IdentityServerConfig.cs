using IdentityServer4.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Hospital.PresentationLogic.Configurations
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                new ApiResource("hospital")
                {
                    Scopes = new List<Scope>
                    {
                        new Scope("hospital.appointment"),
                        new Scope("hospital.doctor"),
                        new Scope("hospital.interval"),
                        new Scope("hospital.office"),
                        new Scope("hospital.specialization")
                    },
                }
            };
        }

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            return new[]
            {
                new Client
                {
                    ClientId = "hospitalui_pkce",
                    ClientName = "Hospital Web PKCE Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireConsent = false,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    RedirectUris = { $"{configuration["HospitalUI"]}/" },
                    AllowedScopes = { "openid", "profile", "email", "hospital.appointment" }
                },
                new Client
                {
                    ClientId = "hospitalswaggerui",
                    ClientName = "Hospital Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{configuration["HospitalApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{configuration["HospitalApi"]}/swagger/" },

                    AllowedScopes =
                    {
                        "hospital.appointment",
                        "hospital.doctor",
                        "hospital.interval",
                        "hospital.office",
                        "hospital.specialization"
                    }
                },
            };
        }
    }
}