using CoreServices.Authentication.Descriptions;
using CoreServices.Authentication.DTOs;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace CoreServices.Authentication
{
    public class JWTAuthenticationService : IJWTAuthenticationService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public JWTAuthenticationService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<string> Authenticate(string webAPILogicalName
            , string webAPIAuthUri
            , string userName
            , string password)
        {
            var httpClient = httpClientFactory.CreateClient(webAPILogicalName);
            var response = await httpClient.PostAsJsonAsync(webAPIAuthUri, new Credential()
            {
                Username = userName,
                Password = password
            });
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }


    }
}
