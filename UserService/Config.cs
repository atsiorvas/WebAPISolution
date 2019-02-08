using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;

namespace UserService {
    public class Config {
        public static IEnumerable<ApiResource> GetApis() {
            return new List<ApiResource>
            {
                new ApiResource("api")
                {
                    ApiSecrets = { new Secret("secret".Sha256()) }
                }
            };
        }

        public static IEnumerable<Client> GetClients() {
            return new List<Client>
            {
                // JWT
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "api" }
                },
                // reference
                new Client
                {
                    ClientId = "client.reference",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "api" },

                    AccessTokenType = AccessTokenType.Reference
                }
            };
        }
        public static List<TestUser> GetUsers() {
            return new List<TestUser>{
                    new TestUser
                    {
                        SubjectId = "1",
                        Username = "alice",
                        Password = "password"
                    },
                    new TestUser
                    {
                        SubjectId = "2",
                        Username = "bob",
                        Password = "password"
                    }
            };
        }
        //public static IEnumerable<ApiResource> GetApiResources() {
        //}
    }
}
